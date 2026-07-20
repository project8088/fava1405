import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { finalize, map, shareReplay } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { AppBase } from '@app/app.base';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@core/material/material.module';
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
import { ROLES } from '@core/authentication/auth.service';
import { CoreModule } from '../core/core.module';
import { FormsModule } from '@angular/forms';
import { UploadUserAvatarDialogComponent } from '@app/shared/_dialog/upload-avatar/upload-avatar.component';
import { MenuDynamicComponent } from '@app/shared/menu-dynamic/menu-dynamic.component';

@Component({
  selector: 'app-panel',
  templateUrl: './panel.component.html',
  styleUrls: ['./panel.component.scss'],
  imports: [
    CommonModule,
    MaterialModule,
    MatSidenavModule,
    RouterModule,
    CoreModule,
    FormsModule,
    MenuDynamicComponent,
  ],
})
export class PanelComponent extends AppBase implements OnInit, OnDestroy {
  theme: string = 'default';

  miniSideBar: boolean = false;

  selectedRole = '';
  roleMenus = new Map<string, SideNavMenuItem[]>();
  selectedRoleMenu: SideNavMenuItem[] = [];
  protected breakpointObserver = inject(BreakpointObserver);

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map((result) => result.matches),
    shareReplay(),
  );
  user?: AuthUser | null;
  baseUrl: string = ServerApis.baseUrl;
  userImage: string = '';
  constructor() {
    super();
    this.authService.currentUser.subscribe((u) => {
      this.user = u;
      this.roleMenus = new Map<string, SideNavMenuItem[]>();
      this.selectedRoleMenu = [];
      if (!this.user) return;

      if (this.user?.isJobseeker) this.getJobseekerImage();
      if (this.user?.isCompany) this.getCompanyLogo();
      if (this.user?.isCitizen) this.getCitizenImage();

      const roles = this.user.roles ?? [];
      // admin
      if (roles.indexOf(ROLES.admin) > -1) {
        this.roleMenus.set(ROLES.admin, ADMIN_MENU);
      }
      // company
      if (roles.indexOf(ROLES.company) > -1) {
        if (this.user.userCompanyStatus == 1) {
          this.roleMenus.set(ROLES.company, COMPANY_MENU_STATE_1);
        } else {
          this.roleMenus.set(ROLES.company, COMPANY_MENU_STATE_2);
        }
      }
      // card
      if (roles.indexOf(ROLES.card) > -1) {
        this.roleMenus.set(ROLES.card, CARD_MENU);
      }
      // web api user
      if (roles.indexOf(ROLES.webapiuser) > -1) {
        this.getLastApiHelp((list) => {
          this.roleMenus.set(ROLES.webapiuser, [...WEB_USER_MENU, ...list]);
        });
      }
      // citizen
      if (roles.indexOf(ROLES.citizen) > -1) {
        this.roleMenus.set(ROLES.citizen, CITIZEN_MENU);
      }

      this.selectedRole = this.roleMenus.entries().next().value?.[0] ?? '';
      this.selectedRoleMenu = this.roleMenus.get(this.selectedRole) ?? [];
    });
  }

  ngOnInit(): void {
    document.body.classList.add(this.theme);
  }
  ngOnDestroy() {
    document.body.classList.remove(this.theme);
  }

  getLastApiHelp(callback: (list: SideNavMenuItem[]) => void) {
    this.dataService
      .get(ServerApis.getLastNews, {})
      .pipe(
        finalize(() => {
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        var arr = response.data ? response.data : [];
        let list: SideNavMenuItem[] = [];
        for (var i in arr) {
          list.push({
            name: arr[i].title,
            icon: 'fa fa-barcode',
            url: '/webuser/help-details/' + arr[i].id,
          });
        }
        list.push({
          name: 'تغییر کلمه عبور',
          icon: 'fa fa-key',
          url: '/webuser/change-password',
        });

        callback(list);
      });
  }

  logout() {
    this.authService.logout(true);
  }

  openUploadDialog() {
    if (!this.user?.isCompany && !this.user?.isJobseeker && !this.user?.isCitizen) return;

    this.matDialog
      .open(UploadUserAvatarDialogComponent, {
        data: { imageUrl: this.userImage },
        panelClass: 'custom-dialog',
        width: '85%',
        height: '90%',
      })
      .afterClosed()
      .subscribe((res) => {
        if (res) {
          this.userImage = res;
          this.chdr.detectChanges();
        }
      });
  }

  getJobseekerImage() {
    this.dataService
      .get(ServerApis.getKarjoImage)
      .pipe(
        finalize(() => {
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess) this.userImage = response.data.imageUrl;
      });
  }
  getCompanyLogo() {
    this.dataService
      .get(ServerApis.getCompanyLogo)
      .pipe(
        finalize(() => {
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess) this.userImage = response.data.imageUrl;
      });
  }
  getCitizenImage() {
    this.dataService
      .get(ServerApis.getShortCitizenInfoByCitizen)
      .pipe(
        finalize(() => {
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess)
          this.userImage = response.data.personalPictureUrl + '?v=' + Math.random() * 1000;
      });
  }
  onChangeRole() {
    this.selectedRoleMenu = this.roleMenus.get(this.selectedRole) ?? [];
    this.chdr.detectChanges();
  }
}
