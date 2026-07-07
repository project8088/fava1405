import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminAddFeedBackDialogComponent } from './_dialogs/add-feedback/add-feedback.component';
import { AdminAddOrUpdateAppserviceComponent } from './citizenApps/add-or-update-appservice/add-or-update-appservice.component';
import { AdminAddOrUpdateFaqComponent } from './content/add-or-update-faq/add-or-update-faq.component';
import { AdminAddOrUpdateGroupsComponent } from './groups/add-or-update-groups/add-or-update-groups.component';
import { AdminAddOrUpdateManagerComponent } from './users/add-or-update-manager/add-or-update-manager.component';
import { AdminAddOrUpdateMenuDialogComponent } from './content/menu-management/dialog/add-update-menu/add-update-menu.component';
import { AdminAddOrUpdateNewsComponent } from './content/add-or-update-news/add-or-update-news.component';
import { AdminAddOrUpdateNotificationComponent } from './content/add-or-update-notification/add-or-update-notification.component';
import { AdminAddOrUpdatePageComponent } from './content/add-or-update-page/add-or-update-page.component';
import { AdminAddOrUpdateSlideShowDialogComponent } from './content/slide-show-list/dialog/add-update-slide/add-update-slide.component';
import { AdminAddSabtAhvalDialogComponent } from './sabtAhval/dialog/add-sabtAhval/add-sabtAhval.component';
import { AdminAddUserDialogComponent } from './users/dialogs/add-user/add-user.component';
import { AdminAddUserGrousDialogComponent } from './users/dialogs/add-usergroups/add-usergroups.component';
import { AdminAppserviceListComponent } from './citizenapps/appservice-list/appservice-list.component';
import { AdminChangePasswordDialogComponent } from './users/dialogs/change-user-password/change-user-password.component';
import { AdminCitizenAdvancedSearchComponent } from './citizens/citizen-advanced-search/citizen-advanced-search.component';
import { AdminCitizenEditImageDialogComponent } from './_dialogs/citizen-edit-image/citizen-edit-image.component';
import { AdminCitizenFamilyDetailsComponent } from './citizens/citizen-family-details/citizen-family-details.component';
import { AdminCitizenImageDialogComponent } from './_dialogs/citizen-image/citizen-image.component';
import { AdminCitizenManzelatReviewComponent } from './_dialogs/citizen-manzelat-review/citizen-manzelat-review.component';
import { AdminCitizenRejectFamilyComponent } from './_dialogs/citizen-reject-family/citizen-reject-family.component';
import { AdminCitizenRejectImageDialogComponent } from './_dialogs/citizen-reject-image/citizen-reject-image.component';
import { AdminCitizenSmsListDialogComponent } from './_dialogs/citizen-sms-list/citizen-sms-list.component';
import { AdminCitizenTransactionListComponent } from './citizens/transaction-list/transaction-list.component';
import { AdminCitizensComponent } from './citizens/citizens.component';
import { AdminCitizensFamilyComponent } from './citizens/citizen-family/citizens-family.component';
import { AdminCitizensPicturesComponent } from './citizens/citizens-pictures/citizens-pictures.component';
import { AdminCompaniesListComponent } from './company-list/company-list.component';
import { AdminCompanyChangeStatusDialogComponent } from './_dialogs/company-change-status/company-change-status.component';
import { AdminCompanyContractDialogComponent } from './_dialogs/company-contract/company-contract.component';
import { AdminComponent } from './admin.component';
import { AdminContactUsListComponent } from './contact-list/contact-list.component';
import { AdminDashboardComponent } from './dashboard/dashboard.component';
import { AdminEditCitizenInfoComponent } from './citizens/edit-citizen-info/edit-citizen-info.component';
import { AdminFaqGroupsComponent } from './content/faq-groups/faq-groups.component';
import { AdminFaqListComponent } from './content/faq-list/faq-list.component';
import { AdminGroupListComponent } from './groups/groups-list/groups-list.component';
import { AdminManageGroupsCitizenComponent } from './citizens/manage-groups-citizens/manage-group-citizen.component';
import { AdminManagerUsersComponent } from './users/manager-users/manager-users.component';
import { AdminManzelatCitizensComponent } from './citizens/manzelat-citizens/manzelat-citizens.component';
import { AdminManzelatCitizensDetailsComponent } from './citizens/manzelat-citizens-details/manzelat-citizens-details.component';
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
import { AdminTreePermissionComponent } from './users/user-permissions/tree-permission/tree-permission.component';
import { AdminUnitListComponent } from './organization/unit-list/unit-list.component';
import { AdminUpdateUserDialogComponent } from './users/dialogs/update-user/update-user.component';
import { AdminUserGroupsComponent } from './users/admin-userGroups/admin-userGroups.component';
import { AdminUserPermissionsComponent } from './users/user-permissions/user-permissions.component';
import { AdminUsersComponent } from './users/admin-users/admin-users.component';
import { CommonModule } from '@angular/common';
import { CoreModule } from '@core/core.module';
import { ManageAttachmentDialogComponent } from './_dialogs/manage-attachment/manage-attachment.component';
import { MaterialModule } from '@core/material/material.module';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { SmsSettingComponent } from './sms-setting/sms-setting.component';
import { UserRoleListComponent } from './users/user-role/user-role.component';
import { AdminViewEventDetailsDialogComponent } from './_dialogs/event-details/event-details.component';
import { AdminViewEventDetailsComponent } from './_dialogs/event-details/event-details/event-details.component';
import { AdminCitizenProfileComponent } from './citizen-profile-edit/citizen-profile.component';
import { AdminCitizenBaseInfoComponent } from './citizen-profile-edit/base-info/base-info.component';
import { AdminCitizenOtherInfoComponent } from './citizen-profile-edit/other-info/other-info.component';
import { AdminCitizenAddressInfoComponent } from './citizen-profile-edit/address-info/address-info.component';
import { AdminWebApiUsersComponent } from './users/web-api-users/web-api-users.component';
import { AdminAddWebApiUserDialogComponent } from './users/dialogs/add-webapi-users/add-webapi-users.component';
import { AdminTreeWebApiPermissionComponent } from './users/web-api-user-permissions/tree-permission/tree-user-web-api-permission.component';
import { AdminWebApiUserPermissionsComponent } from './users/web-api-user-permissions/web-api-user-permissions.component';
import { AdminUserAppAccessServiceComponent } from './users/user-access-app-service/user-access-app-service.component';
import { AdminCardListComponent } from './store/card-list/card-list.component';
import { AdminAddOrUpdateCardComponent } from './store/add-or-update-card/add-or-update-card.component';
import { AdminRefundExcelBatchFileDetailsComponent } from './Refund/refund-excel-batch-file-details/refund-excel-batch-file-details.component';
import { AdminRefundExcelBatchFileListComponent } from './Refund/refund-excel-batch-file/refund-excel-batch-file.component';
import { AdminImportRefundExcelDialogComponent } from './Refund/dialog/refund-import-excel/import-refund-excel.component';
import { AdminRefundAccessListComponent } from './Refund/refund-access-list/refund-access-list.component';
import { AdminRefundAccessDetailsListComponent } from './Refund/refund-access-details-list/refund-access-details-list.component';
import { AdminChangeRefundAccessDialogComponent } from './Refund/dialog/change-refund-access/change-refund-access.component';
import { AdminChangeRefundDialogComponent } from './Refund/dialog/change-refund/change-refund.component';
import { AdminCitizenCardAdvancedSearchComponent } from './citizens-cards/citizen-card-advanced-search/citizen-card-advanced-search.component';
import { AdminBackCitizenCardDialogComponent } from './citizens-cards/dialog/back-citizen-card/back-citizen-card.component';
import { AdminDeliveredCitizenCardDialogComponent } from './citizens-cards/dialog/delivered-citizen-card/delivered-citizen-card.component';
import { AdminCancellationCitizenCardDialogComponent } from './citizens-cards/dialog/cancellation-citizen-card/cancellation-citizen-card.component';
import { AdminCitizenCardExportSearchComponent } from './citizens-cards/citizen-card-export-search/citizen-card-export-search.component';
import { AdminCitizenCardExportDetailsComponent } from './citizens-cards/citizen-card-export-details/citizen-card-export-details.component';
import { AdminImportCardNumberDialogComponent } from './citizens-cards/dialog/import-card-number/import-card-number.component';
import { AdminAllCitizensFeedBacksComponent } from './citizens/all-citizens-feed-backs/all-citizens-feed-backs.component';
import { AdminReportRefundDialogComponent } from './Refund/dialog/report-refund/report-refund.component';
import { AdminImportNationCodeGroupsExcelDialogComponent } from './import-Excel-Import/dialog/nationCode-groups-import-excel/import-nationCode-groups-excel.component';
import { AdminCitizensInGroupsComponent } from './citizens/citizens-in-group/citizens-in-group.component';
import { AdminCitizenExcelBatchFileListComponent } from './citizens/citizen-excel-batch-file/citizen-excel-batch-file.component';
import { AdminCitizenExcelBatchFileDetailsComponent } from './citizens/citizen-excel-batch-file-details/citizen-excel-batch-file-details.component';
import { AdminImportCitizenExcelDialogComponent } from './citizens/dialog/citizen-import-excel/import-citizen-excel.component';
import { AdminGroupQueueCitizensListComponent } from './groups/group-queue-citizens-list/group-queue-citizens-list.component';
import { AdminRefundAccessSearchListComponent } from './Refund/refund-access-search-list/refund-access-search-list.component';
import { ManzalatSettingComponent } from './manzalat-setting/manzalat-setting.component';
import { AdminAddRefundTransactionDialogComponent } from './Refund/dialog/add-refund-transaction/add-refund-transaction.component';
import { AdminGroupTransferDialogComponent } from './groups/_dialogs/group-transfer/group-transfer.component';
import { AdminAddOrUpdateCitizensNotificationComponent } from './content/citizens-notification/citizens-notification.component';
import { AdminAllUsersComponent } from './users/all-users/all-users.component';
import { AdminUserAccessGroupPermissionsComponent } from './users/user-access-group-permissions/user-access-group-permissions.component';
import { AdminOrganizationUnitGroupsComponent } from './organization/organization-unit-groups/organization-unit-groups.component';
import { AdminUserAccessIpComponent } from './users/user-access-ip/user-access-ip.component';
import { AdminCitizenAuthenticationComponent } from './citizens-authentication/citizens-authentication.component';
import { AdminUpdateCitizenMobileNumberDialogComponent } from './citizens/dialog/update-citizen-mobile-number/update-citizen-mobile-number.component';
import { AdminDashboardCitizenRegisterReportChartComponent } from './dashboard/citizen-register-report-chart/citizen-register-report-chart.component';
import { AdminCheckStateLifeListComponent } from './sabtAhval/chek-state-life-list/check-state-life-list.component';
import { AdminCitizenIdentityInfoComponent } from './citizen-profile-edit/citizen-Identity-Info/citizen-Identity-Info.component';
import { AdminUpdateCitizenIdentityInfoComponent } from './citizen-profile-edit/update-citizen-Identity-Info/update-citizen-Identity-Info.component';
import { AdminUpdateCitizenSabtStateDialogComponent } from './citizens/dialog/update-citizen-sabt-state/update-citizen-sabt-state.component';
import { AdminUpdateManzalatBaseFormComponent } from './manzalat-base-form/update-manzalat-base-form/update-manzalat-base-form.component';
import { AdminManzalatBaseFromListComponent } from './manzalat-base-form/manzalat-base-list/manzalat-base-list.component';
import { AdminRefundUsersComponent } from './Refund/refund-users/refund-users.component';
import { AdminAddRefundUserDialogComponent } from './Refund/dialog/add-refund-user/add-refund-user.component';
import { AdminCitizenAuthenticationSearchComponent } from './citizens/citizen-authentication-search/citizen-authentication-search.component';
import { AdminConfigComponent } from './config/config.component';
import { HtmlEditorModule } from '@core/public-component/html-editor/html-editor.module';

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
    AdminUsersComponent,
    AdminAddUserDialogComponent,
    AdminChangePasswordDialogComponent,
    AdminUpdateUserDialogComponent,
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
    AdminManagerUsersComponent,
    AdminAddOrUpdateManagerComponent,
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

    AdminCitizensComponent,
    AdminSlideShowListComponent,
    AdminAddOrUpdateSlideShowDialogComponent,
    AdminAddOrUpdateAppserviceComponent,
    AdminAppserviceListComponent,
    AdminUserGroupsComponent,
    AdminAddUserGrousDialogComponent,
    AdminUserPermissionsComponent,
    AdminTreePermissionComponent,
    AdminTreeWebApiPermissionComponent,
    AdminWebApiUserPermissionsComponent,
    AdminAddFeedBackDialogComponent,
    AdminCitizensPicturesComponent,
    AdminCitizenTransactionListComponent,
    AdminCitizenImageDialogComponent,
    AdminCitizenSmsListDialogComponent,
    AdminCitizenRejectImageDialogComponent,
    AdminCitizenEditImageDialogComponent,
    AdminCitizensFamilyComponent,
    AdminCitizenFamilyDetailsComponent,
    AdminCitizenRejectFamilyComponent,
    AdminManzelatCitizensComponent,
    AdminManzelatCitizensDetailsComponent,
    AdminCitizenManzelatReviewComponent,
    AdminAddSabtAhvalDialogComponent,
    AdminSabtAhvalListComponent,
    AdminSabtAhvalCitizensListComponent,
    AdminCheckStateLifeListComponent,
    UserRoleListComponent,
    AdminGroupListComponent,
    AdminAddOrUpdateGroupsComponent,
    AdminManageGroupsCitizenComponent,
    AdminCitizenAdvancedSearchComponent,
    AdminEditCitizenInfoComponent,
    AdminViewEventDetailsComponent,
    AdminViewEventDetailsDialogComponent,
    AdminCitizenProfileComponent,
    AdminCitizenBaseInfoComponent,
    AdminCitizenOtherInfoComponent,
    AdminCitizenAddressInfoComponent,
    AdminWebApiUsersComponent,
    AdminAddWebApiUserDialogComponent,
    AdminUserAppAccessServiceComponent,
    AdminRefundExcelBatchFileDetailsComponent,
    AdminRefundExcelBatchFileListComponent,
    AdminImportRefundExcelDialogComponent,
    AdminRefundAccessListComponent,
    AdminRefundAccessDetailsListComponent,
    AdminChangeRefundAccessDialogComponent,
    AdminChangeRefundDialogComponent,
    AdminCitizenCardAdvancedSearchComponent,
    AdminBackCitizenCardDialogComponent,
    AdminDeliveredCitizenCardDialogComponent,
    AdminCancellationCitizenCardDialogComponent,
    AdminCitizenCardExportSearchComponent,
    AdminCitizenCardExportDetailsComponent,
    AdminImportCardNumberDialogComponent,
    AdminAllCitizensFeedBacksComponent,
    AdminReportRefundDialogComponent,
    AdminImportNationCodeGroupsExcelDialogComponent,
    AdminCitizensInGroupsComponent,
    AdminCitizenExcelBatchFileListComponent,
    AdminCitizenExcelBatchFileDetailsComponent,
    AdminImportCitizenExcelDialogComponent,
    AdminGroupQueueCitizensListComponent,
    AdminRefundAccessSearchListComponent,
    ManzalatSettingComponent,
    AdminAddRefundTransactionDialogComponent,
    AdminGroupTransferDialogComponent,
    AdminAddOrUpdateCitizensNotificationComponent,
    AdminAllUsersComponent,
    AdminUserAccessGroupPermissionsComponent,
    AdminUserAccessIpComponent,
    AdminOrganizationUnitGroupsComponent,
    AdminCitizenAuthenticationComponent,
    AdminUpdateCitizenMobileNumberDialogComponent,
    AdminDashboardCitizenRegisterReportChartComponent,
    AdminCitizenIdentityInfoComponent,
    AdminUpdateCitizenIdentityInfoComponent,
    AdminUpdateCitizenSabtStateDialogComponent,
    AdminUpdateManzalatBaseFormComponent,
    AdminManzalatBaseFromListComponent,
    AdminRefundUsersComponent,
    AdminAddRefundUserDialogComponent,
    AdminCitizenAuthenticationSearchComponent,
    AdminConfigComponent,
  ],

  imports: [
    CoreModule,
    CommonModule,
    AdminRoutingModule,
    FormsModule,
    SharedModule,
    ReactiveFormsModule,
    MaterialModule,
    RouterModule,
    HtmlEditorModule,
  ],
})
export class AdminModule {}
