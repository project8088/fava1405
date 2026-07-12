import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'adm-pay-setting',
  templateUrl: './pay-setting.component.html',
  styleUrls: ['./pay-setting.component.scss'],
  standalone: false,
})
export class AdminPaySettingComponent extends AppBase implements OnInit {
  settingForm: FormGroup;
  isSaving = false;
  loading?: boolean;

  constructor() {
    super();
    this.settingForm = this.fb.group({
      bankCustomerId: [null, [Validators.required]],
      bankPassword: [null, [Validators.required]],
      bankUserName: [null, [Validators.required]],
      bankTerminalId: [null, [Validators.required]],
      refundTerminalId: [0],
      refundUserName: [''],
      refundPassword: [''],
      callBackUrl: [''],
      refundIsActive: [false],
      refundDeActiveDescription: [''],
      refundAmountDeficit: [''],
      bankPaymentMethod: [null, [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.loading = true;
    this.dataService
      .get(ServerApis.getFinancialSettings)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response.isSuccess && response.data) {
            this.settingForm.patchValue(response.data);
          } else {
            let msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
  }

  saveSetting() {
    if (this.settingForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.settingForm.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    var params = this.settingForm.value;
    this.dataService
      .post(ServerApis.updateFinancialSettings, params)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
          } else {
            let msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
  }
}
