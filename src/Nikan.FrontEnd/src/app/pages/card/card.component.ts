import { Component, OnInit, OnDestroy } from '@angular/core';
import { SideNavMenuItem } from '../../core/models/menuItems';
import { Observable } from 'rxjs';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { map, shareReplay } from 'rxjs/operators';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'crd-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.scss'],
})
export class CardComponent extends AppBase implements OnInit, OnDestroy {
  theme: string = 'purple-love';

  miniSideBar: boolean;
  menuItems: SideNavMenuItem[] = [
    { name: 'پروفایل شهروندی', icon: 'fa fa-reply', url: '/citizen/dashboard' },
    { name: 'پیشخوان', icon: 'fa fa-chart-network', url: '/card/dashboard' },
    { name: 'کاربران', permission: 'cardmanageusers', icon: 'fa fa-user', url: '/card/card-users' },
    {
      name: 'تنظیمات کارت',
      permission: 'cardsettings',
      icon: 'fa fa-cog',
      url: '/card/order-card-list',
    },

    {
      name: 'جستجوی شهروند',
      permission: 'cardsearchcitizen',
      icon: 'fa fa-search',
      url: '/card/search-citizen',
    },
    {
      name: 'تصاویر',
      permission: 'cardcitizenspictures',
      url: '/card/card-citizens-pictures',
      icon: 'fa fa-image',
    },
    {
      name: 'جستجوی کارت',
      permission: 'cardmanage',
      url: '/card/advanced-search-card-citizen',
      icon: 'fa fa-search',
    },
    { name: 'صدور کارت رایگان ', url: '/card/free-request-card-list', icon: 'fa fa-file-export' },
    {
      name: 'خروجی صدور کارت',
      permission: 'cardexport',
      url: '/card/export-card-citizen',
      icon: 'fa fa-file-export',
    },
    {
      name: 'توزیع کارت',
      permission: 'carddistribute',
      url: '/card/card-distribute-course-list',
      icon: 'fa fa-braille',
    },
    {
      name: 'صف توزیع کارت',
      permission: 'carddistribute',
      url: '/card/citizen-card-search-in-queue',
      icon: 'fa fa-file-export',
    },
    {
      name: 'تراکنش های مالی',
      permission: 'cardtrasaction',
      url: '/card/transaction-list',
      icon: 'fa fa-envelope',
    },
    {
      name: 'تیکت ها',
      permission: 'cardticket',
      url: '/card/tickets',
      icon: 'fa fa-location-arrow',
    },
    { name: 'تغییر کلمه عبور', icon: 'fa fa-key', url: '/card/change-password' },
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
  }
  ngOnDestroy() {
    document.body.classList.remove(this.theme);
  }

  logout() {
    this.authService.logout();
  }
}
