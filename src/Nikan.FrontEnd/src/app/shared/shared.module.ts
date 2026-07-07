import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { AttachmentListComponent } from './attachments/attachments.component';
import { CaptchaComponent } from './captcha/captcha.component';
import { ChangeCurrentUserPasswordComponent } from './change-current-user-password/change-current-user-password.component';
import { CitizenProfileDialogComponent } from './_dialog/citizen-profile/citizen-profile.component';
import { CommonModule } from '@angular/common';
import { CompanyInfoComponent } from './company-info/company-info.component';
import { CoreModule } from '../core/core.module';
import { CropperComponent } from './cropper/cropper.component';
import { ImageCropperComponent } from 'ngx-image-cropper';
import { JobseekerProfileDialogComponent } from './_dialog/jobseeker-profile/jobseeker-profile.component';
import { MatIconModule } from '@angular/material/icon';
// import { MatTableExporterModule } from 'mat-table-exporter';
import { MaterialModule } from '../core/material/material.module';
import { MenuDynamicComponent } from './side-nav-menu/menu-dynamic/menu-dynamic.component';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { NgxUploaderModule } from 'ngx-uploader';
import { RouterModule } from '@angular/router';
import { SendTicketDialogComponent } from './ticket/_dialogs/send-ticket/send-ticket.component';
import { ShowImageDialogComponent } from './_dialog/show-image/show-image.component';
import { SideNavMenuComponent } from './side-nav-menu/side-nav-menu.component';
import { StoreDetailsComponent } from './store-details/store-details.component';
import { TicketActivityComponent } from './ticket/view-ticket/tabs/ticket-activity/ticket-activity.component';
import { TicketCommentsComponent } from './ticket/view-ticket/tabs/ticket-comments/ticket-comments.component';
import { TicketDetailsComponent } from './ticket/view-ticket/ticket-details.component';
import { TicketListComponent } from './ticket/ticket-list/ticket-list.component';
import { TicketResponseComponent } from './ticket/view-ticket/tabs/ticket-response/ticket-response.component';
import { TimerComponent } from './timer/timer.component';
import { TransactionDetailsComponent } from './transaction-details/transaction-details.component';
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

@NgModule({
  declarations: [
    SideNavMenuComponent,
    MenuDynamicComponent,
    UploaderComponent,
    SendTicketDialogComponent,
    TicketListComponent,
    TicketDetailsComponent,
    TicketResponseComponent,
    TicketActivityComponent,
    TicketCommentsComponent,
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
    TransactionDetailsComponent,
    CaptchaComponent,
    CitizenProfileDialogComponent,
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
    NgxMaterialTimepickerModule,
    //MatTableExporterModule,
    MatIconModule,
  ],
  exports: [
    ImageCropperComponent,
    SideNavMenuComponent,
    UploaderComponent,
    SendTicketDialogComponent,
    TicketListComponent,
    TicketDetailsComponent,
    TicketResponseComponent,
    TicketActivityComponent,
    TicketCommentsComponent,
    ViewJobDetailsDialogComponent,
    ViewJobDetailsComponent,
    JobseekerProfileDialogComponent,
    CitizenProfileDialogComponent,
    CardProfileDialogComponent,
    ViewCitizenProfileComponent,
    ViewCardProfileComponent,
    ViewJobseekerProfileComponent,
    CompanyInfoComponent,
    NgxMaterialTimepickerModule,
    ChangeCurrentUserPasswordComponent,
    ViewNotificationDetailsComponent,
    ViewNotificationDetailsDialogComponent,
    UploadUserAvatarDialogComponent,
    StoreDetailsComponent,
    TransactionDetailsComponent,
    // MatTableExporterModule,
    AttachmentListComponent,
    CaptchaComponent,
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
