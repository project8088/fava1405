import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-add-refund-user-dialog',
  templateUrl: './add-refund-user.component.html',
  styleUrls: ['./add-refund-user.component.scss'],
  standalone: false,
})
export class AdminAddRefundUserDialogComponent extends AppBase implements OnInit {
  isSaving=false;
  userForm: FormGroup;
  loading: boolean = true;

  organizationList: any = ([] = []);
  unitList: any = ([] = []);
  periorityList: any[] = [];
  loadingUnit: boolean;
  loadingData: boolean = true;
  constructor(
    private matDialogRef: MatDialogRef<AdminAddRefundUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.userForm = this.fb.group({
      nationCode: ['', [Validators.required]],
    });
  }

  ngOnInit() {}

  saveInfo() {
    if (this.userForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.userForm.markAllAsTouched();
        return ;
    }
    var formValue = this.userForm.value;

    this.isSaving = true;

    this.dataService
      .post(ServerApis.addRefundUser, {
        NationCode: formValue.nationCode,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response && response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
            this.matDialogRef.close(true);
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
