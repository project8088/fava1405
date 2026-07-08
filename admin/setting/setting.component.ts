import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-setting',
  templateUrl: './setting.component.html',
  styleUrls: ['./setting.component.scss'],
  standalone: false,
})
export class AdminSettingComponent extends AppBase implements OnInit {
  settingForm: FormGroup;
  isSaving=false;
    loading?: boolean;

  baseUrl: string = ServerApis.baseUrl;
  logoUrl: string='';
  uploadUrl: string = ServerApis.uploadSiteLogo;

  constructor() {
    super();
    this.settingForm = this.fb.group({
      siteUrl: [null, [Validators.required]],
      siteName: [null, [Validators.required]],
      fullSiteName: [null, [Validators.required]],
      siteKeywords: [null, [Validators.required]],
      siteDescription: [null, [Validators.required]],

      mailServerUrl: [null],
      mailServerPort: [null],
      mailServerUserName: [null],
      mailServerPassword: [null],

      cellNumber: [null, []],
      mobileNumber: [null, []],
      addresss: [null, []],
      telegramChannelId: [null, []],

      smsNumber: [null, []],
      faxNumber: [null, []],
      emailAddress: [null, []],
      footerText: [null, []],
      businessHours: [null, []],

      isfahanProvinceId: [null, []],
      isfahanCityId: [null, []],
      regionCount: [null, []],
      manzalatGroupId: [null, []],
      onlineAuthenticationAfterUpdateCitizenInfo: [false, []],
      onlineAuthentication: [false, []],
    });
  }

  ngOnInit(): void {
    this.loading = true;
    this.dataService.get(ServerApis.getSettings).subscribe(
      (response) => {
        this.loading = false;
        if (response.isSuccess && response.data) {
          this.settingForm.patchValue(response.data);
        } else {
          let msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      },
      (error:any) => {
        this.loading = false;
      },
    );
  }

  saveSetting() {
    if (this.settingForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.settingForm.markAllAsTouched();
        return ;
    }

    this.isSaving = true;
    var params = this.settingForm.value;
    params.logoUrl = this.logoUrl;
    this.dataService.post(ServerApis.updateSettings, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
        } else {
          let msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      },
      (error:any) => {
        this.isSaving = false;
      },
    );
  }
}
