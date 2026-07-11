
import { AdminComponent } from './admin.component';
import { AdminContactUsListComponent } from './contact-list/contact-list.component';
import { AdminOrganizationListComponent } from './organization/organization-list/organization-list.component';
import { AdminPaySettingComponent } from './pay-setting/pay-setting.component';
import { AdminPayTestComponent } from './pay-test/pay-test.component';
import { AdminSabtAhvalCitizensListComponent } from './sabtAhval/get-sabtAhval-citizen-list/get-sabtAhval-citizen-list.component';
import { AdminSabtAhvalListComponent } from './sabtAhval/sabtAhval-list/sabtAhval-list.component';
import { AdminSettingComponent } from './setting/setting.component';
import { AdminSmsListComponent } from './sms-list/sms-list.component';
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
   
  


      { path: 'manzalat-form-list', component: AdminManzalatBaseFromListComponent },
      { path: 'update-manzalat-form/:id', component: AdminUpdateManzalatBaseFormComponent },


      //organization
      { path: 'organization', component: AdminOrganizationListComponent },
      { path: 'units/:id', component: AdminUnitListComponent },
      { path: 'organization-unit-groups/:id', component: AdminOrganizationUnitGroupsComponent },

      //Group
   

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

