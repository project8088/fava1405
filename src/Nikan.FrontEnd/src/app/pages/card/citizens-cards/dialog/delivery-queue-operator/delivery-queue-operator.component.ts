import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'card-delivery-queue-operator',
  templateUrl: './delivery-queue-operator.component.html',
  styleUrls: ['./delivery-queue-operator.component.scss'],
  standalone: false,
})
export class CardDeliveryQueueOperatorDialogComponent extends AppBase implements OnInit {
  isSaving=false;
  frm: FormGroup;
  queueId: string ='';
  loading: boolean = true;
  info: any;
  constructor(
    private matDialogRef: MatDialogRef<CardDeliveryQueueOperatorDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
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
        return ;
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
      (error:any) => {
        this.isSaving = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }
}
