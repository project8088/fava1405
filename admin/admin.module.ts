import { AdminAddFeedBackDialogComponent } from './_dialogs/add-feedback/add-feedback.component';
import { AdminAddSabtAhvalDialogComponent } from './sabtAhval/dialog/add-sabtAhval/add-sabtAhval.component';
import { AdminComponent } from './admin.component';
import { AdminContactUsListComponent } from './contact-list/contact-list.component';
import { AdminOrganizationListComponent } from './organization/organization-list/organization-list.component';
import { AdminPaySettingComponent } from './pay-setting/pay-setting.component';
import { AdminPayTestComponent } from './pay-test/pay-test.component';
import { AdminRoutingModule } from './admin-routing.module';
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
import { AdminUpdateManzalatBaseFormComponent } from './manzalat-base-form/update-manzalat-base-form/update-manzalat-base-form.component';
import { AdminManzalatBaseFromListComponent } from './manzalat-base-form/manzalat-base-list/manzalat-base-list.component';
import { AdminConfigComponent } from './config/config.component';

@NgModule({
  declarations: [
    AdminComponent,
    AdminOrganizationListComponent,
    AdminUnitListComponent,
    AdminSettingComponent,
    SmsSettingComponent,
    AdminContactUsListComponent,
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
 
    ManzalatSettingComponent,
    AdminOrganizationUnitGroupsComponent,
    AdminUpdateManzalatBaseFormComponent,
    AdminManzalatBaseFromListComponent,
    AdminConfigComponent,
  ],

  imports: [
    AdminRoutingModule,
  ],
})
export class AdminModule {}
