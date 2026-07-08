import { RouterModule, Routes } from '@angular/router';

import { AdminAddOrUpdateAppserviceComponent } from './citizen-apps/add-or-update-appservice/add-or-update-appservice.component';
import { AdminAddOrUpdateFaqComponent } from './content/add-or-update-faq/add-or-update-faq.component';
import { AdminAddOrUpdateGroupsComponent } from './groups/add-or-update-groups/add-or-update-groups.component';
import { AdminAddOrUpdateManagerComponent } from './users/add-or-update-manager/add-or-update-manager.component';
import { AdminAddOrUpdateNewsComponent } from './content/add-or-update-news/add-or-update-news.component';
import { AdminAddOrUpdateNotificationComponent } from './content/add-or-update-notification/add-or-update-notification.component';
import { AdminAddOrUpdatePageComponent } from './content/add-or-update-page/add-or-update-page.component';
import { AdminCitizenAdvancedSearchComponent } from './citizens/citizen-advanced-search/citizen-advanced-search.component';
import { AdminCitizenFamilyDetailsComponent } from './citizens/citizen-family-details/citizen-family-details.component';
import { AdminCitizensComponent } from './citizens/citizens.component';
import { AdminCitizensFamilyComponent } from './citizens/citizen-family/citizens-family.component';
import { AdminCitizensPicturesComponent } from './citizens/citizens-pictures/citizens-pictures.component';
import { AdminCompaniesListComponent } from './company-list/company-list.component';
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
import { AdminSabtAhvalCitizensListComponent } from './sabtAhval/get-sabtAhval-citizen-list/get-sabtAhval-citizen-list.component';
import { AdminSabtAhvalListComponent } from './sabtAhval/sabtAhval-list/sabtAhval-list.component';
import { AdminSettingComponent } from './setting/setting.component';
import { AdminSlideShowListComponent } from './content/slide-show-list/slide-show-list.component';
import { AdminSmsListComponent } from './sms-list/sms-list.component';
import { AdminTicketSubjectsComponent } from './base-data/ticket-subjects/ticket-subjects.component';
import { AdminTransactionListComponent } from './financial/transaction-list/transaction-list.component';
import { AdminUnitListComponent } from './organization/unit-list/unit-list.component';
import { AdminUserGroupsComponent } from './users/admin-userGroups/admin-userGroups.component';
import { AdminUserPermissionsComponent } from './users/user-permissions/user-permissions.component';
import { AdminUsersComponent } from './users/admin-users/admin-users.component';
import { ChangeCurrentUserPasswordComponent } from '@app/shared/change-current-user-password/change-current-user-password.component';
import { CompanyInfoComponent } from '@app/shared/company-info/company-info.component';
import { NgModule } from '@angular/core';
import { SmsSettingComponent } from './sms-setting/sms-setting.component';
import { TicketDetailsComponent } from '@app/shared/ticket/view-ticket/ticket-details.component';
import { TicketListComponent } from '@app/shared/ticket/ticket-list/ticket-list.component';
import { TransactionDetailsComponent } from '@app/shared/transaction-details/transaction-details.component';
import { UserRoleListComponent } from './users/user-role/user-role.component';
import { AdminCitizenProfileComponent } from './citizen-profile-edit/citizen-profile.component';
import { AdminWebApiUsersComponent } from './users/web-api-users/web-api-users.component';
import { AdminWebApiUserPermissionsComponent } from './users/web-api-user-permissions/web-api-user-permissions.component';
import { AdminUserAppAccessServiceComponent } from './users/user-access-app-service/user-access-app-service.component';
import { AdminCardListComponent } from './store/card-list/card-list.component';
import { AdminAddOrUpdateCardComponent } from './store/add-or-update-card/add-or-update-card.component';
import { AdminRefundExcelBatchFileDetailsComponent } from './Refund/refund-excel-batch-file-details/refund-excel-batch-file-details.component';
import { AdminRefundExcelBatchFileListComponent } from './Refund/refund-excel-batch-file/refund-excel-batch-file.component';
import { AdminRefundAccessListComponent } from './Refund/refund-access-list/refund-access-list.component';
import { AdminRefundAccessDetailsListComponent } from './Refund/refund-access-details-list/refund-access-details-list.component';
import { AdminCitizenCardAdvancedSearchComponent } from './citizens-cards/citizen-card-advanced-search/citizen-card-advanced-search.component';
import { AdminCitizenCardExportSearchComponent } from './citizens-cards/citizen-card-export-search/citizen-card-export-search.component';
import { AdminCitizenCardExportDetailsComponent } from './citizens-cards/citizen-card-export-details/citizen-card-export-details.component';
import { AdminAllCitizensFeedBacksComponent } from './citizens/all-citizens-feed-backs/all-citizens-feed-backs.component';
import { AdminCitizensInGroupsComponent } from './citizens/citizens-in-group/citizens-in-group.component';
import { AdminCitizenExcelBatchFileDetailsComponent } from './citizens/citizen-excel-batch-file-details/citizen-excel-batch-file-details.component';
import { AdminCitizenExcelBatchFileListComponent } from './citizens/citizen-excel-batch-file/citizen-excel-batch-file.component';
import { AdminGroupQueueCitizensListComponent } from './groups/group-queue-citizens-list/group-queue-citizens-list.component';
import { AdminRefundAccessSearchListComponent } from './Refund/refund-access-search-list/refund-access-search-list.component';
import { ManzalatSettingComponent } from './manzalat-setting/manzalat-setting.component';
import { AdminAddOrUpdateCitizensNotificationComponent } from './content/citizens-notification/citizens-notification.component';
import { AdminAllUsersComponent } from './users/all-users/all-users.component';
import { AdminUserAccessGroupPermissionsComponent } from './users/user-access-group-permissions/user-access-group-permissions.component';
import { AdminOrganizationUnitGroupsComponent } from './organization/organization-unit-groups/organization-unit-groups.component';
import { AdminUserAccessIpComponent } from './users/user-access-ip/user-access-ip.component';
import { AdminCitizenAuthenticationComponent } from './citizens-authentication/citizens-authentication.component';
import { AdminCheckStateLifeListComponent } from './sabtAhval/chek-state-life-list/check-state-life-list.component';
import { AppShowCitizenComponent } from '@app/shared/citizens/show-citizens/show-citizen.component';
import { AdminManzalatBaseFromListComponent } from './manzalat-base-form/manzalat-base-list/manzalat-base-list.component';
import { AdminUpdateManzalatBaseFormComponent } from './manzalat-base-form/update-manzalat-base-form/update-manzalat-base-form.component';
import { AdminRefundUsersComponent } from './Refund/refund-users/refund-users.component';
import { AdminCitizenAuthenticationSearchComponent } from './citizens/citizen-authentication-search/citizen-authentication-search.component';
import { AdminConfigComponent } from './config/config.component';
import { AdminAppserviceListComponent } from './citizen-apps/appservice-list/appservice-list.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: AdminDashboardComponent },
      { path: 'register-company', component: AdminRegisterCompanyComponent },
      { path: 'companies', component: AdminCompaniesListComponent },

      { path: 'news-list', component: AdminNewsListComponent },
      { path: 'addupdate-news/:id', component: AdminAddOrUpdateNewsComponent },
      { path: 'news-comments/:id', component: AdminNewsCommentsComponent },
      { path: 'news-groups', component: AdminNewsGroupsComponent },
      { path: 'pages', component: AdminPageListComponent },
      { path: 'addupdate-page/:id', component: AdminAddOrUpdatePageComponent },
      { path: 'slide-show', component: AdminSlideShowListComponent },

      { path: 'notifications', component: AdminNotificationListComponent },
      { path: 'addupdate-notification/:id', component: AdminAddOrUpdateNotificationComponent },
      {
        path: 'citizen-notifications/:id',
        component: AdminAddOrUpdateCitizensNotificationComponent,
      },

      { path: 'ticket-subjects', component: AdminTicketSubjectsComponent },

      { path: 'tickets', component: TicketListComponent },
      { path: 'ticket-details/:id', component: TicketDetailsComponent },

      { path: 'manzalat-form-list', component: AdminManzalatBaseFromListComponent },
      { path: 'update-manzalat-form/:id', component: AdminUpdateManzalatBaseFormComponent },

      { path: 'faq-groups', component: AdminFaqGroupsComponent },
      { path: 'faq-list', component: AdminFaqListComponent },
      { path: 'add-update-faq/:id', component: AdminAddOrUpdateFaqComponent },

      { path: 'citizen-in-group/:id', component: AdminCitizensInGroupsComponent },
      { path: 'show-citizen/:id', component: AppShowCitizenComponent },

      //organization
      { path: 'organization', component: AdminOrganizationListComponent },
      { path: 'units/:id', component: AdminUnitListComponent },
      { path: 'organization-unit-groups/:id', component: AdminOrganizationUnitGroupsComponent },

      { path: 'company-info/:id', component: CompanyInfoComponent },
      //Group
      { path: 'group-list', component: AdminGroupListComponent },
      { path: 'add-update-group/:id', component: AdminAddOrUpdateGroupsComponent },
      { path: 'manage-citizen-groups/:id', component: AdminManageGroupsCitizenComponent },

      { path: 'group-queue-citizens-list/:id', component: AdminGroupQueueCitizensListComponent },

      { path: 'contact-us', component: AdminContactUsListComponent },
      { path: 'setting', component: AdminSettingComponent },
      { path: 'pay-setting', component: AdminPaySettingComponent },
      { path: 'sms-setting', component: SmsSettingComponent },

      { path: 'manzelat-settings', component: ManzalatSettingComponent },

      { path: 'config', component: AdminConfigComponent },

      { path: 'card-list', component: AdminCardListComponent },
      { path: 'addupdate-card/:cardTypeId/:id', component: AdminAddOrUpdateCardComponent },

      //SabtAhval
      { path: 'sabtAhval-list', component: AdminSabtAhvalListComponent },
      { path: 'sabtAhval-citizens/:id', component: AdminSabtAhvalCitizensListComponent },
      { path: 'check-state-life-list/:id', component: AdminCheckStateLifeListComponent },

      { path: 'menu-management', component: AdminMenuManagementComponent },

      //users
      { path: 'admin-users', component: AdminUsersComponent },
      { path: 'all-users', component: AdminAllUsersComponent },
      { path: 'web-api-users', component: AdminWebApiUsersComponent },
      { path: 'admin-userGroups', component: AdminUserGroupsComponent },
      { path: 'manager-users', component: AdminManagerUsersComponent },
      { path: 'addupdate-manager/:id', component: AdminAddOrUpdateManagerComponent },
      { path: 'change-password', component: ChangeCurrentUserPasswordComponent },
      { path: 'permissions/:id', component: AdminUserPermissionsComponent },
      { path: 'web-api-permissions/:id', component: AdminWebApiUserPermissionsComponent },
      { path: 'user-roles/:id', component: UserRoleListComponent },
      { path: 'user-access-service/:id', component: AdminUserAppAccessServiceComponent },

      {
        path: 'user-group-access-permissions/:id',
        component: AdminUserAccessGroupPermissionsComponent,
      },
      { path: 'user-access-ip/:id', component: AdminUserAccessIpComponent },

      //financial

      { path: 'transaction-list', component: AdminTransactionListComponent },
      { path: 'transaction-details/:id', component: TransactionDetailsComponent },

      { path: 'sms-list', component: AdminSmsListComponent },

      { path: 'pay-test', component: AdminPayTestComponent },

      { path: 'citizen-register-file-list', component: AdminCitizenExcelBatchFileListComponent },
      {
        path: 'citizen-register-file-details/:importId',
        component: AdminCitizenExcelBatchFileDetailsComponent,
      },

      //refund
      { path: 'refund-file-list', component: AdminRefundExcelBatchFileListComponent },
      {
        path: 'refund-file-details/:importId',
        component: AdminRefundExcelBatchFileDetailsComponent,
      },
      { path: 'refund-access-list', component: AdminRefundAccessListComponent },
      { path: 'refund-access-details/:importId', component: AdminRefundAccessDetailsListComponent },
      { path: 'search-refund', component: AdminRefundAccessSearchListComponent },

      { path: 'refund-users', component: AdminRefundUsersComponent },

      //citizens
      { path: 'search-citizen', component: AdminCitizensComponent },
      { path: 'advanced-search-citizen', component: AdminCitizenAdvancedSearchComponent },
      { path: 'search-citizen-family', component: AdminCitizensFamilyComponent },
      { path: 'citizen-family-details/:id', component: AdminCitizenFamilyDetailsComponent },
      { path: 'appService-list', component: AdminAppserviceListComponent },
      { path: 'addupdate-appService/:id', component: AdminAddOrUpdateAppserviceComponent },
      { path: 'citizens-pictures', component: AdminCitizensPicturesComponent },
      { path: 'search-manzelat-citizens', component: AdminManzelatCitizensComponent },
      { path: 'citizen-manzelat-details/:id', component: AdminManzelatCitizensDetailsComponent },
      { path: 'edit-citizen-info/:id', component: AdminEditCitizenInfoComponent },
      { path: 'edit-profile/:id', component: AdminCitizenProfileComponent },
      { path: 'all-citizens-feedBacks', component: AdminAllCitizensFeedBacksComponent },
      { path: 'citizens-authentication', component: AdminCitizenAuthenticationComponent },
      {
        path: 'citizens-authentication-search',
        component: AdminCitizenAuthenticationSearchComponent,
      },

      //citizen Card
      { path: 'advanced-search-card-citizen', component: AdminCitizenCardAdvancedSearchComponent },
      { path: 'export-search-card-citizen', component: AdminCitizenCardExportSearchComponent },
      {
        path: 'export-details-citizen-card/:id',
        component: AdminCitizenCardExportDetailsComponent,
      },
    ],
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}

