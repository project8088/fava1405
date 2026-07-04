import { Component, OnInit } from '@angular/core';
import { ServerApis } from '../../../../core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-citizen-Identity-Info',
  templateUrl: './citizen-Identity-Info.component.html',
  styleUrls: ['./citizen-Identity-Info.component.scss'],
  standalone: false,
})
export class AdminCitizenIdentityInfoComponent extends AppBase implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  userId: string;
  info: any;
  userStatus: number;
  userCode: string = '';
  constructor() {
    super();
    this.route.params.subscribe((p) => {
      if (p.id != '0' && p.id) this.userCode = p.id;
      this.getPersonalInfo();
    });
  }

  ngOnInit() {
    this.getPersonalInfo();
  }

  getPersonalInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getIdentityInformationByAdmin, { userCode: this.userCode })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response && response.isSuccess) {
            this.info = response.data;
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loading = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
  }
}
