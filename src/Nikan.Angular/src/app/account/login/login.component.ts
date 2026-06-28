import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AuthService } from 'src/app/core/authentication/auth.service';
import { DataService } from '../../core/services/data-service.service';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { CaptchaComponent } from '../bot-detect/captcha.component';
import { ServerApis } from '../../core/server-apis';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  loading: boolean = false;
  captchaImage: any;
  loadingCaptcha: boolean = true;
  returnUrl: string = '';
  serviceId:number=0;


  @ViewChild(CaptchaComponent, { static: true }) captchaComponent: CaptchaComponent;





  constructor(
    private fb: FormBuilder,
    private toastrService: ToastrService,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private dataService: DataService,
    private matDialog: MatDialog
  ) {

    this.route.queryParams.subscribe((p) => {
      if (p.returnUrl) this.returnUrl = p.returnUrl; 
    });

    authService.currentUser.subscribe(u => {
      if (u) {
        this.authService.goToDashboard();
      }
    });

    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
      rememberMe: [true, []],
      serviceId: [null],
       userEnteredCaptchaCode : [null, [Validators.required]],
       captchaId : ['']
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
    if (
      !this.loginForm.get('username').value ||
      !this.loginForm.get('password').value
    ) {
      this.loginForm.markAllAsTouched();
      this.toastrService.warning('نام کاربری و کلمه عبور خود را وارد کنید.');
      return false;
    }

    if (!this.loginForm.get('userEnteredCaptchaCode')?.value) {
      this.toastrService.warning('عبارت موجود در تصویر را وارد کنید.');
      return false;
    }


  
      var param: any = this.loginForm.value; 
    param.serviceId = this.serviceId;
    param.CaptchaId = this.captchaComponent.captchaId;
   
     

    this.loading = true;
    this.authService.login(this.loginForm.value).subscribe(
      (response) => {
        this.loading = false;
        if (response.isSuccess == true) {
          this.toastrService.success('احراز هویت با موفقیت انجام شد.');
        
          const user = this.authService.currentUserValue;
          if (user.isCitizen && this.serviceId) {
            this.router.navigateByUrl('/redirect?serviceId=' + this.serviceId +  '&returnUrl=' + this.returnUrl);
          } else {
            if (this.returnUrl) this.router.navigate([this.returnUrl]);
            else
             this.authService.goToDashboard();
          }
        } else {
          var msg = response.messages
            ? response.messages
            : 'نام کاربری یا کلمه عبور صحیح نیست!';
          this.toastrService.error(msg);
          this.captchaComponent.reloadImage();
        }
      },
      (error) => {
        if (error.status == '401')
          this.toastrService.error('نام کاربری یا کلمه عبور صحیح نیست!');
        else this.toastrService.error('خطا در ارتباط با سرور!');
        this.loading = false;
      }
    );
  }
}
