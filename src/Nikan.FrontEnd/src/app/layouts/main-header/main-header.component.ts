import { Component, OnInit } from '@angular/core';
import { Meta, Title } from '@angular/platform-browser';
import { AuthUser } from '@core/authentication/user.model';
import { RegisterServiceModel } from '@core/models/register-service.model';
import { ServerApis } from '@core/server-apis';
import { SiteSettingViewModel } from '@core/models/setting';
import { AppBase } from '@app/app.base';
import { DateTime } from 'luxon';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-main-header',
  templateUrl: './main-header.component.html',
  styleUrls: ['./main-header.component.scss'],
  standalone: false,
})
export class MainHeaderComponent extends AppBase implements OnInit {
  isAuth: boolean = false;
  user?: AuthUser | null;
  datetime: any;
  loading?: boolean;
  slag: string = '';
  setting?: SiteSettingViewModel | null;

  loadingMenu: boolean = true;
  menuItems: any[] = [];
  baseUrl: string = ServerApis.baseUrl;
  registerTypes: RegisterServiceModel[] = [];

  constructor(
    private title: Title,
    private meta: Meta,
  ) {
    super();
    this.authService.currentUser.subscribe((u) => {
      if (u) {
        this.user = u;
        this.isAuth = true;
      } else {
        this.isAuth = false;
      }
    });

    this.dataService.getSetting().subscribe((response) => {
      this.setting = response;
      if (this.setting) {
        this.title.setTitle(this.setting.fullSiteName);
        this.meta.updateTag({ name: 'og:url', content: this.setting.siteUrl });
        this.meta.updateTag({
          name: 'description',
          content: this.setting.siteDescription,
        });
        this.meta.updateTag({
          name: 'keywords',
          content: this.setting.siteKeywords,
        });
      }
    });

    this.dataService
      .get(ServerApis.getMainMenuItems)
      .pipe(
        finalize(() => {
          this.loadingMenu = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess) {
          var data = response.data ? response.data : [];
          data.sort((a: any, b: any) => {
            if (a.tabOrder > b.tabOrder) return 1;
            else if (a.tabOrder < b.tabOrder) return -1;
            else return 0;
          });
          this.menuItems = this.getNestedChildren(data);
        } else {
          var msg = response.messages
            ? response.messages
            : 'دریافت متوی اصلی با خطا مواجه شده است.';
          this.toastrService.error(msg);
        }
      });

    this.dataService
      .get(ServerApis.getAppRegisterListForMainPage)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        const registerTypes: RegisterServiceModel[] = response.data ? response.data : [];
        this.registerTypes = registerTypes.map((el) => {
          return { value: String(el.serviceId), text: el.serviceName, ...el };
        });
      });
  }
  ngOnInit(): void {
    this.dataService
      .get(ServerApis.getCurrentShorDate, {})
      .pipe(finalize(() => this.chdr.detectChanges()))
      .subscribe((response) => {
        this.datetime = response.data ? new Date(response.data) : DateTime.now();
        // console.log(response.data, this.datetime);
      });
  }

  dashboard() {
    this.authService.goToDashboard();
  }

  logout() {
    this.authService.logout();
  }

  getNestedChildren(arr: any, parentId = null) {
    var out = [];
    for (var i in arr) {
      if (arr[i].parentId == parentId) {
        var children = this.getNestedChildren(arr, arr[i].id);

        arr[i].children = children;

        out.push(arr[i]);
      }
    }
    return out;
  }

  registerWithService(service: RegisterServiceModel) {
    if (service.haveTerms) {
      this.router.navigate(['/userregister/terms'], {
        queryParams: { serviceId: service.serviceId },
      });
    } else {
      this.router.navigate(['/userregister/preregister'], {
        queryParams: { serviceId: service.serviceId },
      });
    }
  }
}
