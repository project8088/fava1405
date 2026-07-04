 /**
 * refactor-components.js
 *
 * این اسکریپت تمام فایل‌های *.component.ts پروژه رو پیدا می‌کنه،
 * کلاس کامپوننت رو از AppBase ارث‌بری می‌ده و وابستگی‌هایی که از قبل
 * داخل AppBase با inject() ساخته شدن رو از سازنده (constructor) حذف می‌کنه.
 *
 * نصب پیش‌نیاز:
 *   npm install ts-morph glob
 *
 * اجرا (از ریشه پروژه انگیولار):
 *   node refactor-components.js
 *   یا با الگوی دلخواه:
 *   node refactor-components.js "projects/**\/*.component.ts"
 *
 * ⚠️ حتماً قبل از اجرا commit بگیر یا روی یک برنچ جدا تست کن،
 * چون این اسکریپت فایل‌ها رو مستقیم روی دیسک بازنویسی می‌کنه.
 *
 * محدودیت شناخته‌شده:
 * فقط الگوی "parameter property" رو تشخیص می‌ده، یعنی:
 *   constructor(private dataService: DataService) {}
 * اگر سرویسی بدون private/public/protected در سازنده گرفته میشه و
 * دستی به یک فیلد دیگه assign میشه، این مورد رو باید دستی چک کنی.
 */

const path = require('path');
const fs = require('fs');
const { Project, SyntaxKind, Node } = require('ts-morph');
const { globSync } = require('glob');

// ------------------------------------------------------------------
// تنظیمات قابل تغییر
// ------------------------------------------------------------------

const APP_BASE_IMPORT_PATH = '@app/app.base';
const APP_BASE_CLASS_NAME = 'AppBase';

// نگاشت: "اسمی که در AppBase استفاده شده" -> "نوع (type) آن"
// اگر پراپرتی جدیدی به AppBase اضافه کردی، همینجا هم اضافه کن
const BASE_PROPS = {
  dataService: 'DataService',
  route: 'ActivatedRoute',
  router: 'Router',
  toastrService: 'ToastrService',
  fb: 'FormBuilder',
  authService: 'AuthService',
  matDialog: 'MatDialog',
};

// نگاشت معکوس بر اساس type، برای پیدا کردن اسم استاندارد در AppBase
const TYPE_TO_BASE_NAME = {};
for (const [name, type] of Object.entries(BASE_PROPS)) {
  TYPE_TO_BASE_NAME[type] = name;
}

const globPattern = process.argv[2] || '../src/**/*.component.ts';

// ------------------------------------------------------------------
// راه‌اندازی پروژه ts-morph
// ------------------------------------------------------------------

function findTsConfig() {
  const candidate = path.resolve(process.cwd(), '../tsconfig.json');
  return fs.existsSync(candidate) ? candidate : undefined;
}
 

const project = new Project({
  tsConfigFilePath: findTsConfig(),
  skipAddingFilesFromTsConfig: true,
});

const files = globSync(globPattern, { nodir: true });

if (files.length === 0) {
  console.log('هیچ فایلی با الگوی', globPattern, 'پیدا نشد.');
  process.exit(0);
}

console.log(`${files.length} فایل پیدا شد. شروع پردازش...\n`);

let changedCount = 0;
let skippedCount = 0;

for (const filePath of files) {
  try {
    const result = processFile(filePath);
    if (result === 'changed') changedCount++;
    else if (result === 'skipped') skippedCount++;
  } catch (err) {
    console.error(`❌ خطا در فایل ${filePath}:`, err.message);
  }
}

console.log(`\nپایان. ${changedCount} فایل تغییر کرد، ${skippedCount} فایل رد شد.`);

// ------------------------------------------------------------------
// پردازش هر فایل
// ------------------------------------------------------------------

function processFile(filePath) {
  const sourceFile = project.addSourceFileAtPath(filePath);

  const classDecl = sourceFile
    .getClasses()
    .find((c) => c.getDecorator('Component'));

  if (!classDecl) {
    console.log(`- رد شد (کلاس @Component پیدا نشد): ${filePath}`);
    project.removeSourceFile(sourceFile);
    return 'skipped';
  }

  const existingExtends = classDecl.getExtends();
  if (existingExtends) {
    if (existingExtends.getText() === APP_BASE_CLASS_NAME) {
      console.log(`- رد شد (از قبل AppBase داره): ${filePath}`);
    } else {
      console.log(
        `⚠ رد شد (از کلاس دیگه‌ای ارث‌بری کرده: ${existingExtends.getText()}): ${filePath}`
      );
    }
    project.removeSourceFile(sourceFile);
    return 'skipped';
  }

  // ۱. اضافه کردن extends AppBase
  classDecl.setExtends(APP_BASE_CLASS_NAME);

  // ۲. اصلاح constructor
  const ctor = classDecl.getConstructors()[0];

  if (ctor) {
    const params = ctor.getParameters();
    const removedTypeNames = new Set();

    for (const param of [...params]) {
      const typeNode = param.getTypeNode();
      const typeText = typeNode ? typeNode.getText() : null;

      if (typeText && TYPE_TO_BASE_NAME[typeText]) {
        const oldName = param.getName();
        const baseName = TYPE_TO_BASE_NAME[typeText];

        // اگر اسم لوکال با اسم استاندارد AppBase فرق داره،
        // همه‌ی this.oldName داخل کلاس رو به this.baseName تغییر بده
        if (oldName !== baseName) {
          renameThisUsages(classDecl, oldName, baseName);
        }

        removedTypeNames.add(typeText);
        param.remove();
      }
    }

    // اضافه کردن super() به عنوان اولین خط بدنه constructor
    const body = ctor.getBody();
    if (body) {
      const statements = body.getStatements();
      const hasSuperCall = statements.some(
        (s) =>
          Node.isExpressionStatement(s) &&
          s.getExpression().getText().startsWith('super(')
      );
      if (!hasSuperCall) {
        body.insertStatements(0, 'super();');
      }
    }

    cleanUnusedImports(sourceFile, removedTypeNames);
  }

  // ۳. اضافه کردن ایمپورت AppBase (اگه از قبل نیست)
  const hasAppBaseImport = sourceFile
    .getImportDeclarations()
    .some((imp) => imp.getModuleSpecifierValue() === APP_BASE_IMPORT_PATH);

  if (!hasAppBaseImport) {
    sourceFile.addImportDeclaration({
      namedImports: [APP_BASE_CLASS_NAME],
      moduleSpecifier: APP_BASE_IMPORT_PATH,
    });
  }

  sourceFile.saveSync();
  console.log(`✔ Edited: ${filePath}`);

  project.removeSourceFile(sourceFile);
  return 'changed';
}

// ------------------------------------------------------------------
// تغییر this.oldName -> this.newName در کل کلاس
// ------------------------------------------------------------------

function renameThisUsages(classDecl, oldName, newName) {
  const propertyAccesses = classDecl.getDescendantsOfKind(
    SyntaxKind.PropertyAccessExpression
  );

  for (const pa of propertyAccesses) {
    if (
      pa.getExpression().getKind() === SyntaxKind.ThisKeyword &&
      pa.getName() === oldName
    ) {
      pa.getNameNode().replaceWithText(newName);
    }
  }
}

// ------------------------------------------------------------------
// حذف ایمپورت‌هایی که بعد از حذف پارامترها دیگه استفاده نمیشن
// ------------------------------------------------------------------

function cleanUnusedImports(sourceFile, candidateTypeNames) {
  for (const typeName of candidateTypeNames) {
    const importDecl = sourceFile
      .getImportDeclarations()
      .find((imp) =>
        imp.getNamedImports().some((ni) => ni.getName() === typeName)
      );

    if (!importDecl) continue;

    const identifiers = sourceFile.getDescendantsOfKind(SyntaxKind.Identifier);
    const usageCount = identifiers.filter((id) => id.getText() === typeName)
      .length;

    // یک بار استفاده همیشه خود import specifier هست
    // پس اگه usageCount <= 1 یعنی جای دیگه‌ای استفاده نشده
    if (usageCount <= 1) {
      const namedImport = importDecl
        .getNamedImports()
        .find((ni) => ni.getName() === typeName);
      namedImport.remove();

      if (importDecl.getNamedImports().length === 0) {
        importDecl.remove();
      }
    }
  }
}