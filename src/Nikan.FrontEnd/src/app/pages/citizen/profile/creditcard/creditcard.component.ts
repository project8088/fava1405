import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-creditcard',
  templateUrl: './creditcard.component.html',
  styleUrls: ['./creditcard.component.scss'],
  standalone: false,
})
export class CreditcardComponent extends AppBase implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  userId?: string;
  cardForm: FormGroup;
  loadingState: boolean = false;

  constructor() {
    super();
    this.cardForm = this.fb.group({
      cardNumber: [null, [Validators.required]],
      shabaNumber: [null, [Validators.required]],
    });

    this.getCardInfo();
  }

  ngOnInit(): void {}

  getCardInfo() {
    this.dataService.get(ServerApis.getCitizenBankCardNumber).subscribe((data) => {
      this.loading = false;
      this.cardForm.patchValue({
        cardNumber: data.data.cardNumber,
        shabaNumber: data.data.shabaNumber,
      });
    });
  }

  saveForm() {
    const form = this.cardForm.getRawValue();
    this.dataService.post(ServerApis.updteCitizenBankCardNumber, form).subscribe(
      (response) => {
        if (response && response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error: any) => {
        this.isSaving = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }
}
