import { AdminAddFeedBackDialogComponent } from './_dialogs/add-feedback/add-feedback.component';
import { AdminAddOrUpdateGroupsComponent } from './groups/add-or-update-groups/add-or-update-groups.component';
import { AdminAddSabtAhvalDialogComponent } from './sabtAhval/dialog/add-sabtAhval/add-sabtAhval.component';
import { AdminComponent } from './admin.component';
import { AdminContactUsListComponent } from './contact-list/contact-list.component';
import { AdminDashboardComponent } from './dashboard/dashboard.component';
import { AdminGroupListComponent } from './groups/groups-list/groups-list.component';
import { AdminOrganizationListComponent } from './organization/organization-list/organization-list.component';
import { AdminPaySettingComponent } from './pay-setting/pay-setting.component';
import { AdminPayTestComponent } from './pay-test/pay-test.component';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminSabtAhvalCitizensListComponent } from './sabtAhval/get-sabtAhval-citizen-list/get-sabtAhval-citizen-list.component';
import { AdminSabtAhvalListComponent } from './sabtAhval/sabtAhval-list/sabtAhval-list.component';
import { AdminSettingComponent } from './setting/setting.component';
import { AdminSmsListComponent } from './sms-list/sms-list.component';
import { AdminTicketSubjectsComponent } from './base-data/ticket-subjects/ticket-subjects.component';
import { AdminUnitListComponent } from './organization/unit-list/unit-list.component';


import { SmsSettingComponent } from './sms-setting/sms-setting.component';
import { AdminViewEventDetailsDialogComponent } from './_dialogs/event-details/event-details.component';
import { AdminViewEventDetailsComponent } from './_dialogs/event-details/event-details/event-details.component';
import { AdminCardListComponent } from './store/card-list/card-list.component';
import { AdminAddOrUpdateCardComponent } from './store/add-or-update-card/add-or-update-card.component';
import { AdminImportNationCodeGroupsExcelDialogComponent } from './import-Excel-Import/dialog/nationCode-groups-import-excel/import-nationCode-groups-excel.component';
import { ManzalatSettingComponent } from './manzalat-setting/manzalat-setting.component';
import { AdminGroupTransferDialogComponent } from './groups/_dialogs/group-transfer/group-transfer.component';
import { AdminOrganizationUnitGroupsComponent } from './organization/organization-unit-groups/organization-unit-groups.component';
import { AdminDashboardCitizenRegisterReportChartComponent } from './dashboard/citizen-register-report-chart/citizen-register-report-chart.component';
import { AdminCheckStateLifeListComponent } from './sabtAhval/chek-state-life-list/check-state-life-list.component';
import { AdminUpdateManzalatBaseFormComponent } from './manzalat-base-form/update-manzalat-base-form/update-manzalat-base-form.component';
import { AdminManzalatBaseFromListComponent } from './manzalat-base-form/manzalat-base-list/manzalat-base-list.component';
import { AdminConfigComponent } from './config/config.component';

@NgModule({
  declarations: [
    AdminComponent,
    AdminDashboardComponent,
    AdminOrganizationListComponent,
    AdminUnitListComponent,
    AdminSettingComponent,
    SmsSettingComponent,
    AdminContactUsListComponent,
    AdminTicketSubjectsComponent,
    AdminCardListComponent,
    AdminAddOrUpdateCardComponent,
    AdminSmsListComponent,
    AdminPayTestComponent,
    AdminPaySettingComponent,

    AdminAddFeedBackDialogComponent,
    AdminAddSabtAhvalDialogComponent,
    AdminSabtAhvalListComponent,
    AdminSabtAhvalCitizensListComponent,
    AdminCheckStateLifeListComponent,
    AdminGroupListComponent,
    AdminAddOrUpdateGroupsComponent,
    AdminViewEventDetailsComponent,
    AdminViewEventDetailsDialogComponent,
 
    AdminImportNationCodeGroupsExcelDialogComponent,
    ManzalatSettingComponent,
    AdminGroupTransferDialogComponent,
    AdminOrganizationUnitGroupsComponent,
    AdminDashboardCitizenRegisterReportChartComponent,
    AdminUpdateManzalatBaseFormComponent,
    AdminManzalatBaseFromListComponent,
    AdminConfigComponent,
  ],

  imports: [
    AdminRoutingModule,
  ],
})
export class AdminModule {}
