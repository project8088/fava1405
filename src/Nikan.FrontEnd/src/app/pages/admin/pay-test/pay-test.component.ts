import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { ServerApis } from '../../../core/server-apis';
import { DataService } from '../../../core/services/data-service.service';
@Component({
  selector: 'adm-pay-test',
  templateUrl: './pay-test.component.html',
  styleUrls: ['./pay-test.component.scss'],
})
export class AdminPayTestComponent implements OnInit {
  loading: boolean;
  form: FormGroup;
  RefId: string;
  waitForRedirectToBank: boolean;
  constructor(
    private dataService: DataService,
    private fb: FormBuilder,
    private toastrService: ToastrService,
  ) {
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
