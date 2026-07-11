import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OrganizationRoutingModule } from './organization-routing-module';
import { AdminOrganizationListComponent } from './organization-list/organization-list.component';
import { AdminOrganizationUnitGroupsComponent } from './organization-unit-groups/organization-unit-groups.component';
import { AdminUnitListComponent } from './unit-list/unit-list.component';
import { CoreModule } from '@core/core.module';
import { MaterialModule } from '@core/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AdminOrganizationListComponent,
    AdminUnitListComponent,
    AdminOrganizationUnitGroupsComponent,
  ],
  imports: [
    CommonModule,
    OrganizationRoutingModule,
    CoreModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class OrganizationModule {}
