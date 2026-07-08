import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import Swal from 'sweetalert2';
import { BaseDataModel } from '@core/models/base-data-model';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-register-company',
  templateUrl: './register-company.component.html',
  styleUrls: ['./register-company.component.scss'],
  standalone: false,
})
export class RegisterCompanyComponent extends AppBase implements OnInit {
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
        companyName: [null, [Validators.required]],
        englishName: [null, [Validators.required]],
        companyRepresentative: [null],
        establishedYear: [null],
        mobileNumber: [null, [Validators.required]],
        txtTinNo: [null],
        txtRegNO: [null],
        userName: [null, [Validators.required]],
        password: [null, [Validators.required]],
        confirmPassword: [null, [Validators.required]],
        captchaCode: [null, [Validators.required]],
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
      (error:any) => {
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
    //if (!this.registerForm.get('captchaCode')?.value) {
    //  this.toastrService.warning('عبارت موجود در تصویر را وارد کنید.');
    //  //  return ;
    //}

    var formValue = this.registerForm.value;

    this.isSaving = true;
    this.dataService
      .post(ServerApis.companyRegister, {
        CompanyName: formValue.companyName,
        EnglishName: formValue.englishName,
        CompanyRepresentative: formValue.companyRepresentative,
        EstablishedYear: formValue.establishedYear,
        TxtTinNo: formValue.txtTinNo,
        TxtRegNO: formValue.txtRegNO,
        MobileNumber: formValue.mobileNumber,
        UserName: formValue.userName,
        Password: formValue.password,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response && response.isSuccess) {
            this.toastrService.success('ثبت نام شما با موفقیت انجام شد.');
            if (response.data.access_token && response.data.refresh_token) {
              this.authService.storeToken(response.data.access_token, response.data.refresh_token);

              this.authService.goToDashboard();
            } else {
              Swal.fire({
                title: 'ثبت نام شما با موفقیت انجام شد.',
                text: 'اکنون می توانید وارد حساب کاربری خود شوید.',
                confirmButtonText: 'ورود به حساب کاربری',
                showConfirmButton: true,
                showCancelButton: false,
                allowEnterKey: true,
                allowEscapeKey: false,
                allowOutsideClick: false,
              }).then((result) => {
                this.router.navigate(['/account/login']);
              });
            }
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error:any) => {
          this.isSaving = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
  }

  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.key === c2.key : c1 === c2;
  }
}
