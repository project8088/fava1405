import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'adm-manzalat-setting',
  templateUrl: './manzalat-setting.component.html',
  styleUrls: ['./manzalat-setting.component.scss'],
  standalone: false,
})
export class ManzalatSettingComponent extends AppBase implements OnInit {
  settingForm: FormGroup;
  isSaving = false;
  loading?: boolean;

  constructor() {
    super();
    this.settingForm = this.fb.group({
      minAgeSalmand: [0],
      minAgeBazneshasteh: [0],
      janbazanDescription: [''],
      bazneshastehDescription: [''],
      zanSarparastDescription: [''],
      salmandDescription: [''],
      maloulinDescription: [''],
    });
  }

  ngOnInit(): void {
    this.loading = true;
    this.dataService
      .get(ServerApis.getManzalatSettings)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess && response.data) {
          this.settingForm.patchValue(response.data);
        } else {
          let msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      });
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
      .post(ServerApis.updateManzalatSettings, params)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
        } else {
          let msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      });
  }
}
