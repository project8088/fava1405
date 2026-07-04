import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable } from 'rxjs';
import { shareReplay, map } from 'rxjs/operators';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { SideNavMenuItem } from '../../core/models/menuItems';
import { AuthUser } from '../../core/authentication/user.model';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-company',
  templateUrl: './company.component.html',
  styleUrls: ['./company.component.scss'],
  standalone: false,
})
export class CompanyComponent extends AppBase implements OnInit, OnDestroy {
  theme: string = 'purple-love';
  user: AuthUser;
  miniSideBar: boolean;
  menuItems: SideNavMenuItem[] = [];
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map((result) => result.matches),
    shareReplay(),
  );
  constructor(private breakpointObserver: BreakpointObserver) {
    super();
    this.authService.currentUser.subscribe((u) => {
      this.user = u;
      if (!this.user) return false;

      if (this.user.userCompanyStatus == 1) {
        this.menuItems = [
          { name: 'پیشخوان', icon: 'fa fa-bar-chart', url: '/company/dashboard' },
          { name: 'اطلاعات شرکت', url: '/company/company-profile/0', icon: 'fa fa-briefcase' },

          { name: 'پرسنل', url: '/company/citizenExcelBatchFile-list', icon: 'fa fa-briefcase' },

          { name: 'کاربران', url: '/company/users/0', icon: 'fa fa-users' },
          { name: 'پرسنل شرکت', url: '/company/personal/0', icon: 'fa fa-users' },
          { name: 'تیکت ها', url: '/company/tickets', icon: 'fa fa-envelope' },
          { name: 'تغییر کلمه عبور', icon: 'fa fa-key', url: '/company/change-password' },
        ];
      } else {
        this.menuItems = [
          { name: 'پیشخوان', icon: 'fa fa-bar-chart', url: '/company/dashboard' },
          { name: 'اطلاعات شرکت', url: '/company/company-profile/0', icon: 'fa fa-briefcase' },

          {
            name: 'پشتیبانی',
            icon: 'fa fa-envelope',
            children: [
              { name: 'تیکت ها', url: '/company/tickets' },
              { name: 'پیام های تماس با ما', url: '/company/contact-us' },
            ],
          },
          { name: 'تغییر کلمه عبور', icon: 'fa fa-key', url: '/company/change-password' },
        ];
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
}
