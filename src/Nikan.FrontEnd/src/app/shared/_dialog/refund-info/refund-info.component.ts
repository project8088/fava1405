import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import Swal from 'sweetalert2';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-refund-info-dialog',
  templateUrl: './refund-info.component.html',
  styleUrls: ['./refund-info.component.scss'],
  standalone: false,
})
export class CitizenRefundInfoDialogComponent extends AppBase implements OnInit {
  isSaving = false;
  frm: FormGroup;
  showSaveCardNumberPanel: boolean = false;
  loading: boolean = true;
  info: any;
  refundloading: boolean = false;
  constructor(
    private matDialogRef: MatDialogRef<CitizenRefundInfoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.info = _data.info;

    this.frm = this.fb.group({
      refundId: [null],
      ownerBankCardNumber: [''],
      adminDescription: [''],
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
    var url = ServerApis.updateRefundSaveCardNmber;
    var params = this.frm.value;
    params.refundId = this.info.refundId;
    params.ownerBankCardNumber = params.ownerBankCardNumber;
    params.adminDescription = params.adminDescription;

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

  commitRefundByAdmin(refundId: number) {
    Swal.fire({
      title: 'آیا برای برگشت هزینه به شماره کارت نمایش داده شده مطمئن هستید',
      text: ' در صورت تایید برگشت هزینه صرفا به شماره کارتی برگشت داده می شود که هزینه پرداخت شده است',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.refundloading = true;
        this.dataService
          .get(ServerApis.commitRefundByAdmin, {
            refundId: refundId,
          })
          .pipe(
            finalize(() => {
              this.refundloading = false;
              this.chdr.detectChanges();
            }),
          )
          .subscribe(
            (response) => {
              if (response.isSuccess) {
                this.toastrService.success('با موفقیت برگشت داده شد.');
                this.matDialogRef.close(true);
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            },
            (error: any) => {},
          );
      }
    });
  }
}
