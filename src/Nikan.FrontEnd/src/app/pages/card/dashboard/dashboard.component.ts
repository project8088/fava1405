import { OnInit, Component } from '@angular/core';
import { ServerApis } from '../../../core/server-apis';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'crd-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
    standalone: false
})
export class CardDashboardComponent extends AppBase implements OnInit {
  events: any[] = [];
  loading: boolean;
  statisticalInfo: any;
  baseUrl: string = ServerApis.baseUrl;
  matDialog: any;

  constructor(
) {
      super();}

  ngOnInit(): void {
    this.getStatisticalReport();
  }

  getStatisticalReport() {
    this.loading = true;
    this.dataService.get(ServerApis.getStatisticalCardReport, {}).subscribe(
      (response) => {
        this.loading = false;
        if (response && response.isSuccess) {
          this.statisticalInfo = response.data ? response.data : {};
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
