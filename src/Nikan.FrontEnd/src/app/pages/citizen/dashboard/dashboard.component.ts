import { Component, OnInit } from '@angular/core';
import { RegisterServiceModel } from '@core/models/models';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-citizen-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  standalone: false,
})
export class CitizenDashboardComponent extends AppBase implements OnInit {
  loading?: boolean;
  registerTypes: RegisterServiceModel[] = [];
  loadingData?: boolean;
  citizen: any = {};
  baseUrl: string = ServerApis.baseUrl;

  constructor() {
    super();
  }
  ngOnInit(): void {
    this.dataService.get(ServerApis.getAppDashbordList).subscribe(
      (response) => {
        this.loading = false;
        const registerTypes: RegisterServiceModel[] = response.data ? response.data : [];
        this.registerTypes = registerTypes;
      },
      (error: any) => {
        this.loading = false;
      },
    );

    this.getCitizenInfo();
  }

  getCitizenInfo() {
    this.loadingData = true;
    this.dataService
      .get(ServerApis.getMyFullInfo)
      .pipe(
        finalize(() => {
          this.loadingData = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response.isSuccess) {
            this.citizen = response.data ? response.data : {};
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
  }
}
