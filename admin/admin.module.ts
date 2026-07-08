import { AdminAddFeedBackDialogComponent } from './_dialogs/add-feedback/add-feedback.component';
import { AdminAddOrUpdateFaqComponent } from './content/add-or-update-faq/add-or-update-faq.component';
import { AdminAddOrUpdateGroupsComponent } from './groups/add-or-update-groups/add-or-update-groups.component';
import { AdminAddOrUpdateMenuDialogComponent } from './content/menu-management/dialog/add-update-menu/add-update-menu.component';
import { AdminAddOrUpdateNewsComponent } from './content/add-or-update-news/add-or-update-news.component';
import { AdminAddOrUpdateNotificationComponent } from './content/add-or-update-notification/add-or-update-notification.component';
import { AdminAddOrUpdatePageComponent } from './content/add-or-update-page/add-or-update-page.component';
import { AdminAddOrUpdateSlideShowDialogComponent } from './content/slide-show-list/dialog/add-update-slide/add-update-slide.component';
import { AdminAddSabtAhvalDialogComponent } from './sabtAhval/dialog/add-sabtAhval/add-sabtAhval.component';
import { AdminCompaniesListComponent } from './company-list/company-list.component';
import { AdminCompanyChangeStatusDialogComponent } from './_dialogs/company-change-status/company-change-status.component';
import { AdminCompanyContractDialogComponent } from './_dialogs/company-contract/company-contract.component';
import { AdminComponent } from './admin.component';
import { AdminContactUsListComponent } from './contact-list/contact-list.component';
import { AdminDashboardComponent } from './dashboard/dashboard.component';
import { AdminFaqGroupsComponent } from './content/faq-groups/faq-groups.component';
import { AdminFaqListComponent } from './content/faq-list/faq-list.component';
import { AdminGroupListComponent } from './groups/groups-list/groups-list.component';
import { AdminMenuManagementComponent } from './content/menu-management/menu-management.component';
import { AdminNewsCommentsComponent } from './content/news-comments/news-comments.component';
import { AdminNewsGroupsComponent } from './content/news-groups/news-groups.component';
import { AdminNewsListComponent } from './content/news-list/news-list.component';
import { AdminNotificationListComponent } from './content/notification-list/notification-list.component';
import { AdminOrganizationListComponent } from './organization/organization-list/organization-list.component';
import { AdminPageListComponent } from './content/page-list/page-list.component';
import { AdminPaySettingComponent } from './pay-setting/pay-setting.component';
import { AdminPayTestComponent } from './pay-test/pay-test.component';
import { AdminRegisterCompanyComponent } from './company-list/register-company/register-company.component';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminSabtAhvalCitizensListComponent } from './sabtAhval/get-sabtAhval-citizen-list/get-sabtAhval-citizen-list.component';
import { AdminSabtAhvalListComponent } from './sabtAhval/sabtAhval-list/sabtAhval-list.component';
import { AdminSettingComponent } from './setting/setting.component';
import { AdminSlideShowListComponent } from './content/slide-show-list/slide-show-list.component';
import { AdminSmsListComponent } from './sms-list/sms-list.component';
import { AdminTicketSubjectsComponent } from './base-data/ticket-subjects/ticket-subjects.component';
import { AdminTransactionListComponent } from './financial/transaction-list/transaction-list.component';
import { AdminTreeMenuComponent } from './content/menu-management/tree-menu/tree-menu.component';
import { AdminUnitListComponent } from './organization/unit-list/unit-list.component';

import { ManageAttachmentDialogComponent } from './_dialogs/manage-attachment/manage-attachment.component';

import { SmsSettingComponent } from './sms-setting/sms-setting.component';
import { AdminViewEventDetailsDialogComponent } from './_dialogs/event-details/event-details.component';
import { AdminViewEventDetailsComponent } from './_dialogs/event-details/event-details/event-details.component';
import { AdminCardListComponent } from './store/card-list/card-list.component';
import { AdminAddOrUpdateCardComponent } from './store/add-or-update-card/add-or-update-card.component';
import { AdminImportNationCodeGroupsExcelDialogComponent } from './import-Excel-Import/dialog/nationCode-groups-import-excel/import-nationCode-groups-excel.component';
import { AdminGroupQueueCitizensListComponent } from './groups/group-queue-citizens-list/group-queue-citizens-list.component';
import { ManzalatSettingComponent } from './manzalat-setting/manzalat-setting.component';
import { AdminGroupTransferDialogComponent } from './groups/_dialogs/group-transfer/group-transfer.component';
import { AdminAddOrUpdateCitizensNotificationComponent } from './content/citizens-notification/citizens-notification.component';
import { AdminOrganizationUnitGroupsComponent } from './organization/organization-unit-groups/organization-unit-groups.component';
import { AdminDashboardCitizenRegisterReportChartComponent } from './dashboard/citizen-register-report-chart/citizen-register-report-chart.component';
import { AdminCheckStateLifeListComponent } from './sabtAhval/chek-state-life-list/check-state-life-list.component';
import { AdminUpdateManzalatBaseFormComponent } from './manzalat-base-form/update-manzalat-base-form/update-manzalat-base-form.component';
import { AdminManzalatBaseFromListComponent } from './manzalat-base-form/manzalat-base-list/manzalat-base-list.component';
import { AdminConfigComponent } from './config/config.component';

@NgModule({
  declarations: [
    AdminComponent,
    AdminCompaniesListComponent,
    AdminNewsListComponent,
    AdminAddOrUpdateNewsComponent,
    AdminNewsGroupsComponent,
    AdminDashboardComponent,
    AdminFaqGroupsComponent,
    AdminOrganizationListComponent,
    AdminUnitListComponent,
    AdminNewsCommentsComponent,
    AdminFaqListComponent,
    AdminAddOrUpdateFaqComponent,
    AdminSettingComponent,
    SmsSettingComponent,
    AdminContactUsListComponent,
    AdminPageListComponent,
    AdminAddOrUpdatePageComponent,
    AdminTicketSubjectsComponent,
    AdminRegisterCompanyComponent,
    AdminNotificationListComponent,
    AdminAddOrUpdateNotificationComponent,
    ManageAttachmentDialogComponent,
    AdminMenuManagementComponent,
    AdminAddOrUpdateMenuDialogComponent,
    AdminCardListComponent,
    AdminAddOrUpdateCardComponent,
    AdminTreeMenuComponent,
    AdminCompanyContractDialogComponent,
    AdminSmsListComponent,
    AdminCompanyChangeStatusDialogComponent,
    AdminPayTestComponent,
    AdminTransactionListComponent,
    AdminPaySettingComponent,

    AdminSlideShowListComponent,
    AdminAddOrUpdateSlideShowDialogComponent,
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
    AdminGroupQueueCitizensListComponent,
    ManzalatSettingComponent,
    AdminGroupTransferDialogComponent,
    AdminAddOrUpdateCitizensNotificationComponent,
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
