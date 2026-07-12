import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { AdminAddUserGrousDialogComponent } from '../dialogs/add-usergroups/add-usergroups.component';
import { ServerApis } from '@core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { AppBase } from '@app/app.base';
import { userGroupsDto } from '@core/models/users/userGroups';
import { finalize } from 'rxjs';

@Component({
  selector: 'adm-admin-userGroup',
  templateUrl: './admin-userGroups.component.html',
  styleUrls: ['./admin-userGroups.component.scss'],
  standalone: false,
})
export class AdminUserGroupsComponent extends AppBase implements OnInit {
  userGroupList: userGroupsDto[] = [];
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  loading: boolean = true;

  groupList: any[] = [];

  constructor(private customValidator: CustomFormValidators) {
    super();
  }

  ngOnInit() {
    this.getList();
  }

  getList() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getAllUserGroups, {})
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response && response.isSuccess) {
          this.userGroupList = response.data ? response.data : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      });
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

  openUpdateUserDialog(item: any) {
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

  deleteUserGroup(row: any) {
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
          .subscribe((response) => {
            if (response.isSuccess) {
              this.toastrService.success('با موفقیت حذف شد.');
              this.getList();
            } else {
              let msg = response.messages
                ? response.messages
                : 'متاسفانه خطایی در سرور رخ داده است!';
              this.toastrService.error(msg);
            }
          });
      }
    });
  }
}
