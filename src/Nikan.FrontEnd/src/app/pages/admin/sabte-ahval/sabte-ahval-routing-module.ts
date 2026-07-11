import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminCheckStateLifeListComponent } from './chek-state-life-list/check-state-life-list.component';
import { AdminSabtAhvalCitizensListComponent } from './get-sabtAhval-citizen-list/get-sabtAhval-citizen-list.component';
import { AdminSabtAhvalListComponent } from './sabtAhval-list/sabtAhval-list.component';

const routes: Routes = [
  { path: '', component: AdminSabtAhvalListComponent },
  { path: 'sabtAhval-citizens/:id', component: AdminSabtAhvalCitizensListComponent },
  { path: 'check-state-life-list/:id', component: AdminCheckStateLifeListComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SabteAhvalRoutingModule {}
