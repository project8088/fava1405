import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CitizenRoutingModule } from './citizen-routing-module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { CoreModule } from '@core/core.module';
import { MaterialModule } from '@core/material/material.module';
import { AdminCitizenAdvancedSearchComponent } from './citizen-advanced-search/citizen-advanced-search.component';
import { AdminCitizenFamilyDetailsComponent } from './citizen-family-details/citizen-family-details.component';
import { AdminCitizensComponent } from './citizens.component';
import { AdminCitizensFamilyComponent } from './citizen-family/citizens-family.component';
import { AdminCitizensPicturesComponent } from './citizens-pictures/citizens-pictures.component';
import { AdminEditCitizenInfoComponent } from './edit-citizen-info/edit-citizen-info.component';
import { AdminManageGroupsCitizenComponent } from './manage-groups-citizens/manage-group-citizen.component';
import { AdminManzelatCitizensComponent } from './manzelat-citizens/manzelat-citizens.component';
import { AdminManzelatCitizensDetailsComponent } from './manzelat-citizens-details/manzelat-citizens-details.component';
import { AdminCitizenCardAdvancedSearchComponent } from './citizens-cards/citizen-card-advanced-search/citizen-card-advanced-search.component';
import { AdminBackCitizenCardDialogComponent } from './citizens-cards/dialog/back-citizen-card/back-citizen-card.component';
import { AdminDeliveredCitizenCardDialogComponent } from './citizens-cards/dialog/delivered-citizen-card/delivered-citizen-card.component';
import { AdminCancellationCitizenCardDialogComponent } from './citizens-cards/dialog/cancellation-citizen-card/cancellation-citizen-card.component';
import { AdminCitizenCardExportSearchComponent } from './citizens-cards/citizen-card-export-search/citizen-card-export-search.component';
import { AdminCitizenCardExportDetailsComponent } from './citizens-cards/citizen-card-export-details/citizen-card-export-details.component';
import { AdminImportCardNumberDialogComponent } from './citizens-cards/dialog/import-card-number/import-card-number.component';
import { AdminAllCitizensFeedBacksComponent } from './all-citizens-feed-backs/all-citizens-feed-backs.component';
import { AdminCitizensInGroupsComponent } from './citizens-in-group/citizens-in-group.component';
import { AdminCitizenExcelBatchFileListComponent } from './citizen-excel-batch-file/citizen-excel-batch-file.component';
import { AdminCitizenExcelBatchFileDetailsComponent } from './citizen-excel-batch-file-details/citizen-excel-batch-file-details.component';
import { AdminImportCitizenExcelDialogComponent } from './_dialog/citizen-import-excel/import-citizen-excel.component';
import { AdminCitizenAuthenticationComponent } from './citizens-authentication/citizens-authentication.component';
import { AdminUpdateCitizenMobileNumberDialogComponent } from './_dialog/update-citizen-mobile-number/update-citizen-mobile-number.component';
import { AdminUpdateCitizenSabtStateDialogComponent } from './_dialog/update-citizen-sabt-state/update-citizen-sabt-state.component';
import { AdminCitizenAuthenticationSearchComponent } from './citizen-authentication-search/citizen-authentication-search.component';
import { AdminCitizenIdentityInfoComponent } from './citizen-profile-edit/citizen-Identity-Info/citizen-Identity-Info.component';
import { AdminUpdateCitizenIdentityInfoComponent } from './citizen-profile-edit/update-citizen-Identity-Info/update-citizen-Identity-Info.component';
import { AdminAppserviceListComponent } from './citizen-apps/appservice-list/appservice-list.component';

import { AdminCitizenProfileComponent } from './citizen-profile-edit/citizen-profile.component';
import { AdminCitizenBaseInfoComponent } from './citizen-profile-edit/base-info/base-info.component';
import { AdminCitizenOtherInfoComponent } from './citizen-profile-edit/other-info/other-info.component';
import { AdminCitizenAddressInfoComponent } from './citizen-profile-edit/address-info/address-info.component';
import { AdminAddOrUpdateAppserviceComponent } from './citizen-apps/add-or-update-appservice/add-or-update-appservice.component';
import { AdminCitizenRejectFamilyComponent } from './_dialog/citizen-reject-family/citizen-reject-family.component';
import { AdminCitizenManzelatReviewComponent } from './_dialog/citizen-manzelat-review/citizen-manzelat-review.component';
import { AdminGroupQueueCitizensListComponent } from './group-queue-citizens-list/group-queue-citizens-list.component';

@NgModule({
  declarations: [
    AdminCitizenCardAdvancedSearchComponent,
    AdminBackCitizenCardDialogComponent,
    AdminDeliveredCitizenCardDialogComponent,
    AdminCancellationCitizenCardDialogComponent,
    AdminCitizenCardExportSearchComponent,
    AdminCitizenCardExportDetailsComponent,
    AdminImportCardNumberDialogComponent,
    AdminAllCitizensFeedBacksComponent,
    AdminCitizensComponent,
    AdminCitizensPicturesComponent,
    AdminCitizensFamilyComponent,
    AdminCitizenFamilyDetailsComponent,
    AdminManzelatCitizensComponent,
    AdminManzelatCitizensDetailsComponent,
    AdminManageGroupsCitizenComponent,
    AdminCitizenAdvancedSearchComponent,
    AdminEditCitizenInfoComponent,
    AdminCitizensInGroupsComponent,
    AdminCitizenExcelBatchFileListComponent,
    AdminCitizenExcelBatchFileDetailsComponent,
    AdminImportCitizenExcelDialogComponent,
    AdminCitizenAuthenticationComponent,
    AdminUpdateCitizenMobileNumberDialogComponent,
    AdminUpdateCitizenSabtStateDialogComponent,
    AdminCitizenAuthenticationSearchComponent,
    AdminAppserviceListComponent,
    AdminAddOrUpdateAppserviceComponent,
    AdminCitizenProfileComponent,
    AdminCitizenBaseInfoComponent,
    AdminCitizenOtherInfoComponent,
    AdminCitizenAddressInfoComponent,
    AdminCitizenIdentityInfoComponent,
    AdminUpdateCitizenIdentityInfoComponent,
    AdminCitizenRejectFamilyComponent,
    AdminCitizenManzelatReviewComponent,
    AdminGroupQueueCitizensListComponent
  ],
  imports: [
    CommonModule,
    CitizenRoutingModule,
    MaterialModule,
    CoreModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
  ],
})
export class CitizenModule {}
