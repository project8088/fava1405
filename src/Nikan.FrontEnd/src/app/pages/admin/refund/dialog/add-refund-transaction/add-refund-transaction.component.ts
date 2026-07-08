import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-add-refund-transaction-dialog',
  templateUrl: './add-refund-transaction.component.html',
  styleUrls: ['./add-refund-transaction.component.scss'],
  standalone: false,
})
export class AdminAddRefundTransactionDialogComponent extends AppBase implements OnInit {
  isSaving=false;
  frm: FormGroup;
  loading: boolean = true;
  importId: number = 0;
  constructor(
    private matDialogRef: MatDialogRef<AdminAddRefundTransactionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.frm = this.fb.group({
      orderId: [null, [Validators.required]],
      transactionCode: [null, [Validators.required]],
      totalRefundAmount: [null, [Validators.required]],
      refundAmount: [null, [Validators.required]],
      nationCode: [null, [Validators.required]],
      description: [''],
      otherDescription: [''],
      adminDescription: [''],
      isClosed: [false],
    });
    this.importId = _data.importId;
  }

  ngOnInit() {}

  saveInfo() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
        return ;
    }

    this.isSaving = true;
    var url = ServerApis.addRefund;
    var params = this.frm.value;
    params.importId = this.importId;
    params.orderId = params.orderId;
    params.transactionCode = params.transactionCode;
    params.totalRefundAmount = +params.totalRefundAmount;
    params.refundAmount = +params.refundAmount;
    params.description = params.description;
    params.nationCode = params.nationCode;
    params.description = params.description;
    params.otherDescription = params.otherDescription;
    params.adminDescription = params.adminDescription;
    params.isClosed = params.isClosed;

    this.dataService.post(url, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response && response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
          this.matDialogRef.close(true);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error:any) => {
        this.isSaving = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }
}
