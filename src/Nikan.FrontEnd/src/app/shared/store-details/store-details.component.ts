import { Component, OnInit } from '@angular/core';
import { ServerApis } from '../../core/server-apis';
import { AuthUser } from '../../core/authentication/user.model';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-store-details',
  templateUrl: './store-details.component.html',
  styleUrls: ['./store-details.component.scss'],
  standalone: false,
})
export class StoreDetailsComponent extends AppBase implements OnInit {
  id: string;
  loading: boolean;
  info: any;
  user: AuthUser;

  constructor() {
    super();
    this.user = this.authService.currentUserValue;
    this.route.params.subscribe((p) => {
      this.id = p.id;
      this.getInfo();
    });
  }

  ngOnInit(): void {}

  getInfo() {
    this.loading = true;
    return this.dataService.get(ServerApis.getStoreSaleItems, { id: this.id }).subscribe(
      (response) => {
        this.loading = false;
        if (response.isSuccess && response.data) {
          this.info = response.data ? response.data : {};
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

  buy() {
    this.toastrService.info('در حال حاضر امکان ارتباط با درگاه پرداخت برقرار نیست.');
  }
}
