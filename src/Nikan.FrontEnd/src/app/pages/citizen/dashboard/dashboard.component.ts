import { Component, OnInit, Sanitizer } from '@angular/core';

import { DataService } from '@core/services/data-service.service';
import { RegisterServiceModel } from '@core/models/models';
import { ServerApis } from '@core/server-apis';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-citizen-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class CitizenDashboardComponent implements OnInit {
  loading: boolean;
  registerTypes;
  loadingData: boolean;
  citizen: any = {};
  baseUrl: string = ServerApis.baseUrl;

  constructor(
    private toastrService: ToastrService,
    private dataService: DataService,
  ) {}
  ngOnInit(): void {
    this.dataService.get(ServerApis.getAppDashbordList).subscribe(
      (response) => {
        this.loading = false;
        const registerTypes: RegisterServiceModel[] = response.data ? response.data : [];
        this.registerTypes = registerTypes;
      },
      (error) => {
        this.loading = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );

    this.getCitizenInfo();
  }

  getCitizenInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getMyFullInfo).subscribe(
      (response) => {
        this.loadingData = false;
        if (response.isSuccess) {
          this.citizen = response.data ? response.data : {};
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loadingData = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }
}
