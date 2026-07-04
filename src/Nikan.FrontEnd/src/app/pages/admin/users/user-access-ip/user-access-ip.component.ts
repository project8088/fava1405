import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { ServerApis } from '../../../../core/server-apis';
import { RequireMatch } from '../../../../core/custom-validator/requireMatch';
import Swal from 'sweetalert2';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'user-access-ip',
  templateUrl: './user-access-ip.component.html',
  styleUrls: ['./user-access-ip.component.scss'],
    standalone: false
})
export class AdminUserAccessIpComponent extends AppBase implements OnInit {
  loading: boolean;
  userAppAccessList: any[] = [];
  appList: any[] = [];
  userId: string = '';
  form: FormGroup;
  isSaving: boolean;

  constructor(
    private customValidators: CustomFormValidators
  ) {
      super();
    this.form = this.fb.group({
      start: [null, [Validators.required]],
      end: [''],
    });

    this.route.params.subscribe((p) => {
      if (p.id != '0' && p.id) this.userId = p.id;
      this.getWebUserAccessRangeIp();
    });
  }

  ngOnInit(): void {}

  getWebUserAccessRangeIp() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getWebUserAccessRangeIp, {
        userId: this.userId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.userAppAccessList = response.data ? response.data : [];
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loading = false;
        },
      );
  }

  save() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
      return false;
    }
    var form = this.form.value;
    this.isSaving = true;
    let dataToPost = {
      start: form.start,
      end: form.end,
      UserId: this.userId,
    };
    this.dataService.post(ServerApis.addWebUserAccessRangeIp, dataToPost).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
          this.form.reset();
          this.getWebUserAccessRangeIp();
        } else {
          var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.isSaving = false;
      },
    );
  }

  delete(row) {
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
          .get(ServerApis.deleteUserAccessRangeIp, { userId: row.userId, ipId: row.id })
          .subscribe(
            (response) => {
              row.loading = false;
              if (response.isSuccess) {
                this.toastrService.success('حذف اطلاعات با موفقیت انجام شد.');
                this.getWebUserAccessRangeIp();
              } else {
                let msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
                this.toastrService.error(msg);
              }
            },
            (error) => {
              row.loading = false;
            },
          );
      }
    });
  }
}
