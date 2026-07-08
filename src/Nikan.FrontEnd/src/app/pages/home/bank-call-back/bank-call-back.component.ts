import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'home-bank-call-back',
  templateUrl: './bank-call-back.component.html',
  styleUrls: ['./bank-call-back.component.scss'],
  standalone: false,
})
export class BankCallBackComponent extends AppBase implements OnInit {
    loading?: boolean;
  id: string ='';

  response: any;

  constructor() {
    super();
    this.route.queryParams.subscribe((p) => {
      if (p['id']) {
        this.id = p['id'];
        this.payVerify();
      } else {
        this.toastrService.warning('شناسه پرداخت وارد نشده است.');
      }
    });
  }

  ngOnInit(): void {}

  payVerify() {
    this.loading = true;
    this.dataService.post(ServerApis.showPayResult, { id: this.id }).subscribe(
      (response) => {
        this.loading = false;
        this.response = response;
        if (response.isSuccess) {
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error:any) => {
        this.loading = false;
      },
    );
  }
}
