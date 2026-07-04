import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '../../../core/server-apis';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-pay-test',
  templateUrl: './pay-test.component.html',
  styleUrls: ['./pay-test.component.scss'],
    standalone: false
})
export class AdminPayTestComponent extends AppBase implements OnInit {
  loading: boolean;
  form: FormGroup;
  RefId: string;
  waitForRedirectToBank: boolean;
  constructor(
) {
      super();
    this.form = this.fb.group({
      amount: [0, [Validators.required]],
      orderId: [0, [Validators.required]],
    });
  }

  ngOnInit(): void {}

  pay() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
      return false;
    }

    this.loading = true;
    var formValue = this.form.value;
    this.dataService
      .post(ServerApis.testPay, {
        OrderId: +formValue.orderId,
        Amount: +formValue.amount,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess) {
            this.RefId = response.data.refId;
            var form: any = document.getElementById('payFormMellat');
            this.waitForRedirectToBank = true;
            setTimeout(() => {
              form.submit();
            }, 1000);
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loading = false;
        },
      );
  }
}
