import { OnInit, Component } from '@angular/core';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'webuser-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  standalone: false,
})
export class WebUserDashboardComponent extends AppBase implements OnInit {
  loading?: boolean;
  activeWaterMeter: any;
  errorMessage: string = '';
  loadingActiveWaterMeter: boolean = false;

  constructor() {
    super();
  }
  ngOnInit() {
    this.getActiveWaterMeterPeriod();
  }

  getActiveWaterMeterPeriod(): void {
    this.loading = true;
    this.dataService.get('', {})
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response && response.isSuccess) {
                this.loadingActiveWaterMeter = true;
                this.activeWaterMeter = response.data ? response.data : {};
              } else {
                this.loadingActiveWaterMeter = false;
                const msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.errorMessage = msg;
              }
            }, (error: any) => {
            });
  }
}
