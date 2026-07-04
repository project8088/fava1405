import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { AuthService } from '@core/authentication/auth.service';
import Swal from 'sweetalert2';
import { ServerApis } from '../../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { AdminUpdateUserDialogComponent } from '../dialogs/update-user/update-user.component';
import { AdminChangePasswordDialogComponent } from '../dialogs/change-user-password/change-user-password.component';
import { AdminAddUserDialogComponent } from '../dialogs/add-user/add-user.component';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-user-permissions',
  templateUrl: './user-permissions.component.html',
  styleUrls: ['./user-permissions.component.scss'],
    standalone: false
})
export class AdminUserPermissionsComponent extends AppBase implements OnInit {
  groupId: string;
  data: any[] = [];
  isLoadingResults: boolean = true;
  isSaving: boolean;
  constructor(
    private customValidator: CustomFormValidators
  ) {
      super();
    this.route.params.subscribe((p) => {
      this.groupId = p.id;
    });
  }

  ngOnInit() {
    this.getList();
  }
  getList() {
    this.isLoadingResults = true;
    this.data = [];
    this.dataService
      .get(ServerApis.getPermissionList, {
        groupId: this.groupId,
      })
      .subscribe(
        (response: any) => {
          this.isLoadingResults = false;
          if (response) {
            this.data = response;
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.isLoadingResults = false;
        },
      );
  }

  savePermissions() {
    this.isSaving = true;
    var selectedPermissions = [];
    for (var item in this.data) {
      for (var child of this.data[item]) {
        if (child.selected) selectedPermissions.push(child.key);
      }
    }
    this.dataService
      .post(ServerApis.addPermissions, {
        groupId: this.groupId,
        Permissions: selectedPermissions,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.isSaving = false;
        },
      );
  }
}
