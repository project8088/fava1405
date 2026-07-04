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

  const decorator = classDecl.getDecorator('Component');
  const args = decorator.getArguments();
  const objArg = args[0];

  // @Component() بدون هیچ آرگومانی (به‌ندرت پیش میاد)
  if (!objArg) {
    decorator.addArgument('{\n  standalone: false,\n}');
    sourceFile.saveSync();
    console.log(`✔ اضافه شد (بدون آرگومان قبلی): ${filePath}`);
    project.removeSourceFile(sourceFile);
    return 'changed';
  }

  if (objArg.getKind() !== SyntaxKind.ObjectLiteralExpression) {
    console.log(`⚠ رد شد (آرگومان دکوریتور object literal نیست): ${filePath}`);
    project.removeSourceFile(sourceFile);
    return 'skipped';
  }

  if (objArg.getProperty('standalone')) {
    console.log(`- رد شد (standalone از قبل تعریف شده): ${filePath}`);
    project.removeSourceFile(sourceFile);
    return 'skipped';
  }

  if (objArg.getProperty('imports')) {
    console.log(`- رد شد (imports داره یعنی خودش standalone هست): ${filePath}`);
    project.removeSourceFile(sourceFile);
    return 'skipped';
  }

  objArg.addPropertyAssignment({
    name: 'standalone',
    initializer: 'false',
  });

  sourceFile.saveSync();
  console.log(`✔ اضافه شد standalone: false: ${filePath}`);

  project.removeSourceFile(sourceFile);
  return 'changed';
}
