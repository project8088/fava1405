import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { DataService } from '@core/services/data-service.service';
import { ServerApis } from '@core/server-apis';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-creditcard',
  templateUrl: './creditcard.component.html',
  styleUrls: ['./creditcard.component.scss'],
})
export class CreditcardComponent implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  userId: string;
  cardForm: FormGroup;
  loadingState: boolean;

  constructor(
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private dataService: DataService,
  ) {
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
      (error) => {
        this.isSaving = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }
}
