import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { ServerApis } from '../../../../core/server-apis';
import Swal from 'sweetalert2';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'company-activity-info',
  templateUrl: './activity-info.component.html',
  styleUrls: ['./activity-info.component.scss'],
  standalone: false,
})
export class CompanyActivityInfoComponent extends AppBase implements OnInit {
    loading?: boolean;
  companyActivityList: any[] = [];
  activityList: any[] = [];
  companyId: string = '';
  form: FormGroup;
  isSaving=false;

  constructor(private customValidators: CustomFormValidators) {
    super();
    this.form = this.fb.group({
      activityId: [null, [Validators.required]],
    });

    this.route.params.subscribe((p) => {
      if (p['id'] != '0' && p['id']) this.companyId = p['id'];
      this.getActivityInfo();
    });
  }

  ngOnInit(): void {
    this.dataService.get(ServerApis.getActivitiyList).subscribe((response) => {
      this.activityList = response.data ? response.data : [];
    });
  }

  getActivityInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getCompanyActivity, {
        companyId: this.companyId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.companyActivityList = response.data ? response.data : [];
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
        return ;
    }
    var form = this.form.value;
    this.isSaving = true;
    let dataToPost = {
      ActivityId: form.activityId,
      UserCompanyId: this.companyId,
    };
    this.dataService.post(ServerApis.addCompanyActivity, dataToPost).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
          this.form.reset();
          this.getActivityInfo();
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

  delete(row:any) {
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
        this.dataService.get(ServerApis.removeActivitiy, { id: row.id }).subscribe(
          (response) => {
            row.loading = false;
            if (response.isSuccess) {
              this.toastrService.success('حذف اطلاعات با موفقیت انجام شد.');
              this.getActivityInfo();
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
