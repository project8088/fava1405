import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { finalize, map, shareReplay } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { AppBase } from '@app/app.base';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@core/material/material.module';
import { SideNavMenuComponent } from '@app/shared/side-nav-menu/side-nav-menu.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { RouterModule } from '@angular/router';
import { AuthUser } from '@core/authentication/user.model';
import { SideNavMenuItem } from '@core/models/menuItems';
import { COMPANY_MENU_STATE_2 } from './COMPANY_MENU_STATE_2';
import { COMPANY_MENU_STATE_1 } from './COMPANY_MENU_STATE_1';
import { ADMIN_MENU } from './ADMIN_MENU';
import { CARD_MENU } from './CARD_MENU';
import { CITIZEN_MENU } from './CITIZEN_MENU';
import { WEB_USER_MENU } from './WEB_USER_MENU';
import { ServerApis } from '@core/server-apis';

@Component({
  selector: 'app-panel',
  templateUrl: './panel.component.html',
  styleUrls: ['./panel.component.scss'],
  imports: [CommonModule, MaterialModule, MatSidenavModule, RouterModule, SideNavMenuComponent],
})
export class PanelComponent extends AppBase implements OnInit, OnDestroy {
  theme: string = 'default';

  miniSideBar: boolean = false;
  menuItems: SideNavMenuItem[] = [];
  protected breakpointObserver = inject(BreakpointObserver);

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map((result) => result.matches),
    shareReplay(),
  );
  user?: AuthUser | null;
  constructor() {
    super();
    this.authService.currentUser.subscribe((u) => {
      this.user = u;
      if (!this.user) return;
      this.menuItems = [];
      if (this.user.isAdmin) {
        this.menuItems = ADMIN_MENU;
      } else if (this.user.isCompany) {
        if (this.user.userCompanyStatus == 1) {
          this.menuItems = COMPANY_MENU_STATE_1;
        } else {
          this.menuItems = COMPANY_MENU_STATE_2;
        }
      } else if (this.user.isCardUser) {
        this.menuItems = CARD_MENU;
      } else if (this.user.isCitizen) {
        this.menuItems = CITIZEN_MENU;
      } else if (this.user.isWebUser) {
        this.menuItems = WEB_USER_MENU;
        this.getLastApiHelp();
      }
    });
  }

  ngOnInit(): void {
    document.body.classList.add(this.theme);
  }
  ngOnDestroy() {
    document.body.classList.remove(this.theme);
  }

  logout() {
    this.authService.logout();
  }

  getLastApiHelp() {
    this.dataService
      .get(ServerApis.getLastNews, {})
      .pipe(
        finalize(() => {
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
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
      });
  }
}
