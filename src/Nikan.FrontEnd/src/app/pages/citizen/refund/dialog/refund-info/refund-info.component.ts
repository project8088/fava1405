import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '../../../../../core/custom-validator/form-validation';
import { DataService } from '../../../../../core/services/data-service.service';
import { ServerApis } from '../../../../../core/server-apis';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-refund-info-dialog',
  templateUrl: './refund-info.component.html',
  styleUrls: ['./refund-info.component.scss'],
})
export class CitizenRefundInfoDialogComponent implements OnInit {
  isSaving: boolean;
  frm: FormGroup;
  showSaveCardNumberPanel: boolean;
  loading: boolean = true;
  info: any;
  refundloading: boolean = false;
  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<CitizenRefundInfoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private customValidator: CustomFormValidators,
    private dataService: DataService,
  ) {
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
      return false;
    }

    this.isSaving = true;
    var url = ServerApis.updateRefundSaveCardNmber;
    var params = this.frm.value;
    params.refundId = this.info.refundId;
    params.ownerBankCardNumber = params.ownerBankCardNumber;
    params.adminDescription = params.adminDescription;

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
      (error) => {
        this.isSaving = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  commitRefundByAdmin(refundId) {
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
          .subscribe(
            (response) => {
              this.refundloading = false;
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
            (error) => {
              this.refundloading = false;
            },
          );
      }
    });
  }
}
