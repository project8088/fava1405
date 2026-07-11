import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'company-change-user-password-dialog',
  templateUrl: './change-user-password.component.html',
  styleUrls: ['./change-user-password.component.scss'],
  standalone: false,
})
export class CompanyChangePasswordDialogComponent extends AppBase implements OnInit {
  isSaving = false;
  changePasswordForm: FormGroup;
  userId?: string;

  loading: boolean = true;

  companyId: string = '';

  constructor(
    private matDialogRef: MatDialogRef<CompanyChangePasswordDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.changePasswordForm = this.fb.group(
      {
        userId: [null],
        displayName: [null],
        username: [null],
        password: [null, [Validators.required]],
        confirmPassword: [null, [Validators.required]],
      },
      { validator: this.checkPasswords },
    );

    this.companyId = _data.companyId;

    if (_data.userId) {
      this.userId = _data.userId;
      this.changePasswordForm.setValue({
        userId: this.userId,
        username: _data.userName,
        displayName: _data.displayName,
        password: '',
        confirmPassword: '',
      });
    }
  }

  /**
   * بررسی یکی بودن کلمه عبور و تائید آن
   */
  checkPasswords(group: FormGroup) {
    let pass = group.controls['password']?.value;
    let confirmPassword = group.controls['confirmPassword']?.value;

    return pass === confirmPassword ? null : { notSame: true };
  }

  ngOnInit() {}

  saveInfo() {
    if (this.changePasswordForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.changePasswordForm.markAllAsTouched();
      return;
    }

    var formValue = this.changePasswordForm.value;

    this.isSaving = true;
    this.dataService
      .post(ServerApis.changeUserPassword, {
        CompanyId: this.companyId && this.companyId != '0' ? +this.companyId : null,
        UserId: this.userId,
        NewPassword: formValue.password,
        ConfirmPassword: formValue.confirmPassword,
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
        (error: any) => {
          this.isSaving = false;
        },
      );
  }
}
