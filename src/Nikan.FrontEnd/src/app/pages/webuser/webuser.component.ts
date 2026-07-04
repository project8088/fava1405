import { Component, OnInit, OnDestroy } from '@angular/core';
import { SideNavMenuItem } from '../../core/models/menuItems';
import { Observable } from 'rxjs';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { map, shareReplay } from 'rxjs/operators';
import { ServerApis } from '../../core/server-apis';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'webuser-webuserpanel',
  templateUrl: './webuser.component.html',
  styleUrls: ['./webuser.component.scss'],
    standalone: false
})
export class WebUserComponent extends AppBase implements OnInit, OnDestroy {
  theme: string = 'purple-love';

  miniSideBar: boolean;
  menuItems: SideNavMenuItem[] = [
    { name: 'داشبورد', icon: 'fa fa-chart-network', url: '/webuser/dashboard' },
  ];

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map((result) => result.matches),
    shareReplay(),
  );
  constructor(
    private breakpointObserver: BreakpointObserver
  ) {
      super();}

  ngOnInit(): void {
    document.body.classList.add(this.theme);
    this.getLastApiHelp();
  }
  ngOnDestroy() {
    document.body.classList.remove(this.theme);
  }

  getLastApiHelp() {
    this.dataService.get(ServerApis.getLastNews, {}).subscribe(
      (response) => {
        var arr = response.data ? response.data : [];
        for (var i in arr) {
          this.menuItems.push({
            name: arr[i].title,
            icon: 'fa fa-barcode',
            url: '/webuser/help-details/' + arr[i].id,
          });
        }

        this.menuItems.push({
          name: 'تغییر کلمه عبور',
          icon: 'fa fa-key',
          url: '/webuser/change-password',
        });
      },
      (error) => {
        alert('error');
      },
    );
  }

  logout() {
    this.authService.logout();
  }
}
