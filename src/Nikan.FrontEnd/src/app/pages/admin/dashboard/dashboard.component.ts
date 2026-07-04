import { Component, OnInit } from '@angular/core';
import { ServerApis } from '../../../core/server-apis';
import { AdminViewEventDetailsDialogComponent } from '../_dialogs/event-details/event-details.component';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class AdminDashboardComponent extends AppBase implements OnInit {
  events: any[] = [];
  loading: boolean;
  statisticalInfo: any;
  baseUrl: string = ServerApis.baseUrl;

  constructor(
) {
      super();}

  ngOnInit(): void {}
  ngAfterViewInit() {
    this.getStatisticalReport();
    this.getListevents();
  }
  getStatisticalReport() {
    this.loading = true;
    this.dataService.get(ServerApis.getStatisticalReport, {}).subscribe(
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

  getListevents() {
    this.dataService.get(ServerApis.getTopEvent).subscribe(
      (response) => {
        if (response.isSuccess) {
          this.events = response.data ? response.data : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {},
    );
  }

  openevents(message) {
    console.log('id ', message.id);
    this.matDialog.open(AdminViewEventDetailsDialogComponent, {
      data: {
        id: message.id,
      },
      panelClass: 'custom-dialog',
      minWidth: '80%',
    });
  }
}
