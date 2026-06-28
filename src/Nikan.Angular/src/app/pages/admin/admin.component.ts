import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { map, shareReplay } from 'rxjs/operators';

import { AuthService } from '../../core/authentication/auth.service';
import { Observable } from 'rxjs';
import { SideNavMenuItem } from '../../core/models/menuItems';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
})
export class AdminComponent implements OnInit, OnDestroy {
  theme: string = 'default';

  miniSideBar: boolean;
  menuItems: SideNavMenuItem[] = [
    { name: 'پیشخوان', icon: 'fa fa-chart-network', url: '/admin/dashboard' },
    {
      name: 'اطلاعات پایه',
      permission: 'basedata',
      icon: 'fa fa-database',
      children: [ 
        { name: 'گروههای شهروندی', url: '/admin/group-list' },
        { name: 'خدمات شهروندی', url: '/admin/appService-list' },
        { name: 'تنظیمات سامانه', url: '/admin/setting' },
        { name: 'تنظیمات پیامک', url: '/admin/sms-setting' },
      ],
    },

    {
      name: 'مدیریت سازمان ها',
      permission: 'organ',
      icon: 'fa fa-database',
      children: [
        { name: 'سازمان ها', url: '/admin/organization' }, 
      
      ],
    },
    {
      name: 'مدیریت کاربران',
      permission: 'usermanagement',
      icon: 'fa fa-database',
      children: [
       
        { name: 'گروه های کاربری', url: '/admin/admin-userGroups' },
        { name: 'لیست کاربران', url: '/admin/all-users' },
        { name: 'مدیران سامانه', url: '/admin/admin-users' },
        { name: 'توسعه دهندگان', url: '/admin/web-api-users' },

      ],
    },

    

    {
      name: 'شهروندان',
      permission: 'citizens',
      icon: 'fa fa-star',
      children: [
        { name: 'جستجوی شهروند', url: '/admin/search-citizen' },
        { name: 'جستجوی پیشرفته شهروند', url: '/admin/advanced-search-citizen' },
        { name: 'بازبینی تصاویر', url: '/admin/citizens-pictures' },
        { name: 'بازخوردهای ثبت شده', url: '/admin/all-citizens-feedBacks' },
        { name: 'خانواده شهروند', url: '/admin/search-citizen-family' }, 
        { name: 'گروههای شهروندی', url: '/admin/group-list' },
        { name: 'پیامک های ارسال شده', url: '/admin/sms-list' }, 
        { name: 'ثبت نام دسته ایی شهروند', url: '/admin/citizen-register-file-list' },
      

        
      ],
    },
    {
      name: 'طرح منزلت', permission: 'manzalat', icon: 'fa fa-bars', children: [
        { name: 'طرح منزلت', url: '/admin/search-manzelat-citizens' },
     /*   { name: 'تنظیمات منزلت', url: '/admin/manzelat-settings' },*/
            { name: 'تنظیمات  ', url: '/admin/manzalat-form-list' },
        //ManzalatSettingComponent
      ]
    },



    {
      name: 'مدیریت کارت شهروندی', permission: 'card', icon: 'fa fa-bars', children: [
        { name: 'جستجوی کارت شهروندی', url: '/admin/advanced-search-card-citizen' },
        { name: 'مشخصات کارت', url: '/admin/card-list' },
        { name: 'خروجی صدور کارت', url: '/admin/export-search-card-citizen' },
      ]
    },


    {
      name: 'اشخاص حقوقی',
      permission: 'companymanagement',
      icon: 'fa fa-building',
      children: [
        { name: 'اشخاص حقوقی', url: '/admin/companies' },
      ],
    },



{
  name: 'احراز هویت',
  permission: 'sabtahval',
      icon: 'fa fa-star',
      children: [
        { name: 'لیست ورودی و خروجی', url: '/admin/sabtAhval-list' },
        { name: 'احراز هویت شهروند', url: '/admin/citizens-authentication' },
        { name: 'جستجوی استعلام شهروند', url: '/admin/citizens-authentication-search' },
        
      ],
    },
    {
      name: 'امور مالی',
      permission: 'financial',
      icon: 'fa fa-coins',
      children: [
        { name: 'تنظیمات مالی', url: '/admin/pay-setting' },
        { name: 'تراکنش های مالی', url: '/admin/transaction-list' }, 
        { name: 'تست پرداخت', url: '/admin/pay-test' },
      ],
    },


    {
      name: 'استرداد هزینه',
      permission: 'refundmanagement',
      icon: 'fa fa-coins',
      children: [
       
        { name: 'بارگذاری فایل استرداد', url: '/admin/refund-file-list' },
        { name: 'لیست دسترسی های استرداد', url: '/admin/refund-access-list' },
        { name: 'جستجوی استرداد', url: '/admin/search-refund' },
        { name: 'کاربران استرداد', url: '/admin/refund-users' },


        
      ],
    },




    {
      name: 'پشتیبانی',
      permission: 'supportmanagement',
      icon: 'fa fa-headphones',
      children: [
        { name: 'اطلاعیه ها', url: '/admin/notifications' },
        { name: 'موضوع های تیکت', url: '/admin/ticket-subjects' },
        { name: 'تیکت ها', url: '/admin/tickets' },
        { name: 'پیام های تماس با ما', url: '/admin/contact-us' },
      ],
    },

    {
      name: 'مدیریت محتوا',
      permission: 'contentmanagement',
      icon: 'fa fa-star',
      children: [
        { name: 'گروه های خبری', url: '/admin/news-groups' },
        { name: 'راهنمای توسعه دهندگان', url: '/admin/news-list' },
        { name: 'ثبت راهنمای جدید', url: '/admin/addupdate-news/0' },
        { name: 'گروه های پرسش و پاسخ', url: '/admin/faq-groups' },
        { name: 'لیست سوالات متداول', url: '/admin/faq-list' },
        { name: 'لیست صفحات', url: '/admin/pages' }, 
        { name: 'مدیریت منو', url: '/admin/menu-management' },
        { name: 'اسلاید شو', url: '/admin/slide-show' }
      ],
    },

    {
      name: 'تغییر کلمه عبور',
      icon: 'fa fa-key',
      url: '/admin/change-password',
    },
  ];

  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(
      map((result) => result.matches),
      shareReplay()
    );
  constructor(
    private breakpointObserver: BreakpointObserver,
    private authService: AuthService
  ) {}

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
