import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminManzalatBaseFromListComponent } from './manzalat-base-list/manzalat-base-list.component';
import { ManzalatSettingComponent } from './manzalat-setting/manzalat-setting.component';
import { AdminUpdateManzalatBaseFormComponent } from './update-manzalat-base-form/update-manzalat-base-form.component';

const routes: Routes = [
  { path: 'manzalat-form-list', component: AdminManzalatBaseFromListComponent },
  { path: 'update-manzalat-form/:id', component: AdminUpdateManzalatBaseFormComponent },
  { path: 'manzelat-settings', component: ManzalatSettingComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ManzelatRoutingModule {}
