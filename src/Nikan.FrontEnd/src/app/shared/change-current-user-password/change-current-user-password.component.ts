import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { FormGroup, Validators } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'change-current-user-password',
  templateUrl: './change-current-user-password.component.html',
  styleUrls: ['./change-current-user-password.component.scss'],
  standalone: false,
})
export class ChangeCurrentUserPasswordComponent extends AppBase implements OnInit {
  isSaving=false;
  changePasswordForm: FormGroup;

  constructor(private customValidator: CustomFormValidators) {
    super();
    this.changePasswordForm = this.fb.group(
      {
        username: [null],
        oldPassword: [null, [Validators.required]],
        password: [null, [Validators.required]],
        confirmPassword: [null, [Validators.required]],
      },
      { validator: this.checkPasswords },
    );

    this.changePasswordForm.get('username')?.setValue(this.authService.getAuthUser()?.userName);
  }
  /**
   * بررسی یکی بودن کلمه عبور و تائید آن
   */
  checkPasswords(group: FormGroup) {
    let pass = group.controls['password']?.value;
    let confirmPassword = group.controls['confirmPassword']?.value;

    return pass === confirmPassword ? null : { notSame: true };
  }

  ngOnInit(): void {}

  saveInfo() {
    if (this.changePasswordForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.changePasswordForm.markAllAsTouched();
        return ;
    }

    var formValue = this.changePasswordForm.value;

    this.isSaving = true;
    this.dataService
      .post(ServerApis.changeCurrentUserPassword, {
        oldPassword: formValue.oldPassword,
        NewPassword: formValue.password,
        ConfirmPassword: formValue.confirmPassword,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response && response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
            this.authService.logout(false);
            this.router.navigate(['/account/login']);
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
