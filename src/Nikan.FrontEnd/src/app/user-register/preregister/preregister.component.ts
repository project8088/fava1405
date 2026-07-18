import { ApiResult } from '@core/models/models';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';

import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { UserRegisterService } from '../userregister.service';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-preregister',
  templateUrl: './preregister.component.html',
  styleUrls: ['./preregister.component.scss'],
  standalone: false,
})
export class PreregisterComponent extends AppBase implements OnInit {
  form: FormGroup;
  loading?: boolean;
  isSaving: boolean = false;
  nationalityList: any[] = [];
  serviceId?: number;

  constructor(
    private accounService: UserRegisterService,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.form = this.fb.group({
      serviceId: [null, [Validators.required]],
      nationality: ['0', [Validators.required]],
      mobileNumber: [
        null,
        [Validators.required, Validators.pattern(/(^\(\d{3}\)\s\d{3}-\d{4}$)|(^\d{10})/)],
      ],
      nationCode: [null, [Validators.required, this.customValidator.checkNationalCode]],
      isAgree: [null, [Validators.required]],
    });

    this.form.get('nationality')?.valueChanges.subscribe((value) => {
      if (+value === 0) {
        this.form
          .get('nationCode')
          ?.setValidators([this.customValidator.checkNationalCode, Validators.required]);
      } else {
        this.form.get('nationCode')?.setValidators([Validators.required]);
      }
      this.form.get('nationCode')?.updateValueAndValidity();
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
  }
  submitForm(): void {
    if (this.form.valid) {
      this.isSaving = true;
      const form = this.form.getRawValue();
      this.dataService
        .post(ServerApis.checkCitzenRegister, {
          mobileNumber: form.mobileNumber,
          nationCode: form.nationCode,
          nationality: form.nationality,
          serviceId: form.serviceId,
        })
        .pipe(
          finalize(() => {
            this.isSaving = false;
            this.chdr.detectChanges();
          }),
        )
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
            }
          }
        );
    }
  }

  isIranian() {
    return +this.form.value.nationality === 0;
  }
}
