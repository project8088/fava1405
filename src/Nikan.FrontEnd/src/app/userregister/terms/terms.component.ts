import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-terms',
  templateUrl: './terms.component.html',
  styleUrls: ['./terms.component.scss'],
  standalone: false,
})
export class TermsComponent extends AppBase implements OnInit {
  regLink: string = '';
  service = { serviceName: null, terms: [], haveTerms: false };
  serviceId?: string;
  loadingData?: boolean;

  constructor() {
    super();
    this.route.queryParams.subscribe((params) => {
      this.serviceId = params['serviceId'];
    });
  }

  ngOnInit(): void {
    this.getServiceInfo();
  }

  getServiceInfo() {
    this.loadingData = true;
    this.dataService
      .get(ServerApis.getAppInfo, {
        id: this.serviceId,
      })
      .subscribe(
        (response) => {
          this.loadingData = false;
          if (response.isSuccess) {
            debugger;
            this.service = response.data;
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loadingData = false;
        },
      );
  }
}
