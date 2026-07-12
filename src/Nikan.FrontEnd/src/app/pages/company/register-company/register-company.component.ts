import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { Observable, finalize } from 'rxjs';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { BaseDataModel } from '@core/models/base-data-model';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'admin-register-company',
  templateUrl: './register-company.component.html',
  styleUrls: ['./register-company.component.scss'],
  standalone: false,
})
export class AdminRegisterCompanyComponent extends AppBase implements OnInit {
  isSaving = false;
  registerForm: FormGroup;
  id: string = '';
  loading: boolean = true;

  captchaImage: any;
  loadingCaptcha: boolean = true;

  loadingState: boolean = false;
  stateList: BaseDataModel[] = [];
  filteredState = new Observable<any[]>();

  constructor(private customValidator: CustomFormValidators) {
    super();
    this.registerForm = this.fb.group(
      {
        companyName: [null, [Validators.required, Validators.maxLength(500)]],
        englishName: [
          null,
          [
            Validators.required,
            Validators.maxLength(40),
            this.customValidator.checkEnglishCharacters,
          ],
        ],
        companyRepresentative: [
          null,
          [
            Validators.required,
            Validators.maxLength(100),
            this.customValidator.checkPersianCharacters,
          ],
        ],
        establishedYear: [
          null,
          [Validators.required, Validators.maxLength(4), Validators.minLength(4)],
        ],
        mobileNumber: [null, [Validators.required, this.customValidator.checkMobileNumber]],
        email: [null, [Validators.required, this.customValidator.checkEmail]],
        txtTinNo: [null, [Validators.required]],
        txtRegNO: [null, [Validators.required]],
        userName: [null, [Validators.required, this.customValidator.checkEnglishCharacters]],
        password: [null, [Validators.required]],
        confirmPassword: [null, [Validators.required]],
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

  ngOnInit(): void {
    this.getCaptcha();
  }

  getCaptcha() {
    this.loadingCaptcha = true;

    this.dataService.getCaptchaImage({}).subscribe(
      (response) => {
        this.loadingCaptcha = false;
        let reader = new FileReader();
        let photo = new File([response], 'captcha.png', { type: 'image/png' });
        reader.readAsDataURL(photo);
        reader.onload = (event: any) => {
          this.captchaImage = event.target.result;
        };
      },
      (error: any) => {
        this.loadingCaptcha = false;
      },
    );
  }

  displayFn(item: any): string {
    return item && item.text ? item.text : '';
  }

  saveInfo() {
    if (this.registerForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.registerForm.markAllAsTouched();
      return;
    }

    var formValue = this.registerForm.value;

    this.isSaving = true;
    this.dataService
      .post(ServerApis.companyRegisterAsync, {
        CompanyName: formValue.companyName,
        EnglishName: formValue.englishName,
        CompanyRepresentative: formValue.companyRepresentative,
        EstablishedYear: formValue.establishedYear,
        TxtTinNo: formValue.txtTinNo,
        TxtRegNO: formValue.txtRegNO,
        MobileNumber: formValue.mobileNumber,
        Email: formValue.email,
        UserName: formValue.userName,
        Password: formValue.password,
      })
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response && response.isSuccess) {
            this.toastrService.success('ثبت نام شما با موفقیت انجام شد.');
            this.router.navigate(['/admin/companies']);
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
  }

  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.key === c2.key : c1 === c2;
  }
}
