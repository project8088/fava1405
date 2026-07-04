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

const globPattern = process.argv[2] || '../src/**/*.ts';
const includeSpec = process.argv.includes('--include-spec');

let files = globSync(globPattern, { nodir: true });

if (!includeSpec) {
  files = files.filter((f) => !f.endsWith('.spec.ts') && !f.endsWith('.d.ts'));
}

if (files.length === 0) {
  console.log('هیچ فایلی با الگوی', globPattern, 'پیدا نشد.');
  process.exit(0);
}

console.log(`${files.length} فایل پیدا شد. شروع بررسی ایمپورت‌ها...\n`);

let changedFiles = 0;
let removedTotal = 0;

for (const filePath of files) {
  try {
    const { changed, removed } = processFile(filePath);
    if (changed) {
      changedFiles++;
      removedTotal += removed;
    }
  } catch (err) {
    console.error(`❌ خطا در فایل ${filePath}:`, err.message);
  }
}

console.log(`\nپایان. ${changedFiles} فایل تغییر کرد، مجموعاً ${removedTotal} ایمپورت حذف شد.`);

function processFile(filePath) {
  const sourceFile = project.addSourceFileAtPath(filePath);
  const importDecls = sourceFile.getImportDeclarations();

  let removed = 0;
  const removedNames = [];

  for (const importDecl of [...importDecls]) {
    const defaultImport = importDecl.getDefaultImport();
    const namespaceImport = importDecl.getNamespaceImport();
    const namedImports = importDecl.getNamedImports();

    // ایمپورت side-effect مثل import 'zone.js'; رو دست نزن
    if (!defaultImport && !namespaceImport && namedImports.length === 0) {
      continue;
    }

    // بررسی named imports: import { A, B } from '...'
    for (const ni of [...namedImports]) {
      const name = ni.getAliasNode() ? ni.getAliasNode().getText() : ni.getName();
      if (!isUsedElsewhere(sourceFile, importDecl, name)) {
        removedNames.push(name);
        ni.remove();
        removed++;
      }
    }

    // بررسی default import: import X from '...'
    if (importDecl.getDefaultImport()) {
      const name = importDecl.getDefaultImport().getText();
      if (!isUsedElsewhere(sourceFile, importDecl, name)) {
        removedNames.push(name);
        importDecl.removeDefaultImport();
        removed++;
      }
    }

    // بررسی namespace import: import * as X from '...'
    if (importDecl.getNamespaceImport()) {
      const name = importDecl.getNamespaceImport().getText();
      if (!isUsedElsewhere(sourceFile, importDecl, name)) {
        removedNames.push(name);
        importDecl.removeNamespaceImport();
        removed++;
      }
    }

    // اگه دیگه چیزی از این import نمونده، کل خط رو حذف کن
    const stillHasDefault = importDecl.getDefaultImport();
    const stillHasNamespace = importDecl.getNamespaceImport();
    const stillHasNamed = importDecl.getNamedImports().length > 0;

    if (!stillHasDefault && !stillHasNamespace && !stillHasNamed) {
      importDecl.remove();
    }
  }

  if (removed > 0) {
    sourceFile.saveSync();
    console.log(`✔ ${filePath} -> حذف شد: ${removedNames.join(', ')}`);
  }

  project.removeSourceFile(sourceFile);
  return { changed: removed > 0, removed };
}

// چک می‌کنه که آیا "name" جایی خارج از خودِ import declaration استفاده شده یا نه
function isUsedElsewhere(sourceFile, importDecl, name) {
  const start = importDecl.getStart();
  const end = importDecl.getEnd();

  const identifiers = sourceFile.getDescendantsOfKind(SyntaxKind.Identifier);

  for (const id of identifiers) {
    if (id.getText() !== name) continue;
    const pos = id.getStart();
    if (pos >= start && pos < end) continue; // خود import رو رد کن
    return true;
  }
  return false;
}
