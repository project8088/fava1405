/// <reference path="../../../../core/models/users/usergroups.ts" />

import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { userGroupsDto } from '@core/models/users/userGroups';
import Swal from 'sweetalert2';
import { AdminAddUserGrousDialogComponent } from '../dialogs/add-usergroups/add-usergroups.component';
import { ServerApis } from '../../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-admin-userGroup',
  templateUrl: './admin-userGroups.component.html',
  styleUrls: ['./admin-userGroups.component.scss'],
})
export class AdminUserGroupsComponent extends AppBase implements OnInit {
  userGroupList: userGroupsDto[] = [];
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  loading: boolean = true;

  groupList: any[] = [];

  constructor(
    private customValidator: CustomFormValidators
  ) {
      super();}

  ngOnInit() {
    this.getList();
  }

  getList() {
    this.loading = true;
    this.dataService.get(ServerApis.getAllUserGroups, {}).subscribe(
      (response) => {
        this.loading = false;
        if (response && response.isSuccess) {
          this.userGroupList = response.data ? response.data : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loading = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  openAddUserDialog() {
    this.matDialog
      .open(AdminAddUserGrousDialogComponent, {
        panelClass: 'custom-dialog',
        data: {},
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getList();
      });
  }

  openUpdateUserDialog(item) {
    this.matDialog
      .open(AdminAddUserGrousDialogComponent, {
        panelClass: 'custom-dialog',
        data: {
          userGroups: item,
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getList();
      });
  }

  deleteUserGroup(row) {
    Swal.fire({
      title: 'حذف گروه کاربری',
      text: 'آیا برای حذف گروه اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService
          .get(ServerApis.removeUserGroups, {
            id: row.id,
          })
          .subscribe(
            (response) => {
              if (response.isSuccess) {
                this.toastrService.success('با موفقیت حذف شد.');
                this.getList();
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            },
            (error) => {},
          );
      }
    });
  }
}
