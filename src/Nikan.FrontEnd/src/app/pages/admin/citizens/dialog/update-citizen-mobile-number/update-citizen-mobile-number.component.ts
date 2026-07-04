import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '../../../../../core/custom-validator/form-validation';
import { ServerApis } from '../../../../../core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-adm-update-citizen-mobile-number-dialog',
  templateUrl: './update-citizen-mobile-number.component.html',
  styleUrls: ['./update-citizen-mobile-number.component.scss'],
  standalone: false,
})
export class AdminUpdateCitizenMobileNumberDialogComponent extends AppBase implements OnInit {
  isSaving=false;
  userForm: FormGroup;
  userCode: string;
  loading: boolean = true;
  info: any;

  loadingData: boolean = true;
  constructor(
    private matDialogRef: MatDialogRef<AdminUpdateCitizenMobileNumberDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    if (_data) {
      this.userCode = _data.userCode;
      this.getUserInfo();
    }

    this.userForm = this.fb.group({
      mobileNumber: [null, [Validators.required, this.customValidator.checkMobileNumber]],
    });
  }

  ngOnInit() {}

  getUserInfo() {
    this.loading = true;
    //todo
    this.dataService
      .get(ServerApis.getShortCitizenInfoByAdmin, { userCode: this.userCode })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess) {
            this.info = response.data;
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
            this.matDialogRef.close(false);
          }
        },
        (error) => {
          this.loading = false;
          this.matDialogRef.close(false);
        },
      );
  }

  displayFn(item:any): string {
    return item && item.text ? item.text : '';
  }

  saveInfo() {
    if (this.userForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.userForm.markAllAsTouched();
        return ;
    }

    var formValue = this.userForm.value;

    this.isSaving = true;
    this.dataService
      .post(ServerApis.updteCitizenMobileNumberByAdmin, {
        userCode: this.userCode,
        MobileNumber: formValue.mobileNumber,
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
