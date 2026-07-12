import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'app-adm-update-citizen-sabt-state-by-card-dialog',
  templateUrl: './update-citizen-sabt-state-by-card.component.html',
  styleUrls: ['./update-citizen-sabt-state-by-card.component.scss'],
  standalone: false,
})
export class CardUpdateCitizenSabtStateByCardDialogComponent extends AppBase implements OnInit {
  isSaving = false;
  userForm: FormGroup;
  userCode: string = '';
  loading: boolean = true;
  info: any;

  loadingData: boolean = true;
  constructor(
    private matDialogRef: MatDialogRef<CardUpdateCitizenSabtStateByCardDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    if (_data) {
      this.userCode = _data.userCode;
      this.getUserInfo();
    }

    this.userForm = this.fb.group({
      sabtStatus: [null, [Validators.required]],
    });
  }

  ngOnInit() {}

  getUserInfo() {
    this.loading = true;
    //todo
    this.dataService
            .get(ServerApis.getShortCitizenInfoByCard, { userCode: this.userCode })
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
                if (response.isSuccess) {
                  this.info = response.data;
                } else {
                  let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است.';
                  this.toastrService.error(msg);
                  this.matDialogRef.close(false);
                }
              }, (error: any) => {
                this.matDialogRef.close(false);
              });
  }

  displayFn(item: any): string {
    return item && item.text ? item.text : '';
  }

  saveInfo() {
    if (this.userForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.userForm.markAllAsTouched();
      return;
    }

    var formValue = this.userForm.value;

    this.isSaving = true;
    this.dataService
            .post(ServerApis.updateSabtStatusByCard, {
              userCode: this.userCode,
              sabtStatus: formValue.sabtStatus,
            })
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
                if (response && response.isSuccess) {
                  this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
                  this.matDialogRef.close(true);
                } else {
                  let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                  this.toastrService.error(msg);
                }
              }, (error: any) => {
              });
  }
}
