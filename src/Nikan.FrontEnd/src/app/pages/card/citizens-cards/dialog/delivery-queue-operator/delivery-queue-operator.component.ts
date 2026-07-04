import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '../../../../../core/custom-validator/form-validation';
import { DataService } from '../../../../../core/services/data-service.service';
import { ServerApis } from '../../../../../core/server-apis';

@Component({
  selector: 'card-delivery-queue-operator',
  templateUrl: './delivery-queue-operator.component.html',
  styleUrls: ['./delivery-queue-operator.component.scss'],
})
export class CardDeliveryQueueOperatorDialogComponent implements OnInit {
  isSaving: boolean;
  frm: FormGroup;
  queueId: string;
  loading: boolean = true;
  info: any;
  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<CardDeliveryQueueOperatorDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private customValidator: CustomFormValidators,
    private dataService: DataService,
  ) {
    this.info = _data.info;

    this.frm = this.fb.group({
      queueId: [null],
      nationalCode: [null, [Validators.required]],
      description: [''],
    });
    this.queueId = _data.info.id;
  }

  ngOnInit() {}

  saveInfo() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return false;
    }

    this.isSaving = true;
    var url = ServerApis.deliveryQueueToOperator;
    var params = this.frm.value;
    params.queueId = +this.queueId;
    params.nationalCode = params.nationalCode;
    params.description = params.description;

    this.dataService.post(url, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response && response.isSuccess) {
          this.toastrService.success(response.messages);
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
}
