import { OnInit, Component } from '@angular/core';
import { DataService } from '../../../core/services/data-service.service';
import { ServerApis } from '../../../core/server-apis';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'webuser-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class WebUserDashboardComponent implements OnInit {
  loading: boolean;
  activeWaterMeter: any;
  errorMessage: string = '';
  loadingActiveWaterMeter: boolean = false;

  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
  ) {}
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
