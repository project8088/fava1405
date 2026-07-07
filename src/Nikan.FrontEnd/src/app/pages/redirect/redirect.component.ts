import { Component, OnInit } from '@angular/core';

import { ApiResult } from '@core/models/response';
import { ServerApis } from '@core/server-apis';
import { MatCardModule } from '@angular/material/card';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-redirect',
  templateUrl: './redirect.component.html',
  styleUrls: ['./redirect.component.scss'],
  imports: [MatCardModule],
})
export class RedirectComponent extends AppBase implements OnInit {
  serviceId = '';
  loading: boolean = true;
  url: string = '';
  returnUrl: string = '';
  constructor() {
    super();
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe((p) => {
      this.returnUrl = p['returnUrl'] ?? '';
    });

    this.route.queryParams.subscribe((params) => {
      if (params['serviceId']) {
        this.serviceId = params['serviceId'];
        this.getUserUrl();
      } else this.router.navigate(['/']);
    });
  }

  getUserUrl() {
    this.dataService
      .get(
        ServerApis.callCitizenService +
          '?serviceId=' +
          this.serviceId +
          '&returnUrl=' +
          this.returnUrl,
      )
      .subscribe(
        (data: ApiResult<any>) => {
          if (data.isSuccess) {
            this.url = data.data;
            if (data.data.indexOf('http') > -1) window.location.href = data.data;
            else this.router.navigate([data.data]);
          } else this.toastrService.error(data.messages);
        },
        (error) => {
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
  }
}
