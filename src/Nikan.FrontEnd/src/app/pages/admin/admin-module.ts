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
import { SharedModule } from '@app/shared/shared.module';
import { AdminConfigComponent } from './config/config.component';
import { AdminContactUsListComponent } from './contact-list/contact-list.component';
import { AdminSettingComponent } from './setting/setting.component';
import { AdminSmsListComponent } from './sms-list/sms-list.component';
import { SmsSettingComponent } from './sms-setting/sms-setting.component';
import { AdminAddFeedBackDialogComponent } from './_dialogs/add-feedback/add-feedback.component';
import { AdminAddOrUpdateCitizensNotificationComponent } from './citizens-notification/citizens-notification.component';
import { ChartModule } from 'angular-highcharts';

@NgModule({
  declarations: [
    AdminDashboardComponent,
    AdminDashboardCitizenRegisterReportChartComponent,
    AdminViewEventDetailsComponent,
    AdminViewEventDetailsDialogComponent,
    AdminAddFeedBackDialogComponent,
    AdminSettingComponent,
    SmsSettingComponent,
    AdminContactUsListComponent,
    AdminSmsListComponent,
    AdminConfigComponent,
    AdminAddOrUpdateCitizensNotificationComponent,
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    CoreModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    ChartModule,
  ],
})
export class AdminModule {}
