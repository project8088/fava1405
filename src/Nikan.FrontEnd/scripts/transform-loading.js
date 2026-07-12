const path = require('path');
const fs = require('fs');
const { Project, SyntaxKind } = require('ts-morph');
const { globSync } = require('glob');

function findTsConfig() {
  const candidate = path.resolve(process.cwd(), '../tsconfig.json');
  return fs.existsSync(candidate) ? candidate : undefined;
}

const project = new Project({
  tsConfigFilePath: findTsConfig(),
  skipAddingFilesFromTsConfig: true,
});

const globPattern = process.argv[2] || '../src/**/*.component.ts';
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
    else skippedCount++;
  } catch (err) {
    console.error(`❌ خطا در فایل ${filePath}:`, err.message);
  }
}

console.log(`\nپایان. ${changedCount} فایل تغییر کرد، ${skippedCount} فایل رد شد.`);
function normalizeWs(s) {
  return s.replace(/\s+/g, ' ');
}
function processFile(filePath) {
  const sourceFile = project.addSourceFileAtPath(filePath);
  let changed = false;

  sourceFile.getClasses().forEach((classDecl) => {
    classDecl.getMethods().forEach((method) => {
      // پیدا کردن تمام فراخوانی‌های subscribe
      method.getDescendantsOfKind(SyntaxKind.CallExpression).forEach((call) => {
        const expression = call.getExpression();
        if (expression.getKind() !== SyntaxKind.PropertyAccessExpression) return;

        const propertyAccess = expression.asKind(SyntaxKind.PropertyAccessExpression);
        if (propertyAccess.getName() !== 'subscribe') return;

        // ۱. پیدا کردن زنجیره سرویس (مثلاً this.dataService.post(...))
        const serviceChain = propertyAccess.getExpression();
        const chainText = serviceChain.getText().replace(/\s+/g, ' ');

        const isTargetService = /dataService\.(get|post|put|delete|patch)/.test(chainText);

        if (isTargetService) {
          // ۲. پیدا کردن متغیر loading (isSaving = true)
          const assignments = method.getDescendantsOfKind(SyntaxKind.AssignmentExpression);
          const loadingAssignment = assignments.find((a) => {
            const leftText = a.getLeft().getText();
            const rightText = a.getRight().getText().trim(); // trim برای حذف \r یا \n
            return (
              leftText.startsWith('this.') && rightText === 'true' && a.getStart() < call.getStart()
            );
          });

          if (loadingAssignment) {
            const loadingProp = loadingAssignment.getLeft().getText();

            // جلوگیری از تکرار
            if (method.getText().includes('finalize')) return;

            // ۳. استراتژی جایگزینی هوشمند:
            // ما باید کل عبارت serviceChain.subscribe(...) را بگیریم
            // و آن را به serviceChain.pipe(finalize(...)).subscribe(...) تبدیل کنیم.

            // برای اینکه در subscribe فعلی (با تمام آرگومان‌هایش) دست نبردیم:
            // از متد replaceWithText روی خودِ 'call' (یعنی همان subscribe) استفاده می‌کنیم

            // اول متنِ قبل از subscribe را می‌گیریم (یعنی serviceChain)
            const servicePartText = serviceChain.getText();

            // متنِ کلِ فراخوانی subscribe را می‌گیریم (شامل پرانتزها و callback ها)
            const subscribePartText = call.getText();

            // حالا یک جراحی می‌کنیم:
            // باید پیدا کنیم subscribe از کجا شروع شده و کل عبارت را با pipe جدید بسازیم
            // اما ساده‌ترین راه این است که کل 'call' را جایگزین کنیم
            // با استفاده از متد realize:

            const newExpression = `${servicePartText}.pipe(finalize(() => { ${loadingProp} = false; this.chdr.detectChanges(); })).subscribe(arguments_here)`;

            // اما چون آرگومان‌ها (callback ها) پیچیده‌اند، نباید از رشته استفاده کنیم.
            // راه درست این است که خودِ subscribe را به یک زنجیره جدید تبدیل کنیم:

            // پیدا کردن شروعِ پرانتزِ subscribe
            const openParenthesis = call.getArguments()[0]
              ? call.getArguments()[0].getStart()
              : call.getStart();
            // در ts-morph، CallExpression خودش شامل پرانتزهاست.

            // بهترین راه: ایجاد یک CallExpression جدید برای pipe و سپس اتصال آن به subscribe فعلی
            // اما چون ts-morph در ایجاد زنجیره پیچیده سخت است، از این ترفند استفاده می‌کنیم:

            const fullCallText = call.getText(); // تمام عبارت: .subscribe((data)=>..., (err)=>...)
            // پیدا کردن نقطه شروع subscribe در متن اصلی
            const subscribeIndex = fullCallText.indexOf('.subscribe');

            // جدا کردن بخش قبل از subscribe و بخش خود subscribe
            // در واقع ما می‌خواهیم قبل از .subscribe، عبارت .pipe(...) را تزریق کنیم

            // برای جلوگیری از پیچیدگی، از روش String Manipulation روی خودِ call استفاده می‌کنیم
            // که بسیار امن‌تر از روش قبلی است:
            const updatedCallText = fullCallText.replace(
              '.subscribe',
              `.pipe(finalize(() => { ${loadingProp} = false; this.chdr.detectChanges(); })).subscribe`,
            );

            call.replaceWithText(updatedCallText);
            changed = true;
          }
        }
      });
    });
  });

  if (changed) {
    // مدیریت Import
    const rxjsImport = sourceFile.getImportDeclaration(
      (i) => i.getModuleSpecifierValue() === 'rxjs',
    );
    if (!rxjsImport) {
      sourceFile.addImportDeclaration({ moduleSpecifier: 'rxjs', namedImports: ['finalize'] });
    } else if (!rxjsImport.getNamedImports().find((n) => n.getName() === 'finalize')) {
      rxjsImport.addNamedImport('finalize');
    }
    sourceFile.saveSync();
    console.log(`✔ آپدیت شد: ${filePath}`);
    return 'changed';
  }

  project.removeSourceFile(sourceFile);
  return 'skipped';
}
