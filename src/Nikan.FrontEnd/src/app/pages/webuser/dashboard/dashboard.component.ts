import { OnInit, Component } from '@angular/core';
import { ServerApis } from '../../../core/server-apis';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'webuser-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class WebUserDashboardComponent extends AppBase implements OnInit {
  loading: boolean;
  activeWaterMeter: any;
  errorMessage: string = '';
  loadingActiveWaterMeter: boolean = false;

  constructor(
) {
      super();}
  ngOnInit() {
    this.getActiveWaterMeterPeriod();
  }

  getActiveWaterMeterPeriod(): void {
    this.loading = true;
    this.dataService.get('', {}).subscribe(
      (response) => {
        this.loading = false;
        if (response && response.isSuccess) {
          this.loadingActiveWaterMeter = true;
          this.activeWaterMeter = response.data ? response.data : {};
        } else {
          this.loadingActiveWaterMeter = false;
          const msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.errorMessage = msg;
        }
      },
      (error) => {
        this.loading = false;
      },
    );
  }
}
