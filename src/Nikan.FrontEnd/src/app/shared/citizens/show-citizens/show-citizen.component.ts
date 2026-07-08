import { Component, AfterViewInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-show-citizen',
  templateUrl: './show-citizen.component.html',
  styleUrls: ['./show-citizen.component.scss'],
  standalone: false,
})
export class AppShowCitizenComponent extends AppBase implements AfterViewInit {
  userCode: string = '';
  info: any = {};
  loading: boolean = true;
  imageUrl: string = '';
  baseUrl: string = ServerApis.baseUrl;

  constructor() {
    super();
    this.route.params.subscribe((p) => {
      this.userCode = p['id'];
    });
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.getInfo();
  }

  getInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getCitizenFullInfo, {
        userCode: this.userCode,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess) {
            this.info = response.data ? response.data : {};
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error:any) => {
          this.loading = false;
        },
      );
  }

  back() {
    window.history.back();
  }

  sendcitizenForAuthentication(citizenId: number) {
    this.dataService
      .get(ServerApis.citizenForAuthenticationByCitizenId, { citizenId: citizenId })
      .subscribe(
        (response) => {
          if (response.isSuccess && response.data) {
            this.toastrService.success(response.messages);
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error:any) => {},
      );
  }
}
