import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-refund-info',
  templateUrl: './citizen-refund-info.component.html',
  styleUrls: ['./citizen-refund-info.component.scss'],
  standalone: false,
})
export class CitizenRefundFullInfoComponent extends AppBase implements OnInit {
  isSaving=false;
  frm: FormGroup;
  showSaveCardNumberPanel: boolean;
  loadingData?: boolean;
  info: any;
  refundId: string;
  matDialogRef: any;
  constructor(private customValidator: CustomFormValidators) {
    super();
    this.frm = this.fb.group({
      refundId: [null],
      ownerBankCardNumber: [''],
    });
    this.route.params.subscribe((p) => {
      if (p.refundId) {
        this.refundId = p.refundId;
      }
    });
  }

  ngOnInit() {
    this.getRefundInfo();
  }
  getRefundInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getRefundInfoDetailsByAdmin, { id: this.refundId }).subscribe(
      (response) => {
        this.loadingData = false;
        if (response.isSuccess) {
          this.info = response.data ? response.data : {};
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loadingData = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  saveInfo() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
        return ;
    }

    this.isSaving = true;
    var url = ServerApis.updateRefundSaveCardNmber;
    var params = this.frm.value;
    params.refundId = this.info.refundId;
    params.ownerBankCardNumber = params.ownerBankCardNumber;

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
}
