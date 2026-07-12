import { Component, Input, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { FormGroup } from '@angular/forms';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'app-view-citizen-profile',
  templateUrl: './view-citizen-profile.component.html',
  styleUrls: ['./view-citizen-profile.component.scss'],
  standalone: false,
})
export class ViewCitizenProfileComponent extends AppBase implements OnInit {
  @Input('userCode') userCode: string = '';

  feedbackfrm!: FormGroup;
  citizen: any = {};
  manzalatdata: any[] = [];
  loading?: boolean;
  loadingData?: boolean;
  maritalStatus: any;

  baseUrl: string = ServerApis.baseUrl;

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.getCitizenInfo();
  }
  getCitizenInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getCitizenFullInfo, { userCode: this.userCode })
      .pipe(
        finalize(() => {
          this.loadingData = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response.isSuccess) {
                this.citizen = response.data ? response.data : {};
              } else {
                let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            }, (error: any) => {
              this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
            });
  }
}
