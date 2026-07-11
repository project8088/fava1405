import { AdminAddFeedBackDialogComponent } from './_dialogs/add-feedback/add-feedback.component';
import { AdminComponent } from './admin.component';
import { AdminContactUsListComponent } from './contact-list/contact-list.component';
import { AdminPaySettingComponent } from './pay-setting/pay-setting.component';
import { AdminPayTestComponent } from './pay-test/pay-test.component';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminSettingComponent } from './setting/setting.component';
import { AdminSmsListComponent } from './sms-list/sms-list.component';


import { SmsSettingComponent } from './sms-setting/sms-setting.component';
import { AdminConfigComponent } from './config/config.component';

@NgModule({
  declarations: [
    AdminComponent,
    AdminSettingComponent,
    SmsSettingComponent,
    AdminContactUsListComponent,
    AdminSmsListComponent,
    AdminPayTestComponent,
    AdminPaySettingComponent,

    AdminAddFeedBackDialogComponent,
 
    AdminConfigComponent,
  ],

  imports: [
    AdminRoutingModule,
  ],
})
export class AdminModule {}
