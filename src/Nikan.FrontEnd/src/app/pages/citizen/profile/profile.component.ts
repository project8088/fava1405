import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, inject, OnInit } from '@angular/core';
import { map, shareReplay } from 'rxjs/operators';
import { Observable, finalize } from 'rxjs';
import { ServerApis } from '@core/server-apis';
import { ShortKarjoProfile } from '@core/models/citizen/global-information';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
  standalone: false,
})
export class CitizenProfileComponent extends AppBase implements OnInit {
  userId?: string;
  loading?: boolean;
  personInfo?: ShortKarjoProfile;
  baseUrl: string = ServerApis.baseUrl;
  protected breakpointObserver = inject(BreakpointObserver);

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map((result) => result.matches),
    shareReplay(),
  );

  constructor() {
    super();
    this.route.params.subscribe((p) => {
      this.userId = p['id'] && p['id'] != '0' ? p['id'] : '';
      this.getPersonalInfo();
    });
  }
  ngOnInit(): void {}

  public getPersonalInfo() {
    this.loading = true;
    this.dataService.get(ServerApis.getShortCitizenInfoByCitizen)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response && response.isSuccess) {
                this.personInfo = response.data;
                this.clearImageCache();
              } else {
                let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            }, (error: any) => {
              
            });
  }

  clearImageCache() {
    if (!this.personInfo) return;
    this.personInfo.personalPictureUrl =
      this.personInfo.personalPictureUrl + '?v=' + Math.random() * 100;
  }
}
