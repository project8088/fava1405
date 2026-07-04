import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '../../../core/server-apis';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-sms-setting',
  templateUrl: './sms-setting.component.html',
  styleUrls: ['./sms-setting.component.scss'],
})
export class SmsSettingComponent extends AppBase implements OnInit {
  settingForm: FormGroup;
  isSaving: boolean;
  loading: boolean;

  constructor(
) {
      super();
    this.settingForm = this.fb.group({
      smsUserName: [null],
      smsPassword: [null],
      domainName: [null],
      senderNumber: [null],
      countValidMobileNumber: [null],
      smsToken: [null],

      sendSmsAfterAdminLogin: [false],
      sendSmsAfterRejectCitizenInformationInUpdateForm: [false],
    });
  }

  ngOnInit(): void {
    this.loading = true;
    this.dataService.get(ServerApis.geSmsSettings).subscribe(
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
    this.dataService.post(ServerApis.updateSmsSettings, params).subscribe(
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
