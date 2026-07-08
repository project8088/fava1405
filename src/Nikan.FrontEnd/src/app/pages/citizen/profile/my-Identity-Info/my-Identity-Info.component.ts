import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-my-Identity-Info',
  templateUrl: './my-Identity-Info.component.html',
  styleUrls: ['./my-Identity-Info.component.scss'],
  standalone: false,
})
export class CitizenMyIdentityInfoComponent extends AppBase implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  userId?: string;
  info: any;
  userStatus?: number;

  constructor() {
    super();
  }

  ngOnInit() {
    this.getPersonalInfo();
  }

  getPersonalInfo() {
    this.loading = true;
    this.dataService.get(ServerApis.getIdentityInformationByCitizen).subscribe(
      (response) => {
        this.loading = false;
        if (response && response.isSuccess) {
          this.info = response.data;
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error:any) => {
        this.loading = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }
}
