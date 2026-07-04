import { ActivatedRoute, Router } from '@angular/router';
import { ApiResult, RegisterServiceModel } from '../../core/models/models';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { DataService } from '@core/services/data-service.service';
import { ServerApis } from '../../core/server-apis';
import { ToastrService } from 'ngx-toastr';
import { UserRegisterService } from '../userregister.service';
import { CaptchaComponent } from 'src/app/account/bot-detect/captcha.component';

@Component({
  selector: 'app-preregister',
  templateUrl: './preregister.component.html',
  styleUrls: ['./preregister.component.scss'],
})
export class PreregisterComponent implements OnInit {
  form: FormGroup;
  loading: boolean;
  isSaving: boolean = false;
  nationalityList;
  serviceId: number;
  @ViewChild(CaptchaComponent, { static: true }) captchaComponent: CaptchaComponent;

  constructor(
    private dataService: DataService,
    private accounService: UserRegisterService,
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private customValidator: CustomFormValidators,
  ) {
    this.form = this.fb.group({
      serviceId: [null, [Validators.required]],
      nationality: ['0', [Validators.required]],
      mobileNumber: [
        null,
        [Validators.required, Validators.pattern(/(^\(\d{3}\)\s\d{3}-\d{4}$)|(^\d{10})/)],
      ],
      nationCode: [null, [Validators.required, this.customValidator.checkNationalCode]],
      isAgree: [null, [Validators.required]],
      userEnteredCaptchaCode: [null, [Validators.required]],
      captchaId: [''],
    });

    this.form.get('nationality').valueChanges.subscribe((value) => {
      if (+value === 0) {
        this.form
          .get('nationCode')
          .setValidators([this.customValidator.checkNationalCode, Validators.required]);
      } else {
        this.form.get('nationCode').setValidators([Validators.required]);
      }
      this.form.get('nationCode').updateValueAndValidity();
    });

    this.route.queryParams.subscribe((params) => {
      if (params['serviceId']) {
        this.serviceId = params['serviceId'];
        this.form.patchValue({ serviceId: params['serviceId'] });
      } else this.router.navigate(['/']);
    });

    this.dataService.get(ServerApis.getNationalities).subscribe((data) => {
      this.nationalityList = data.data;
    });
  }

  ngOnInit(): void {
    this.captchaComponent.captchaEndpoint = ServerApis.baseUrl + '/simple-captcha-endpoint.ashx';
  }
  submitForm(): void {
    if (this.form.valid) {
      this.isSaving = true;
      const form = this.form.getRawValue();
      this.dataService
        .post(ServerApis.checkCitzenRegister, {
          captchaCode: form.captchaCode,
          mobileNumber: form.mobileNumber,
          nationCode: form.nationCode,
          nationality: form.nationality,
          userEnteredCaptchaCode: form.userEnteredCaptchaCode,
          CaptchaId: this.captchaComponent.captchaId,
          serviceId: form.serviceId,
        })
        .subscribe(
          (data: ApiResult<any>) => {
            if (data.isSuccess) {
              this.accounService.setUserPreRegisterData({
                mobileNumber: form.mobileNumber,
                nationCode: form.nationCode,
                nationality: form.nationality,
              });
              this.toastrService.success(data.messages);
              this.toastrService.success('در حال انتقال به مراحل ثبت نام شهروند...');

              this.router.navigate(['/userregister/register'], {
                queryParams: { serviceId: this.form.value.serviceId },
              });
            } else {
              this.toastrService.error(data.messages);
              this.isSaving = false;
              this.captchaComponent.reloadImage();
            }
          },
          (error) => {
            this.isSaving = false;
            this.captchaComponent.reloadImage();
            this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
          },
        );
    }
  }

  isIranian() {
    return +this.form.value.nationality === 0;
  }
}
