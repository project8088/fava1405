import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing-module';
import { AdminDashboardComponent } from './dashboard/dashboard.component';
import { AdminDashboardCitizenRegisterReportChartComponent } from './dashboard/citizen-register-report-chart/citizen-register-report-chart.component';
import { CoreModule } from '@core/core.module';
import { MaterialModule } from '@core/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminViewEventDetailsComponent } from './_dialogs/event-details/event-details/event-details.component';
import { AdminViewEventDetailsDialogComponent } from './_dialogs/event-details/event-details.component';

@NgModule({
  declarations: [
    AdminDashboardComponent,
    AdminDashboardCitizenRegisterReportChartComponent,
    AdminViewEventDetailsComponent,
    AdminViewEventDetailsDialogComponent,
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    CoreModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class AdminModule {}
