import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CaptchaComponent } from '../bot-detect/captcha.component';
import { ServerApis } from '@core/server-apis';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss'],
  standalone: false,
})
export class ForgotPasswordComponent extends AppBase implements OnInit {
  userId: string = '';
  serviceId: number = 0;
  captchaImage: any;
  loadingCaptcha: boolean = true;

  forgotForm: FormGroup;
  showChangePassword: boolean = false;
  showConfirmCode: boolean = false;

  sendingSMS: boolean = false;
  timerCounter: number = 120;
  lastTimerCounter: number = 120;
  timerCounterString = '';
  resendTimerInterval: any;

  checkingCode: boolean = false;
  isSaving: boolean = false;

  @ViewChild(CaptchaComponent, { static: true }) captchaComponent!: CaptchaComponent;

  constructor(private customValidator: CustomFormValidators) {
    super();
    this.forgotForm = this.fb.group(
      {
        userEnteredCaptchaCode: [null, [Validators.required]],
        captchaId: [''],
        userName: [null, [Validators.required]],
        mobile: [null, [Validators.required, this.customValidator.checkMobileNumber]],
        verificationCode: [null, [Validators.required]],
        password: [null, [Validators.required, Validators.minLength(6)]],
        confirmPassword: [null, [Validators.required, Validators.minLength(6)]],
      },
      { validator: this.checkPasswords },
    );

    this.route.queryParams.subscribe((params) => {
      if (params['serviceId']) {
        this.serviceId = params['serviceId'];
      }
    });
  }

  ngOnInit(): void {
    //this.captchaComponent.captchaEndpoint = ServerApis.baseUrl + '/simple-captcha-endpoint.ashx';
  }

  /**
   * بررسی یکی بودن کلمه عبور و تائید آن
   */
  checkPasswords(group: FormGroup) {
    let pass = group.controls['password']?.value;
    let confirmPassword = group.controls['confirmPassword']?.value;

    return pass === confirmPassword ? null : { notSame: true };
  }

  /**
   *  ارسال کد تائید شماره موبایل
   *
   */
  sendConfirmCode() {
    if (this.forgotForm.get('userName')?.invalid) {
      this.toastrService.warning('نام کاربری خود را وارد کنید.');
      this.forgotForm.get('userName')?.markAsTouched();
      return;
    }

    //if (!this.forgotForm.get('userEnteredCaptchaCode')?.value) {
    //  this.toastrService.warning('عبارت موجود در تصویر را وارد کنید.');
    //    return ;
    // }

    if (this.forgotForm.get('mobile')?.invalid) {
      this.toastrService.warning('شماره موبایل خود را وارد کنید.');
      this.forgotForm.get('mobile')?.markAsTouched();
      return;
    }

    this.sendingSMS = true;

    this.dataService
      .post(ServerApis.sendSmsForgotPassword, {
        UserName: this.forgotForm.get('userName')?.value,
        MobileNumber: this.forgotForm.get('mobile')?.value,
        //CaptchaId : this.captchaComponent.captchaId
      })
      .pipe(
        finalize(() => {
          this.sendingSMS = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        this.sendingSMS = false;
        if (response.isSuccess) {
          this.userId = response.data.userId;
          this.toastrService.success('کد تائید شماره موبایل با موفقیت ارسال شد.');
          this.lastTimerCounter = this.lastTimerCounter + 60;
          this.timerCounter = this.lastTimerCounter;
          this.startTimer();

          this.showConfirmCode = true;
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      });
  }

  /**
   * تایمر برای ارسال مجدد کد تائید
   * */
  startTimer() {
    this.resendTimerInterval = setInterval(() => {
      this.timerCounter--;
      this.timerCounterString = this.convertSecondstoTime(this.timerCounter);
      if (this.timerCounter <= 0) {
        clearInterval(this.resendTimerInterval);
        this.timerCounter = 0;
      }
    }, 1000);
  }

  /**
   * convert 300s to 5:00
   * @param {any} given_seconds
   */
  convertSecondstoTime(given_seconds: number) {
    var dateObj = new Date(given_seconds * 1000);
    var hours = dateObj.getUTCHours();
    var minutes = dateObj.getUTCMinutes();
    var seconds = dateObj.getSeconds();

    var timeString =
      hours.toString().padStart(2, '0') +
      ':' +
      minutes.toString().padStart(2, '0') +
      ':' +
      seconds.toString().padStart(2, '0');

    return timeString;
  }

  /**
   * بررسی صحیح بودن کد تائید
   *
   */
  checkVerificationCode() {
    if (this.forgotForm.get('verificationCode')?.invalid) {
      this.toastrService.warning('کد تائیدی که برای شما پیامک شده است را وارد کنید.');
      this.forgotForm.get('verificationCode')?.markAsTouched();
      return;
    }

    this.checkingCode = true;
    this.dataService
      .post(ServerApis.checkForgotVerifyCode, {
        VerifyCode: this.forgotForm.get('verificationCode')?.value,
        MobileNumber: this.forgotForm.get('mobile')?.value,
        UserId: this.userId,
      })
      .pipe(
        finalize(() => {
          this.checkingCode = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        this.checkingCode = false;
        if (response.isSuccess) {
          if (response.data.userId) {
            this.toastrService.success('هم اکنون کلمه عبور جدید خود را وارد نمایید.');
            this.showConfirmCode = false;
            this.showChangePassword = true;
          } else {
            this.toastrService.error('کد تائید صحیح نیست!');
          }
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      });
  }

  /**
   *  ذخیره پسورد
   *
   */
  saveNewPassword() {
    if (
      this.forgotForm.get('password')?.invalid ||
      this.forgotForm.get('confirmPassword')?.invalid
    ) {
      this.toastrService.warning('کلمه عبور و تکرار کلمه عبور خود را وارد کنید .');
      this.forgotForm.markAllAsTouched();
      return;
    }
    this.isSaving = true;
    this.dataService
      .post(ServerApis.setNewPassword, {
        VerifyCode: this.forgotForm.get('verificationCode')?.value,
        MobileNumber: this.forgotForm.get('mobile')?.value,
        Password: this.forgotForm.get('password')?.value,
        userId: this.userId,
      })
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('کلمه عبور شما با موفقیت تغییر یافت.');

          if (this.serviceId) {
            this.router.navigate(['/account/login'], {
              queryParams: { serviceId: this.serviceId },
            });
          } else {
            this.router.navigate(['/account/login']);
          }
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      });
  }
}
