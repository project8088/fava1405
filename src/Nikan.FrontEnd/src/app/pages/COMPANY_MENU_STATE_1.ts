import { SideNavMenuItem } from '@core/models/menuItems';


export const COMPANY_MENU_STATE_1: SideNavMenuItem[] = [
  { name: 'پیشخوان', icon: 'fa fa-bar-chart', url: '/company/dashboard' },
  { name: 'اطلاعات شرکت', url: '/company/company-profile/0', icon: 'fa fa-briefcase' },

  { name: 'پرسنل', url: '/company/citizenExcelBatchFile-list', icon: 'fa fa-briefcase' },

  { name: 'کاربران', url: '/company/users/0', icon: 'fa fa-users' },
  { name: 'پرسنل شرکت', url: '/company/personal/0', icon: 'fa fa-users' },
  { name: 'تیکت ها', url: '/company/tickets', icon: 'fa fa-envelope' },
  { name: 'تغییر کلمه عبور', icon: 'fa fa-key', url: '/company/change-password' },
];
