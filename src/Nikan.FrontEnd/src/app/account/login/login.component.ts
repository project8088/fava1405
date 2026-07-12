import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CaptchaComponent } from '../bot-detect/captcha.component';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: false,
})
export class LoginComponent extends AppBase implements OnInit {
  loginForm: FormGroup;
  loading: boolean = false;
  captchaImage: any;
  loadingCaptcha: boolean = true;
  returnUrl: string = '';
  serviceId: number = 0;

  @ViewChild(CaptchaComponent, { static: true }) captchaComponent!: CaptchaComponent;

  constructor() {
    super();
    this.route.queryParams.subscribe((p) => {
      this.returnUrl = p['returnUrl'] ?? '';
    });

    this.authService.currentUser.subscribe((u) => {
      if (u) {
        this.authService.goToDashboard();
      }
    });

    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
      rememberMe: [true, []],
      serviceId: [null],
      userEnteredCaptchaCode: [null, [Validators.required]],
      captchaId: [''],
    });
  }

  ngOnInit(): void {
    this.captchaComponent.captchaEndpoint = ServerApis.baseUrl + '/simple-captcha-endpoint.ashx';

    this.matDialog.closeAll();

    this.route.queryParams.subscribe((params) => {
      if (params['serviceId']) {
        this.serviceId = params['serviceId'];
      }
    });
  }

  login() {
    if (!this.loginForm.get('username')?.value || !this.loginForm.get('password')?.value) {
      this.loginForm.markAllAsTouched();
      this.toastrService.warning('نام کاربری و کلمه عبور خود را وارد کنید.');
      return;
    }

    if (!this.loginForm.get('userEnteredCaptchaCode')?.value) {
      this.toastrService.warning('عبارت موجود در تصویر را وارد کنید.');
      return;
    }

    var param: any = this.loginForm.value;
    param.serviceId = this.serviceId;
    param.CaptchaId = this.captchaComponent.captchaId;

    this.loading = true;
    this.authService
      .login(this.loginForm.value)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe({
        next: (response) => {
          if (response.isSuccess == true) {
            this.toastrService.success('احراز هویت با موفقیت انجام شد.');

            const user = this.authService.currentUserValue;
            if (user && user.isCitizen && this.serviceId) {
              this.router.navigateByUrl(
                '/redirect?serviceId=' + this.serviceId + '&returnUrl=' + this.returnUrl,
              );
            } else {
              if (this.returnUrl) this.router.navigate([this.returnUrl]);
              else this.authService.goToDashboard();
            }
          } else {
            var msg = response.messages ? response.messages : 'نام کاربری یا کلمه عبور صحیح نیست!';
            this.toastrService.error(msg);
            this.captchaComponent.reloadImage();
          }
        },
        error: (error) => {
          if (error.status == '401') this.toastrService.error('نام کاربری یا کلمه عبور صحیح نیست!');
          else this.toastrService.error('خطا در ارتباط با سرور!');
        },
      });
  }
}
