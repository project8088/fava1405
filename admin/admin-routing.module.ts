
import { AdminAddOrUpdateFaqComponent } from './content/add-or-update-faq/add-or-update-faq.component';
import { AdminAddOrUpdateGroupsComponent } from './groups/add-or-update-groups/add-or-update-groups.component';
import { AdminAddOrUpdateNewsComponent } from './content/add-or-update-news/add-or-update-news.component';
import { AdminAddOrUpdateNotificationComponent } from './content/add-or-update-notification/add-or-update-notification.component';
import { AdminAddOrUpdatePageComponent } from './content/add-or-update-page/add-or-update-page.component';
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
import { AdminSabtAhvalCitizensListComponent } from './sabtAhval/get-sabtAhval-citizen-list/get-sabtAhval-citizen-list.component';
import { AdminSabtAhvalListComponent } from './sabtAhval/sabtAhval-list/sabtAhval-list.component';
import { AdminSettingComponent } from './setting/setting.component';
import { AdminSlideShowListComponent } from './content/slide-show-list/slide-show-list.component';
import { AdminSmsListComponent } from './sms-list/sms-list.component';
import { AdminTicketSubjectsComponent } from './base-data/ticket-subjects/ticket-subjects.component';
import { AdminTransactionListComponent } from './financial/transaction-list/transaction-list.component';
import { AdminUnitListComponent } from './organization/unit-list/unit-list.component';

import { SmsSettingComponent } from './sms-setting/sms-setting.component';
import { AdminCardListComponent } from './store/card-list/card-list.component';
import { AdminAddOrUpdateCardComponent } from './store/add-or-update-card/add-or-update-card.component';
import { ManzalatSettingComponent } from './manzalat-setting/manzalat-setting.component';
import { AdminOrganizationUnitGroupsComponent } from './organization/organization-unit-groups/organization-unit-groups.component';
import { AdminCheckStateLifeListComponent } from './sabtAhval/chek-state-life-list/check-state-life-list.component';
import { AdminManzalatBaseFromListComponent } from './manzalat-base-form/manzalat-base-list/manzalat-base-list.component';
import { AdminUpdateManzalatBaseFormComponent } from './manzalat-base-form/update-manzalat-base-form/update-manzalat-base-form.component';
import { AdminConfigComponent } from './config/config.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: AdminDashboardComponent },
   
      { path: 'news-list', component: AdminNewsListComponent },
      { path: 'addupdate-news/:id', component: AdminAddOrUpdateNewsComponent },
      { path: 'news-comments/:id', component: AdminNewsCommentsComponent },
      { path: 'news-groups', component: AdminNewsGroupsComponent },
      { path: 'pages', component: AdminPageListComponent },
      { path: 'addupdate-page/:id', component: AdminAddOrUpdatePageComponent },
      { path: 'slide-show', component: AdminSlideShowListComponent },

      { path: 'notifications', component: AdminNotificationListComponent },
      { path: 'addupdate-notification/:id', component: AdminAddOrUpdateNotificationComponent },
  

      { path: 'ticket-subjects', component: AdminTicketSubjectsComponent },

      { path: 'manzalat-form-list', component: AdminManzalatBaseFromListComponent },
      { path: 'update-manzalat-form/:id', component: AdminUpdateManzalatBaseFormComponent },

      { path: 'faq-groups', component: AdminFaqGroupsComponent },
      { path: 'faq-list', component: AdminFaqListComponent },
      { path: 'add-update-faq/:id', component: AdminAddOrUpdateFaqComponent },

      //organization
      { path: 'organization', component: AdminOrganizationListComponent },
      { path: 'units/:id', component: AdminUnitListComponent },
      { path: 'organization-unit-groups/:id', component: AdminOrganizationUnitGroupsComponent },

      //Group
      { path: 'group-list', component: AdminGroupListComponent },
      { path: 'add-update-group/:id', component: AdminAddOrUpdateGroupsComponent },


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
     

      //financial


      { path: 'sms-list', component: AdminSmsListComponent },

      { path: 'pay-test', component: AdminPayTestComponent },

    

      //refund
   
      //citizens
     
    ],
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}

