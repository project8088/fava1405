import { Project } from 'ts-morph';
const project = new Project();
// مسیر پوشه پروژه Angular خود را اینجا قرار دهید
project.addSourceFilesAtPaths('src/app/**/*.ts');
const allSourceFiles = project.getSourceFiles();
const modules = [];
const components = [];
// یافتن تمام ماژول‌ها و کامپوننت‌ها
allSourceFiles.forEach((sourceFile) => {
    const moduleDeclarations = sourceFile
        .getClasses()
        .filter((cls) => cls.getDecorators().some((dec) => dec.getName() === 'NgModule'));
    moduleDeclarations.forEach((cls) => {
        const decorator = cls.getDecorators().find((dec) => dec.getName() === 'NgModule');
        if (decorator) {
            modules.push({
                name: cls.getName(),
                path: sourceFile.getFilePath(),
                ngModule: decorator.getArguments()[0],
            });
        }
    });
    const componentDeclarations = sourceFile
        .getClasses()
        .filter((cls) => cls.getDecorators().some((dec) => dec.getName() === 'Component'));
    componentDeclarations.forEach((cls) => {
        const decorator = cls.getDecorators().find((dec) => dec.getName() === 'Component');
        if (decorator) {
            components.push({
                name: cls.getName(),
                path: sourceFile.getFilePath(),
                ngComponent: decorator.getArguments()[0],
            });
        }
    });
});
// تجزیه و تحلیل وابستگی‌ها
modules.forEach((moduleInfo) => {
    const moduleDecoratorArgs = moduleInfo.ngModule;
    const importedModulesNames = moduleDecoratorArgs?.imports?.map((imp) => imp.getText().replace('Module', '')) || [];
    const declaredComponentsNames = moduleDecoratorArgs?.declarations?.map((dec) => dec.getText().replace('Component', '')) ||
        [];
    // بررسی کامپوننت‌های تعریف شده در این ماژول
    declaredComponentsNames.forEach((compName) => {
        const componentInfo = components.find((c) => c.name.startsWith(compName));
        if (componentInfo) {
            // بررسی اینکه آیا کامپوننت در ماژول دیگری استفاده شده است یا خیر
            modules.forEach((otherModuleInfo) => {
                if (otherModuleInfo.name !== moduleInfo.name) {
                    const otherModuleDecoratorArgs = otherModuleInfo.ngModule;
                    const otherImportedModulesNames = otherModuleDecoratorArgs?.imports?.map((imp) => imp.getText().replace('Module', '')) || [];
                    if (otherImportedModulesNames.includes(moduleInfo.name.replace('Module', ''))) {
                        // ماژول فعلی در ماژول دیگری import شده است
                        // حالا بررسی کنید که آیا کامپوننت تعریف شده در ماژول فعلی، در آن ماژول دیگر استفاده شده است یا خیر
                        const otherDeclaredComponentsNames = otherModuleDecoratorArgs?.declarations?.map((dec) => dec.getText().replace('Component', '')) || [];
                        if (!otherDeclaredComponentsNames.includes(compName)) {
                            console.error(`خطا: کامپوننت '${componentInfo.name}' در ماژول '${moduleInfo.name}' تعریف شده است، اما در ماژول '${otherModuleInfo.name}' استفاده نشده است.`);
                        }
                    }
                }
            });
        }
    });
    // بررسی ماژول‌هایی که import شده‌اند
    importedModulesNames.forEach((importedModuleName) => {
        const importedModuleInfo = modules.find((m) => m.name.startsWith(importedModuleName));
        if (importedModuleInfo) {
            // بررسی اینکه آیا ماژول import شده، کامپوننت تعریف شده در ماژول فعلی را import کرده است یا خیر
            const importedModuleDecoratorArgs = importedModuleInfo.ngModule;
            const importedModuleDeclaredComponentsNames = importedModuleDecoratorArgs?.declarations?.map((dec) => dec.getText().replace('Component', '')) || [];
            if (!importedModuleDeclaredComponentsNames.includes(moduleInfo.name.replace('Module', ''))) {
                // این بخش نیاز به بررسی دقیق‌تری دارد تا مشخص شود کدام کامپوننت از ماژول import شده استفاده می‌کند.
                // این مثال فقط بررسی می‌کند که آیا کامپوننت‌ها در ماژول import شده، کامپوننت‌های ماژول فعلی را declare کرده‌اند یا خیر.
            }
        }
    });
});
console.log('بررسی وابستگی‌ها تکمیل شد.');
