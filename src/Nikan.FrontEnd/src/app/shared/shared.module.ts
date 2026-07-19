import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { AttachmentListComponent } from './attachments/attachments.component';
import { ChangeCurrentUserPasswordComponent } from './change-current-user-password/change-current-user-password.component';
import { CitizenProfileDialogComponent } from './_dialog/citizen-profile/citizen-profile.component';
import { CommonModule } from '@angular/common';
import { CompanyInfoComponent } from './company-info/company-info.component';
import { CoreModule } from '@core/core.module';
import { CropperComponent } from './cropper/cropper.component';
import { ImageCropperComponent } from 'ngx-image-cropper';
import { JobseekerProfileDialogComponent } from './_dialog/jobseeker-profile/jobseeker-profile.component';
import { MatIconModule } from '@angular/material/icon';
// import { MatTableExporterModule } from 'mat-table-exporter';
import { MaterialModule } from '@core/material/material.module';
import { NgxUploaderModule } from 'ngx-uploader';
import { RouterModule } from '@angular/router';
import { ShowImageDialogComponent } from './_dialog/show-image/show-image.component';
import { SideNavMenuComponent } from './side-nav-menu/side-nav-menu.component';
import { StoreDetailsComponent } from './store-details/store-details.component';
import { TimerComponent } from './timer/timer.component';
import { UploadUserAvatarDialogComponent } from './_dialog/upload-avatar/upload-avatar.component';
import { UploaderComponent } from './uploader/uploader.component';
import { ViewJobDetailsComponent } from './_dialog/job-details/view-job-details/view-job-details.component';
import { ViewJobDetailsDialogComponent } from './_dialog/job-details/job-details.component';
import { ViewJobseekerProfileComponent } from './_dialog/jobseeker-profile/view-jobseeker-profile/view-jobseeker-profile.component';
import { ViewNotificationDetailsComponent } from './_dialog/notification-details/notification-details/notification-details.component';
import { ViewNotificationDetailsDialogComponent } from './_dialog/notification-details/notification-details.component';
import { CardProfileDialogComponent } from './_dialog/card-profile/card-profile.component';
import { ViewCardProfileComponent } from './_dialog/card-profile/view-card-profile/view-card-profile.component';
import { ViewCitizenProfileComponent } from './_dialog/citizen-profile/view-citizen-profile/view-citizen-profile.component';
import { AppCitizenFeedBackListComponent } from './citizens/citizen-feed-backs/citizen-feed-backs.component';
import { AppShowCitizenComponent } from './citizens/show-citizens/show-citizen.component';
import { AppCitizenManzalatRegistrationComponent } from './citizens/citizen-manzalat-registration/citizen-manzalat-registration.component';
import { AppGroupCitizensListComponent } from './citizens/groupCitizens-list/groupCitizens-list.component';
import { AppCitizenSmsListComponent } from './citizens/citizen-sms-list/citizen-sms-list.component';
import { AppCitizenEventListComponent } from './citizens/citizen-event-list/citizen-event-list.component';
import { AdminCitizenTransactionListComponent } from './citizens/transaction-list/citizen-transaction-list.component';
import { ViewCitizenComponent } from './citizens/view-citizen.component';
import { AdminCitizenSmsListDialogComponent } from './_dialog/citizen-sms-list/citizen-sms-list.component';
import { AdminCitizenImageDialogComponent } from './_dialog/citizen-image/citizen-image.component';
import { AdminCitizenEditImageDialogComponent } from './_dialog/citizen-edit-image/citizen-edit-image.component';
import { AdminCitizenRejectImageDialogComponent } from './_dialog/citizen-reject-image/citizen-reject-image.component';
import { AdminChangePasswordDialogComponent } from './_dialog/change-user-password/change-user-password.component';
import { CitizenRefundInfoDialogComponent } from './_dialog/refund-info/refund-info.component';
@NgModule({
  declarations: [
    UploaderComponent,
    ViewJobDetailsDialogComponent,
    ViewJobDetailsComponent,
    JobseekerProfileDialogComponent,
    ViewJobseekerProfileComponent,
    ViewCardProfileComponent,
    CompanyInfoComponent,
    ChangeCurrentUserPasswordComponent,
    ViewNotificationDetailsComponent,
    ViewNotificationDetailsDialogComponent,
    AttachmentListComponent,
    UploadUserAvatarDialogComponent,
    StoreDetailsComponent,
    ViewCitizenComponent,
    CitizenProfileDialogComponent,
    AdminCitizenTransactionListComponent,
    ViewCitizenProfileComponent,
    TimerComponent,
    CropperComponent,
    ShowImageDialogComponent,
    CardProfileDialogComponent,
    AppCitizenFeedBackListComponent,
    AppShowCitizenComponent,
    AppCitizenManzalatRegistrationComponent,
    AppGroupCitizensListComponent,
    AppCitizenSmsListComponent,
    AppCitizenEventListComponent,
    AdminCitizenSmsListDialogComponent,
    AdminCitizenImageDialogComponent,
    AdminCitizenEditImageDialogComponent,
    AdminCitizenRejectImageDialogComponent,
    AdminChangePasswordDialogComponent,
    CitizenRefundInfoDialogComponent,
  ],
  imports: [
    CoreModule,
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    RouterModule,
    NgxUploaderModule,
    ImageCropperComponent,
    //MatTableExporterModule,
    MatIconModule,
  ],
  exports: [
    ImageCropperComponent,
    UploaderComponent,
    ViewJobDetailsDialogComponent,
    ViewJobDetailsComponent,
    JobseekerProfileDialogComponent,
    CitizenProfileDialogComponent,
    AdminCitizenTransactionListComponent,
    ViewCitizenComponent,
    CardProfileDialogComponent,
    ViewCitizenProfileComponent,
    ViewCardProfileComponent,
    ViewJobseekerProfileComponent,
    CompanyInfoComponent,
    ChangeCurrentUserPasswordComponent,
    ViewNotificationDetailsComponent,
    ViewNotificationDetailsDialogComponent,
    UploadUserAvatarDialogComponent,
    StoreDetailsComponent,
    // MatTableExporterModule,
    AttachmentListComponent,
    TimerComponent,
    CropperComponent,
    ShowImageDialogComponent,
    AppCitizenFeedBackListComponent,
    AppShowCitizenComponent,
    AppCitizenManzalatRegistrationComponent,
    AppGroupCitizensListComponent,
    AppCitizenSmsListComponent,
    AppCitizenEventListComponent,
  ],
})
export class SharedModule {
  static forRoot(): ModuleWithProviders<SharedModule> {
    // Forcing the whole app to use the returned providers from the AppModule only.
    return {
      ngModule: SharedModule,
      providers: [/* All of your services here. It will hold the services needed by `itself`. */],
    };
  }
}
