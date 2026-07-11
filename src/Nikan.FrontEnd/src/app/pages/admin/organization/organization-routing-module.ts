import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminOrganizationListComponent } from './organization-list/organization-list.component';
import { AdminOrganizationUnitGroupsComponent } from './organization-unit-groups/organization-unit-groups.component';
import { AdminUnitListComponent } from './unit-list/unit-list.component';

const routes: Routes = [
  { path: '', component: AdminOrganizationListComponent },
  { path: 'units/:id', component: AdminUnitListComponent },
  { path: 'organization-unit-groups/:id', component: AdminOrganizationUnitGroupsComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OrganizationRoutingModule {}
