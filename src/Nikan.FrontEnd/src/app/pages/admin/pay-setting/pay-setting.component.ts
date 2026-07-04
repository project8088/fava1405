import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '../../../core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-pay-setting',
  templateUrl: './pay-setting.component.html',
  styleUrls: ['./pay-setting.component.scss'],
  standalone: false,
})
export class AdminPaySettingComponent extends AppBase implements OnInit {
  settingForm: FormGroup;
  isSaving: boolean;
  loading: boolean;

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
    this.dataService.get(ServerApis.getFinancialSettings).subscribe(
      (response) => {
        this.loading = false;
        if (response.isSuccess && response.data) {
          this.settingForm.patchValue(response.data);
        } else {
          let msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loading = false;
      },
    );
  }

  saveSetting() {
    if (this.settingForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.settingForm.markAllAsTouched();
      return false;
    }

    this.isSaving = true;
    var params = this.settingForm.value;
    this.dataService.post(ServerApis.updateFinancialSettings, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
        } else {
          let msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.isSaving = false;
      },
    );
  }
}
