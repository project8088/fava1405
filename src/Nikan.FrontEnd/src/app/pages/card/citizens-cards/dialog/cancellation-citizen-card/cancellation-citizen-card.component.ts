import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'ard-cancellation-citizen-card-dialog',
  templateUrl: './cancellation-citizen-card.component.html',
  styleUrls: ['./cancellation-citizen-card.component.scss'],
  standalone: false,
})
export class CardCancellationCitizenCardDialogComponent extends AppBase implements OnInit {
  isSaving = false;
  frm: FormGroup;
  centerList: any[] = [];
  baseInfo: any = {};
  loading: boolean = true;
  info: any;
  constructor(
    private matDialogRef: MatDialogRef<CardCancellationCitizenCardDialogComponent>,
    @Inject(MAT_DIALOG_DATA) _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.info = _data.info;

    this.frm = this.fb.group({
      cardCancellationItemId: ['', [Validators.required]],
      description: [null, [Validators.required]],
      cardCancellationOnDate: [null, [Validators.required]],
      sendSms: [false],
    });
    this.frm.patchValue(_data.info);
  }

  ngOnInit() {
    this.dataService.getEnums().subscribe((response) => {
      this.baseInfo = {
        cardCancellationItem: response.cardCancellationItem,
      };
    });
  }

  saveInfo() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    var url = ServerApis.cardCancellation;
    var params = this.frm.value;
    params.id = this.info.id;
    params.cardCancellationItemId = params.cardCancellationItemId;
    params.description = params.description;
    params.cardCancellationOnDate = params.cardCancellationOnDate;
    params.sendSms = params.sendSms;

    this.dataService.post(url, params)
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
