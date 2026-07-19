import { SideNavMenuItem } from '@core/models/menuItems';


export const COMPANY_MENU_STATE_2: SideNavMenuItem[] = [
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


