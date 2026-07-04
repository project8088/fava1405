import { OnInit, Component } from '@angular/core';
import { ServerApis } from '../../../core/server-apis';
import { DataService } from '../../../core/services/data-service.service';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'crd-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class CardDashboardComponent implements OnInit {
  events: any[] = [];
  loading: boolean;
  statisticalInfo: any;
  baseUrl: string = ServerApis.baseUrl;
  matDialog: any;

  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
  ) {}

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
