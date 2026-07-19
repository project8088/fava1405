import { SideNavMenuItem } from '@core/models/menuItems';




export const CITIZEN_MENU: SideNavMenuItem[] = [
  { name: 'پیشخوان', icon: 'fa fa-bar-chart', url: '/citizen/dashboard' },
  { name: 'ویرایش اطلاعات من', icon: 'fa fa-user', url: '/citizen/profile/0' },
  { name: 'طرح منزلت', icon: 'fa fa-star', url: '/citizen/manzelat-plan' },
  { name: 'کارت شهروندی', icon: 'fa fa-star', url: '/citizen/citizen-card' },
  { name: 'تراکنش های مالی', url: '/citizen/transaction-list', icon: 'fa fa-list' },
  { name: 'برگشت هزینه(آموزش)', url: '/citizen/citizen-refund-status', icon: 'fa fa-list' },
  { name: 'تغییر کلمه عبور', icon: 'fa fa-key', url: '/citizen/change-password' },
  { name: ' تماس با کارشناسان ', url: '/home/page/تماس_با_کارشناسان', icon: 'fa fa-envelope' },
  {
    name: 'خدمات فرهنگی و ورزشی کارت منزلت ',
    url: '/home/page/خدمات_فرهنگی_ورزشی_کارت_منزلت',
    icon: 'fa fa-envelope',
  },
];
