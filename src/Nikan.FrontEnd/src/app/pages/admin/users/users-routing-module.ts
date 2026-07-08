import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminAddOrUpdateManagerComponent } from './add-or-update-manager/add-or-update-manager.component';
import { AdminManagerUsersComponent } from './manager-users/manager-users.component';
import { AdminUserGroupsComponent } from './admin-userGroups/admin-userGroups.component';
import { AdminUserPermissionsComponent } from './user-permissions/user-permissions.component';
import { AdminUsersComponent } from './admin-users/admin-users.component';
import { UserRoleListComponent } from './user-role/user-role.component';
import { AdminWebApiUsersComponent } from './web-api-users/web-api-users.component';
import { AdminWebApiUserPermissionsComponent } from './web-api-user-permissions/web-api-user-permissions.component';
import { AdminUserAppAccessServiceComponent } from './user-access-app-service/user-access-app-service.component';
import { AdminAllUsersComponent } from './all-users/all-users.component';
import { AdminUserAccessGroupPermissionsComponent } from './user-access-group-permissions/user-access-group-permissions.component';
import { AdminUserAccessIpComponent } from './user-access-ip/user-access-ip.component';
import { ChangeCurrentUserPasswordComponent } from '@app/shared/change-current-user-password/change-current-user-password.component';

const routes: Routes = [
  { path: 'admin-users', component: AdminUsersComponent },
  { path: 'all-users', component: AdminAllUsersComponent },
  { path: 'web-api-users', component: AdminWebApiUsersComponent },
  { path: 'admin-userGroups', component: AdminUserGroupsComponent },
  { path: 'manager-users', component: AdminManagerUsersComponent },
  { path: 'addupdate-manager/:id', component: AdminAddOrUpdateManagerComponent },
  { path: 'change-password', component: ChangeCurrentUserPasswordComponent },
  { path: 'permissions/:id', component: AdminUserPermissionsComponent },
  { path: 'web-api-permissions/:id', component: AdminWebApiUserPermissionsComponent },
  { path: 'user-roles/:id', component: UserRoleListComponent },
  { path: 'user-access-service/:id', component: AdminUserAppAccessServiceComponent },
  {
    path: 'user-group-access-permissions/:id',
    component: AdminUserAccessGroupPermissionsComponent,
  },
  { path: 'user-access-ip/:id', component: AdminUserAccessIpComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UsersRoutingModule {}
