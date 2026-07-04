import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { ServerApis } from '../../core/server-apis';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-view-citizen',
  templateUrl: './view-citizen.component.html',
  styleUrls: ['./view-citizen.component.scss'],
})
export class ViewCitizenComponent extends AppBase implements AfterViewInit {
  userCode: string;
  info: any = {};
  loading: boolean = true;
  imageUrl: string;
  baseUrl: string = ServerApis.baseUrl;
  constructor(
) {
      super();
    this.route.params.subscribe((p) => {
      this.userCode = p.id;
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
        id: this.userCode,
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
        (error) => {
          this.loading = false;
        },
      );
  }

  back() {
    window.history.back();
  }
}
