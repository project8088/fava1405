import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { map, shareReplay } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ServerApis } from '../../../core/server-apis';
import { ShortKarjoProfile } from '@core/models/citizen/global-information';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-citizen-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class CitizenProfileComponent extends AppBase implements OnInit {
  userId: string;
  loading: boolean;
  personInfo: ShortKarjoProfile;
  baseUrl: string = ServerApis.baseUrl;

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map((result) => result.matches),
    shareReplay(),
  );

  constructor(
    private breakpointObserver: BreakpointObserver
  ) {
      super();
    this.route.params.subscribe((p) => {
      this.userId = p.id && p.id != '0' ? p.id : '';
      this.getPersonalInfo();
    });
  }
  ngOnInit(): void {}

  public getPersonalInfo() {
    this.loading = true;
    this.dataService.get(ServerApis.getShortCitizenInfoByCitizen).subscribe(
      (response) => {
        this.loading = false;
        if (response && response.isSuccess) {
          this.personInfo = response.data;
          this.clearImageCache();
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

  clearImageCache() {
    this.personInfo.personalPictureUrl =
      this.personInfo.personalPictureUrl + '?v=' + Math.random() * 100;
  }
}
