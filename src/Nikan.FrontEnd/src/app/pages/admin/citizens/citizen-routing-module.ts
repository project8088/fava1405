import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppShowCitizenComponent } from '@app/shared/citizens/show-citizens/show-citizen.component';
import { AdminAllCitizensFeedBacksComponent } from './all-citizens-feed-backs/all-citizens-feed-backs.component';
import { AdminCitizenAdvancedSearchComponent } from './citizen-advanced-search/citizen-advanced-search.component';
import { AdminAddOrUpdateAppserviceComponent } from './citizen-apps/add-or-update-appservice/add-or-update-appservice.component';
import { AdminAppserviceListComponent } from './citizen-apps/appservice-list/appservice-list.component';
import { AdminCitizenAuthenticationSearchComponent } from './citizen-authentication-search/citizen-authentication-search.component';
import { AdminCitizenExcelBatchFileDetailsComponent } from './citizen-excel-batch-file-details/citizen-excel-batch-file-details.component';
import { AdminCitizenExcelBatchFileListComponent } from './citizen-excel-batch-file/citizen-excel-batch-file.component';
import { AdminCitizenFamilyDetailsComponent } from './citizen-family-details/citizen-family-details.component';
import { AdminCitizensFamilyComponent } from './citizen-family/citizens-family.component';
import { AdminCitizenProfileComponent } from './citizen-profile-edit/citizen-profile.component';
import { AdminCitizenAuthenticationComponent } from './citizens-authentication/citizens-authentication.component';
import { AdminCitizenCardAdvancedSearchComponent } from './citizens-cards/citizen-card-advanced-search/citizen-card-advanced-search.component';
import { AdminCitizenCardExportDetailsComponent } from './citizens-cards/citizen-card-export-details/citizen-card-export-details.component';
import { AdminCitizenCardExportSearchComponent } from './citizens-cards/citizen-card-export-search/citizen-card-export-search.component';
import { AdminCitizensInGroupsComponent } from './citizens-in-group/citizens-in-group.component';
import { AdminCitizensPicturesComponent } from './citizens-pictures/citizens-pictures.component';
import { AdminCitizensComponent } from './citizens.component';
import { AdminEditCitizenInfoComponent } from './edit-citizen-info/edit-citizen-info.component';
import { AdminManageGroupsCitizenComponent } from './manage-groups-citizens/manage-group-citizen.component';
import { AdminManzelatCitizensDetailsComponent } from './manzelat-citizens-details/manzelat-citizens-details.component';
import { AdminManzelatCitizensComponent } from './manzelat-citizens/manzelat-citizens.component';
import { AdminGroupQueueCitizensListComponent } from './group-queue-citizens-list/group-queue-citizens-list.component';

const routes: Routes = [
  { path: 'citizen-in-group/:id', component: AdminCitizensInGroupsComponent },
  { path: 'show-citizen/:id', component: AppShowCitizenComponent },
  { path: 'manage-citizen-groups/:id', component: AdminManageGroupsCitizenComponent },
  { path: 'citizen-register-file-list', component: AdminCitizenExcelBatchFileListComponent },
  {
    path: 'citizen-register-file-details/:importId',
    component: AdminCitizenExcelBatchFileDetailsComponent,
  },
  { path: 'search-citizen', component: AdminCitizensComponent },
  { path: 'advanced-search-citizen', component: AdminCitizenAdvancedSearchComponent },
  { path: 'search-citizen-family', component: AdminCitizensFamilyComponent },
  { path: 'citizen-family-details/:id', component: AdminCitizenFamilyDetailsComponent },
  { path: 'appService-list', component: AdminAppserviceListComponent },
  { path: 'addupdate-appService/:id', component: AdminAddOrUpdateAppserviceComponent },
  { path: 'citizens-pictures', component: AdminCitizensPicturesComponent },
  { path: 'search-manzelat-citizens', component: AdminManzelatCitizensComponent },
  { path: 'citizen-manzelat-details/:id', component: AdminManzelatCitizensDetailsComponent },
  { path: 'edit-citizen-info/:id', component: AdminEditCitizenInfoComponent },
  { path: 'edit-profile/:id', component: AdminCitizenProfileComponent },
  { path: 'all-citizens-feedBacks', component: AdminAllCitizensFeedBacksComponent },
  { path: 'citizens-authentication', component: AdminCitizenAuthenticationComponent },
  {
    path: 'citizens-authentication-search',
    component: AdminCitizenAuthenticationSearchComponent,
  },
  { path: 'group-queue-citizens-list/:id', component: AdminGroupQueueCitizensListComponent },

  //citizen Card
  { path: 'advanced-search-card-citizen', component: AdminCitizenCardAdvancedSearchComponent },
  { path: 'export-search-card-citizen', component: AdminCitizenCardExportSearchComponent },
  {
    path: 'export-details-citizen-card/:id',
    component: AdminCitizenCardExportDetailsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CitizenRoutingModule {}
