import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BuyCardDialogComponent } from './_dialogs/buy-card/buy-card.component';
import { CitizenCardModule } from './citizen-card/citizen-card.module';
import { CitizenComponent } from './citizen.component';
import { CitizenContactComponent } from './profile/contact/contact.component';
import { CitizenDashboardComponent } from './dashboard/dashboard.component';
import { CitizenDetailedInfoComponent } from './profile/detailed-info/detailed-info.component';
import { CitizenDocumentsComponent } from './profile/documents/documents.component';
import { CitizenEditEmailComponent } from './profile/notifications/edit-email/edit-email.component';
import { CitizenEditMobileComponent } from './profile/notifications/edit-mobile/edit-mobile.component';
import { CitizenEditProfileComponent } from './profile/edit-profile/edit-profile.component';
import { CitizenEducationComponent } from './profile/education/education.component';
import { CitizenEducationDialogComponent } from './profile/_dialogs/education-dialog/education-dialog.component';
import { CitizenFamilyDialogComponent } from './profile/_dialogs/family-dialog/family-dialog.component';
import { CitizenMyFamilyComponent } from './profile/my-family/my-family.component';
import { CitizenNotificationsComponent } from './profile/notifications/notifications.component';
import { CitizenPersonalInfoComponent } from './profile/personal-info/personal-info.component';
import { CitizenPersonnelImageComponent } from './profile/personnel-image/personnel-image.component';
import { CitizenProfileComponent } from './profile/profile.component';
import { CitizenRoutingModule } from './citizen-routing.module';
import { CitizenTransactionListComponent } from './transaction-list/transaction-list.component';
import { CommonModule } from '@angular/common';
import { CoreModule } from '@core/core.module';
import { CreditcardComponent } from './profile/creditcard/creditcard.component';
import { MaterialModule } from '@core/material/material.module';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '@app/shared/shared.module';
import { CitizenRefundStatusComponent } from './refund/citizen-refund-status.component';
import { CitizenRefundAccessListComponent } from './refund/citizen-refund-access-list/citizen-refund-access-list.component';
import { CitizenRefundAccessDetailsListComponent } from './refund/citizen-refund-access-details-list/citizen-refund-access-details-list.component';
import { CitizenRefundFullInfoComponent } from './refund/citizen-refund-info/citizen-refund-info.component';
import { CitizenMyIdentityInfoComponent } from './profile/my-Identity-Info/my-Identity-Info.component';
import { CitizenUpdateIdentityInfoComponent } from './profile/update-Identity-Info/update-Identity-Info.component';
import { ChangeCardAddressDialogComponent } from './_dialogs/change-card-address/change-card-address.component';
import { CitizenUploadManzalatDocumentsComponent } from './manzelat-plan/upload-manzalat-file/upload-manzalat-file.component';
import { CitizenManzelatPlanItemComponent } from './manzelat-plan/manzelat-plan-item/manzelat-plan-item.component';
import { PlansListComponent } from './manzelat-plan/plans-list/plans-list.component';
import { AdminAddOrUpdateCitizensNotificationComponent } from './citizens-notification/citizens-notification.component';

@NgModule({
  declarations: [
    CitizenComponent,
    CitizenDashboardComponent,
    CitizenProfileComponent,
    CitizenPersonalInfoComponent,
    CitizenEducationComponent,
    CitizenEducationDialogComponent,
    CitizenFamilyDialogComponent,
    CitizenEditProfileComponent,
    CitizenMyFamilyComponent,
    CreditcardComponent,
    CitizenDetailedInfoComponent,
    CitizenPersonnelImageComponent,
    CitizenDocumentsComponent,
    CitizenTransactionListComponent,
    CitizenEditMobileComponent,
    CitizenContactComponent,
    CitizenNotificationsComponent,
    CitizenEditEmailComponent,
    BuyCardDialogComponent,
    CitizenRefundStatusComponent,
    CitizenRefundAccessListComponent,
    CitizenRefundAccessDetailsListComponent,
    CitizenRefundFullInfoComponent,
    CitizenMyIdentityInfoComponent,
    CitizenUpdateIdentityInfoComponent,
    ChangeCardAddressDialogComponent,
    CitizenUploadManzalatDocumentsComponent,
    CitizenManzelatPlanItemComponent,
    PlansListComponent,
    AdminAddOrUpdateCitizensNotificationComponent,
  ],
  imports: [
    CommonModule,
    MaterialModule,
    CoreModule,
    SharedModule,
    CitizenRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    CitizenCardModule,
  ],
})
export class CitizenModule {}
