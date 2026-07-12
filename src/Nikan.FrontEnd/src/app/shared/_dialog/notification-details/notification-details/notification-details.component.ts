import { Component, OnInit, Input } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'app-view-notification-details',
  templateUrl: './notification-details.component.html',
  styleUrls: ['./notification-details.component.scss'],
  standalone: false,
})
export class ViewNotificationDetailsComponent extends AppBase implements OnInit {
  @Input('id') id: string = '';

  loadingData: boolean = true;

  notification: any = {};

  baseUrl: string = ServerApis.baseUrl;

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.getInfo();
  }

  getInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getNotification, { id: this.id, forEdit: false })
      .pipe(
        finalize(() => {
          this.loadingData = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response.isSuccess) {
                this.notification = response.data ? response.data : {};
              } else {
                let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            }, (error: any) => {
            });
  }
}
