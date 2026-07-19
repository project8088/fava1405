import { SideNavMenuItem } from '@core/models/menuItems';

export const ADMIN_MENU: SideNavMenuItem[] = [
  { name: 'پیشخوان', icon: 'fa fa-chart-network', url: '/admin/dashboard' },
  {
    name: 'اطلاعات پایه',
    permission: 'basedata',
    icon: 'fa fa-database',
    children: [
      { name: 'گروههای شهروندی', url: '/admin/groups' },
      { name: 'خدمات شهروندی', url: '/admin/citizen/appService-list' },
      { name: 'تنظیمات سامانه', url: '/admin/setting' },
      { name: 'تنظیمات پیامک', url: '/admin/sms-setting' },
    ],
  },

  {
    name: 'مدیریت سازمان ها',
    permission: 'organ',
    icon: 'fa fa-database',
    children: [{ name: 'سازمان ها', url: '/admin/organization' }],
  },
  {
    name: 'مدیریت کاربران',
    permission: 'usermanagement',
    icon: 'fa fa-database',
    children: [
      { name: 'گروه های کاربری', url: '/admin/users/admin-userGroups' },
      { name: 'لیست کاربران', url: '/admin/users/all-users' },
      { name: 'مدیران سامانه', url: '/admin/users/admin-users' },
      { name: 'توسعه دهندگان', url: '/admin/users/web-api-users' },
    ],
  },

  {
    name: 'شهروندان',
    permission: 'citizens',
    icon: 'fa fa-star',
    children: [
      { name: 'جستجوی شهروند', url: '/admin/citizen/search-citizen' },
      { name: 'جستجوی پیشرفته شهروند', url: '/admin/citizen/advanced-search-citizen' },
      { name: 'بازبینی تصاویر', url: '/admin/citizen/citizens-pictures' },
      { name: 'بازخوردهای ثبت شده', url: '/admin/citizen/all-citizens-feedBacks' },
      { name: 'خانواده شهروند', url: '/admin/citizen/search-citizen-family' },
      { name: 'گروههای شهروندی', url: '/admin/groups' },
      { name: 'پیامک های ارسال شده', url: '/admin/sms-list' },
      { name: 'ثبت نام دسته ایی شهروند', url: '/admin/citizen/citizen-register-file-list' },
    ],
  },
  {
    name: 'طرح منزلت',
    permission: 'manzalat',
    icon: 'fa fa-bars',
    children: [
      { name: 'طرح منزلت', url: '/admin/citizen/search-manzelat-citizens' },
      /*   { name: 'تنظیمات منزلت', url: '/admin/manzelat/manzelat-settings' },*/
      { name: 'تنظیمات  ', url: '/admin/manzelat/manzalat-form-list' },
      //ManzalatSettingComponent
    ],
  },

  {
    name: 'مدیریت کارت شهروندی',
    permission: 'card',
    icon: 'fa fa-bars',
    children: [
      { name: 'جستجوی کارت شهروندی', url: '/admin/citizen/advanced-search-card-citizen' },
      { name: 'مشخصات کارت', url: '/admin/card-list' },
      { name: 'خروجی صدور کارت', url: '/admin/citizen/export-search-card-citizen' },
    ],
  },

  {
    name: 'اشخاص حقوقی',
    permission: 'companymanagement',
    icon: 'fa fa-building',
    children: [{ name: 'اشخاص حقوقی', url: '/admin/companies' }],
  },

  {
    name: 'احراز هویت',
    permission: 'sabtahval',
    icon: 'fa fa-star',
    children: [
      { name: 'لیست ورودی و خروجی', url: '/admin/sabte-ahval' },
      { name: 'احراز هویت شهروند', url: '/admin/citizen/citizens-authentication' },
      { name: 'جستجوی استعلام شهروند', url: '/admin/citizen/citizens-authentication-search' },
    ],
  },
  {
    name: 'امور مالی',
    permission: 'financial',
    icon: 'fa fa-coins',
    children: [
      { name: 'تنظیمات مالی', url: '/admin/financial/pay-setting' },
      { name: 'تراکنش های مالی', url: '/admin/financial/transaction-list' },
      { name: 'تست پرداخت', url: '/admin/financial/pay-test' },
    ],
  },

  {
    name: 'استرداد هزینه',
    permission: 'refundmanagement',
    icon: 'fa fa-coins',
    children: [
      { name: 'بارگذاری فایل استرداد', url: '/admin/refund/refund-file-list' },
      { name: 'لیست دسترسی های استرداد', url: '/admin/refund/refund-access-list' },
      { name: 'جستجوی استرداد', url: '/admin/refund/search-refund' },
      { name: 'کاربران استرداد', url: '/admin/refund/refund-users' },
    ],
  },

  {
    name: 'پشتیبانی',
    permission: 'supportmanagement',
    icon: 'fa fa-headphones',
    children: [
      { name: 'اطلاعیه ها', url: '/admin/content/notifications' },
      { name: 'موضوع های تیکت', url: '/admin/tickets/ticket-subjects' },
      { name: 'تیکت ها', url: '/admin/tickets' },
      { name: 'پیام های تماس با ما', url: '/admin/contact-us' },
    ],
  },

  {
    name: 'مدیریت محتوا',
    permission: 'contentmanagement',
    icon: 'fa fa-star',
    children: [
      { name: 'گروه های خبری', url: '/admin/content/news-groups' },
      { name: 'راهنمای توسعه دهندگان', url: '/admin/content/news-list' },
      { name: 'ثبت راهنمای جدید', url: '/admin/content/addupdate-news/0' },
      { name: 'گروه های پرسش و پاسخ', url: '/admin/content/faq-groups' },
      { name: 'لیست سوالات متداول', url: '/admin/content/faq-list' },
      { name: 'لیست صفحات', url: '/admin/content/pages' },
      { name: 'مدیریت منو', url: '/admin/content/menu-management' },
      { name: 'اسلاید شو', url: '/admin/content/slide-show' },
    ],
  },

  {
    name: 'تغییر کلمه عبور',
    icon: 'fa fa-key',
    url: '/admin/users/change-password',
  },
];


