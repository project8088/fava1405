import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import Swal from 'sweetalert2';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'users-role',
  templateUrl: './user-role.component.html',
  styleUrls: ['./user-role.component.scss'],
  standalone: false,
})
export class UserRoleListComponent extends AppBase implements OnInit {
  loading?: boolean;
  userRoleList: any[] = [];
  roleList: any[] = [];
  userId: string = '';
  form: FormGroup;
  isSaving = false;

  constructor(private customValidators: CustomFormValidators) {
    super();
    this.form = this.fb.group({
      roleId: [null, [Validators.required]],
    });

    this.route.params.subscribe((p) => {
      if (p['id'] != '0' && p['id']) this.userId = p['id'];
      this.getuserRoleList();
    });
  }

  ngOnInit(): void {
    this.dataService.get(ServerApis.getAllRols).subscribe((response) => {
      this.roleList = response.data ? response.data : [];
    });
  }

  getuserRoleList() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getAllUserRoles, {
        id: this.userId,
      })
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess && response.data) {
          this.userRoleList = response.data ? response.data : [];
        } else {
          var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      });
  }

  save() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
      return;
    }
    var form = this.form.value;
    this.isSaving = true;
    let dataToPost = {
      RoleId: form.roleId,
      UserId: this.userId,
    };
    this.dataService
      .post(ServerApis.addUserRole, dataToPost)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
          this.form.reset();
          this.getuserRoleList();
        } else {
          var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      });
  }

  delete(row: any) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا برای حذف اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        row.loading = true;

        this.dataService
          .get(ServerApis.deleteUserRole, { userId: row.userId, roleId: row.roleId })
          .subscribe(
            (response) => {
              row.loading = false;
              if (response.isSuccess) {
                this.toastrService.success('حذف اطلاعات با موفقیت انجام شد.');
                this.getuserRoleList();
              } else {
                let msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
                this.toastrService.error(msg);
              }
            },
            (error: any) => {
              row.loading = false;
            },
          );
      }
    });
  }
}
