
import { AdminComponent } from './admin.component';
import { AdminContactUsListComponent } from './contact-list/contact-list.component';
import { AdminPaySettingComponent } from './pay-setting/pay-setting.component';
import { AdminPayTestComponent } from './pay-test/pay-test.component';
import { AdminSettingComponent } from './setting/setting.component';
import { AdminSmsListComponent } from './sms-list/sms-list.component';

import { SmsSettingComponent } from './sms-setting/sms-setting.component';
import { AdminConfigComponent } from './config/config.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
   
  


  

      { path: 'contact-us', component: AdminContactUsListComponent },
      { path: 'setting', component: AdminSettingComponent },
      { path: 'pay-setting', component: AdminPaySettingComponent },
      { path: 'sms-setting', component: SmsSettingComponent },


      { path: 'config', component: AdminConfigComponent },

 
      //SabtAhval
   
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

