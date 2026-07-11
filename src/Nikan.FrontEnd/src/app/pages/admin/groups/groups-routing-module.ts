import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminAddOrUpdateGroupsComponent } from './add-or-update-groups/add-or-update-groups.component';
import { AdminGroupListComponent } from './groups-list/groups-list.component';

const routes: Routes = [
  { path: '', component: AdminGroupListComponent },
  { path: 'add-update-group/:id', component: AdminAddOrUpdateGroupsComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GroupsRoutingModule {}
