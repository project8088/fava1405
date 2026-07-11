import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SabteAhvalRoutingModule } from './sabte-ahval-routing-module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '@core/core.module';
import { MaterialModule } from '@core/material/material.module';
import { AdminCheckStateLifeListComponent } from './chek-state-life-list/check-state-life-list.component';
import { AdminAddSabtAhvalDialogComponent } from './dialog/add-sabtAhval/add-sabtAhval.component';
import { AdminSabtAhvalCitizensListComponent } from './get-sabtAhval-citizen-list/get-sabtAhval-citizen-list.component';
import { AdminSabtAhvalListComponent } from './sabtAhval-list/sabtAhval-list.component';

@NgModule({
  declarations: [
    AdminAddSabtAhvalDialogComponent,
    AdminSabtAhvalListComponent,
    AdminSabtAhvalCitizensListComponent,
    AdminCheckStateLifeListComponent,
  ],
  imports: [
    CommonModule,
    SabteAhvalRoutingModule,
    CoreModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class SabteAhvalModule {}
