import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'company-add-user-dialog',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss'],
  standalone: false,
})
export class CompanyAddUserDialogComponent extends AppBase implements OnInit {
  isSaving=false;
  userForm: FormGroup;
  loading: boolean = true;
  companyId: string;
  constructor(
    private matDialogRef: MatDialogRef<CompanyAddUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.companyId = _data.companyId;

    this.userForm = this.fb.group(
      {
        id: [null],
        firstName: [null, [Validators.required, this.customValidator.checkPersianCharacters]],
        lastName: [null, [Validators.required, this.customValidator.checkPersianCharacters]],
        username: [
          null,
          [Validators.required, this.customValidator.checkEnglishAndNumberCharacters],
        ],
        password: [null, [Validators.required]],
        confirmPassword: [null, [Validators.required]],

        mobile: [null, [Validators.required, this.customValidator.checkMobileNumber]],
        email: [null, [Validators.required, this.customValidator.checkEmail]],
      },
      { validator: this.checkPasswords },
    );
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
    if (this.userForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.userForm.markAllAsTouched();
        return ;
    }
    var formValue = this.userForm.value;

    this.isSaving = true;

    this.dataService
      .post(ServerApis.addCompanyUser, {
        CompanyId: this.companyId && this.companyId != '0' ? +this.companyId : null,
        DisplayName: formValue.firstName + ' ' + formValue.lastName,
        Email: formValue.email,
        MobileNumber: formValue.mobile,
        UserName: formValue.username,
        Password: formValue.password,
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
