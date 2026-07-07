import { Component, Input, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-view-card-profile',
  templateUrl: './view-card-profile.component.html',
  styleUrls: ['./view-card-profile.component.scss'],
  standalone: false,
})
export class ViewCardProfileComponent extends AppBase implements OnInit {
  @Input('cardId') cardId: string = '';

  card: any = {};
  loadingData?: boolean;
  maritalStatus: any;

  baseUrl: string = ServerApis.baseUrl;

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.getCitizenInfo();
    this.dataService.getEnums().subscribe((data) => {
      this.maritalStatus = data.maritalStatus.map((el) => el.text);
    });
  }

  getCitizenInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getCitizenCardInfoByCardId, { id: this.cardId }).subscribe(
      (response) => {
        this.loadingData = false;
        if (response.isSuccess) {
          this.card = response.data ? response.data : {};
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
