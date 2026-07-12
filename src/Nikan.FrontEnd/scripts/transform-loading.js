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
      // اول لیست را بگیر
      const calls = [...method.getDescendantsOfKind(SyntaxKind.CallExpression)];

      for (const call of calls) {
        const expression = call.getExpression();

        if (!expression.isKind(SyntaxKind.PropertyAccessExpression)) continue;

        const propertyAccess = expression;

        if (propertyAccess.getName() !== 'subscribe') continue;

        const serviceChain = propertyAccess.getExpression();

        const chainText = serviceChain.getText();

        if (
          !/this\.dataService\s*\.\s*(get|post|put|delete|patch)\s*\(/.test(
            chainText.replace(/\s+/g, ' '),
          )
        ) {
          continue;
        }

        // اگر قبلا finalize اضافه شده رد کن
        if (chainText.includes('finalize(')) continue;

        // پیدا کردن loading=true
        const loadingAssignment = method
          .getDescendantsOfKind(SyntaxKind.BinaryExpression)
          .find((expr) => {
            return (
              expr.getOperatorToken().getKind() === SyntaxKind.EqualsToken &&
              expr.getLeft().getText().startsWith('this.') &&
              expr.getRight().getKind() === SyntaxKind.TrueKeyword &&
              expr.getStart() < call.getStart()
            );
          });

        if (!loadingAssignment) continue;

        const loadingProp = loadingAssignment.getLeft().getText();

        // فقط متن را قبل از replace بگیر
        const serviceText = serviceChain.getText();
        const subscribeArgs = call
          .getArguments()
          .map((a) => a.getText())
          .join(', ');

        const newText = `${serviceText}
.pipe(
  finalize(() => {
    ${loadingProp} = false;
    this.chdr.detectChanges();
  }),
)
.subscribe(${subscribeArgs})`;

        call.replaceWithText(newText);

        changed = true;

        // مهم
        break;
      }
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
