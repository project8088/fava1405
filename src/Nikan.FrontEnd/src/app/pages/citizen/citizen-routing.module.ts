import { RouterModule, Routes } from '@angular/router';

import { ChangeCurrentUserPasswordComponent } from '@app/shared/change-current-user-password/change-current-user-password.component';
import { CitizenComponent } from './citizen.component';
import { CitizenContactComponent } from './profile/contact/contact.component';
import { CitizenDashboardComponent } from './dashboard/dashboard.component';
import { CitizenDetailedInfoComponent } from './profile/detailed-info/detailed-info.component';
import { CitizenDocumentsComponent } from './profile/documents/documents.component';
import { CitizenEditProfileComponent } from './profile/edit-profile/edit-profile.component';
import { CitizenEducationComponent } from './profile/education/education.component';
import { CitizenMyFamilyComponent } from './profile/my-family/my-family.component';
import { CitizenNotificationsComponent } from './profile/notifications/notifications.component';
import { CitizenPersonalInfoComponent } from './profile/personal-info/personal-info.component';
import { CitizenPersonnelImageComponent } from './profile/personnel-image/personnel-image.component';
import { CitizenProfileComponent } from './profile/profile.component';
import { CitizenTransactionListComponent } from './transaction-list/transaction-list.component';
import { CreditcardComponent } from './profile/creditcard/creditcard.component';
import { NgModule } from '@angular/core';
import { CitizenRefundStatusComponent } from './refund/citizen-refund-status.component';
import { CitizenRefundAccessDetailsListComponent } from './refund/citizen-refund-access-details-list/citizen-refund-access-details-list.component';
import { CitizenRefundFullInfoComponent } from './refund/citizen-refund-info/citizen-refund-info.component';
import { CitizenUploadManzalatDocumentsComponent } from './manzelat-plan/upload-manzalat-file/upload-manzalat-file.component';
import { CitizenManzelatPlanItemComponent } from './manzelat-plan/manzelat-plan-item/manzelat-plan-item.component';
import { PlansListComponent } from './manzelat-plan/plans-list/plans-list.component';
import { AdminAddOrUpdateCitizensNotificationComponent } from './citizens-notification/citizens-notification.component';

const routes: Routes = [
  {
    path: '',
    component: CitizenComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'citizen-notifications/:id',
        component: AdminAddOrUpdateCitizensNotificationComponent,
      },
      { path: 'dashboard', component: CitizenDashboardComponent },
      { path: 'upload-manzalat-form/:id', component: CitizenUploadManzalatDocumentsComponent },
      { path: 'manzelat-plan-item/:id', component: CitizenManzelatPlanItemComponent },
      { path: 'manzelat-plan', component: PlansListComponent },
      { path: 'citizen-refund-status', component: CitizenRefundStatusComponent },
      {
        path: 'profile/:id',
        component: CitizenProfileComponent,
        children: [
          { path: '', redirectTo: 'personal-info', pathMatch: 'full' },
          { path: 'personal-info', component: CitizenPersonalInfoComponent },
          { path: 'my-family', component: CitizenMyFamilyComponent },
          { path: 'educations', component: CitizenEducationComponent },
          { path: 'edit-profile', component: CitizenEditProfileComponent },
          { path: 'creditcard', component: CreditcardComponent },
          { path: 'contact', component: CitizenContactComponent },
          { path: 'additional-information', component: CitizenDetailedInfoComponent },
          {
            path: 'personnel-image',
            component: CitizenPersonnelImageComponent,
          },
          {
            path: 'documents',
            component: CitizenDocumentsComponent,
          },

          {
            path: 'notifications',
            component: CitizenNotificationsComponent,
          },
        ],
      },

      {
        path: 'citizen-card',
        loadChildren: () =>
          import('./citizen-card/citizen-card.module').then((m) => m.CitizenCardModule),
      },

      { path: 'transaction-list', component: CitizenTransactionListComponent },
      {
        path: 'refundAccess-details/:importId',
        component: CitizenRefundAccessDetailsListComponent,
      },
      { path: 'citizen-refund-info/:refundId', component: CitizenRefundFullInfoComponent },

      {
        path: 'change-password',
        component: ChangeCurrentUserPasswordComponent,
      },
    ],
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CitizenRoutingModule {}
