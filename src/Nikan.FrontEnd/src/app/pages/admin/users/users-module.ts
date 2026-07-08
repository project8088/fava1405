import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminAddOrUpdateManagerComponent } from './add-or-update-manager/add-or-update-manager.component';
import { AdminAddUserDialogComponent } from './dialogs/add-user/add-user.component';
import { AdminAddUserGrousDialogComponent } from './dialogs/add-usergroups/add-usergroups.component';
import { AdminManagerUsersComponent } from './manager-users/manager-users.component';
import { AdminWebApiUsersComponent } from './web-api-users/web-api-users.component';
import { AdminAddWebApiUserDialogComponent } from './dialogs/add-webapi-users/add-webapi-users.component';
import { AdminTreeWebApiPermissionComponent } from './web-api-user-permissions/tree-permission/tree-user-web-api-permission.component';
import { AdminWebApiUserPermissionsComponent } from './web-api-user-permissions/web-api-user-permissions.component';
import { AdminUserAppAccessServiceComponent } from './user-access-app-service/user-access-app-service.component';
import { UserRoleListComponent } from './user-role/user-role.component';
import { AdminAllUsersComponent } from './all-users/all-users.component';
import { AdminUserAccessGroupPermissionsComponent } from './user-access-group-permissions/user-access-group-permissions.component';
import { AdminUserAccessIpComponent } from './user-access-ip/user-access-ip.component';
import { AdminUpdateUserDialogComponent } from './dialogs/update-user/update-user.component';
import { AdminUserGroupsComponent } from './admin-userGroups/admin-userGroups.component';
import { AdminUserPermissionsComponent } from './user-permissions/user-permissions.component';
import { AdminUsersComponent } from './admin-users/admin-users.component';
import { AdminTreePermissionComponent } from './user-permissions/tree-permission/tree-permission.component';
import { UsersRoutingModule } from './users-routing-module';
import { MaterialModule } from '@core/material/material.module';
import { CoreModule } from '@core/core.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';

@NgModule({
  declarations: [
    UserRoleListComponent,
    AdminWebApiUsersComponent,
    AdminAddWebApiUserDialogComponent,
    AdminUserAppAccessServiceComponent,
    AdminManagerUsersComponent,
    AdminAddOrUpdateManagerComponent,
    AdminUsersComponent,
    AdminAddUserDialogComponent,
    AdminUpdateUserDialogComponent,
    AdminUserGroupsComponent,
    AdminAddUserGrousDialogComponent,
    AdminUserPermissionsComponent,
    AdminTreePermissionComponent,
    AdminTreeWebApiPermissionComponent,
    AdminWebApiUserPermissionsComponent,
    AdminAllUsersComponent,
    AdminUserAccessGroupPermissionsComponent,
    AdminUserAccessIpComponent,
  ],
  imports: [
    CommonModule,
    UsersRoutingModule,
    MaterialModule,
    CoreModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
  ],
})
export class UsersModule {}
