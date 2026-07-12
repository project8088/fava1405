import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'card-back-citizen-card-dialog',
  templateUrl: './back-citizen-card.component.html',
  styleUrls: ['./back-citizen-card.component.scss'],
  standalone: false,
})
export class CardBackCitizenCardDialogComponent extends AppBase implements OnInit {
  isSaving = false;
  frm: FormGroup;
  centerList: any[] = [];
  loading: boolean = true;
  info: any;
  constructor(
    private matDialogRef: MatDialogRef<CardBackCitizenCardDialogComponent>,
    @Inject(MAT_DIALOG_DATA) _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.info = _data.info;

    this.frm = this.fb.group({
      centerId: ['', [Validators.required]],
      reason: [null, [Validators.required]],
      backCardOnDate: [null, [Validators.required]],
      sendSms: [false],
    });
    this.frm.patchValue(_data.info);
  }

  ngOnInit() {
    this.getcenterList();
  }

  getcenterList() {
    this.dataService.get(ServerApis.getAllCardDeliveryCenters).subscribe((response) => {
      if (response.isSuccess) this.centerList = response.data ? response.data : [];
      else {
        let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
        this.toastrService.error(msg);
      }
    });
  }

  saveInfo() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    var url = ServerApis.backCard;
    var params = this.frm.value;
    params.id = this.info.id;
    params.centerId = params.centerId;
    params.reason = params.reason;
    params.backCardOnDate = params.backCardOnDate;
    params.sendSms = params.sendSms;

    this.dataService
      .post(url, params)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response && response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
            this.matDialogRef.close(true);
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
  }
}
