import { Component, OnInit, Input } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'admin-view-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.scss'],
  standalone: false,
})
export class AdminViewEventDetailsComponent extends AppBase implements OnInit {
  @Input('id') id: string = '';
  loadingData: boolean = true;
  event: any = {};
  baseUrl: string = ServerApis.baseUrl;
  constructor() {
    super();
  }

  ngOnInit(): void {
    this.getInfo();
  }

  getInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getEvent, { id: this.id }).subscribe(
      (response) => {
        this.loadingData = false;
        if (response.isSuccess) {
          this.event = response.data ? response.data : {};
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loadingData = false;
      },
    );
  }
}
