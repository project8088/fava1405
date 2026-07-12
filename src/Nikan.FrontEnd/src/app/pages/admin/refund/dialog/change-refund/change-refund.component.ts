import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'adm-change-refund-dialog',
  templateUrl: './change-refund.component.html',
  styleUrls: ['./change-refund.component.scss'],
  standalone: false,
})
export class AdminChangeRefundDialogComponent extends AppBase implements OnInit {
  isSaving = false;
  frm: FormGroup;
  loading: boolean = true;
  info: any;
  constructor(
    private matDialogRef: MatDialogRef<AdminChangeRefundDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.info = _data.info;

    this.frm = this.fb.group({
      refundId: [null],
      orderId: [null, [Validators.required]],
      transactionCode: [null, [Validators.required]],
      totalRefundAmount: [null],
      refundAmount: [null],
      refundCardNumber: [''],
      description: [''],
      otherDescription: [''],
      adminDescription: [''],
      isClosed: [false],
    });
    this.frm.patchValue(_data.info);
  }

  ngOnInit() {}

  saveInfo() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    var url = ServerApis.updateRefund;
    var params = this.frm.value;
    params.refundId = this.info.refundId;
    params.orderId = params.orderId;
    params.transactionCode = params.transactionCode;
    params.totalRefundAmount = +params.totalRefundAmount;
    params.refundAmount = +params.refundAmount;
    params.description = params.description;

    params.refundCardNumber = params.refundCardNumber;
    params.description = params.description;
    params.otherDescription = params.otherDescription;
    params.adminDescription = params.adminDescription;
    params.isClosed = params.isClosed;

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
              this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
            });
  }
}
