import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: '/account/login', pathMatch: 'full' },
  {
    path: 'home',
    loadChildren: () => import('./pages/home/home-module').then((m) => m.HomeModule),
  },

  // {
  //   path: 'redirect',
  //   loadComponent: () =>
  //     import('./pages/redirect/redirect.component').then((c) => c.RedirectComponent),
  // },

  // {
  //   path: 'citizen',
  //   loadChildren: () => import('./pages/citizen/citizen.module').then((m) => m.CitizenModule),
  //   // canActivate: [AuthGuard],
  // },

  {
    path: 'account',
    loadChildren: () => import('./account/account.module').then((m) => m.AccountModule),
  },
  // {
  //   path: 'userregister',
  //   loadChildren: () =>
  //     import('./userregister/userregister.module').then((m) => m.UserRegisterModule),
  // },
  // {
  //   path: 'admin',
  //   canActivate: [AuthGuard],
  //   loadChildren: () => import('./pages/admin/admin.module').then((m) => m.AdminModule),
  // },

  // {
  //   path: 'webuser',
  //   canActivate: [AuthGuard],
  //   loadChildren: () => import('./pages/webuser/webuser.module').then((m) => m.WebUserModule),
  // },

  // {
  //   path: 'company',
  //   canActivate: [AuthGuard],
  //   loadChildren: () => import('./pages/company/company.module').then((m) => m.CompanyModule),
  // },

  // {
  //   path: 'card',
  //   canActivate: [AuthGuard],
  //   loadChildren: () => import('./pages/card/card.module').then((m) => m.CardModule),
  // },

  // { path: '**', redirectTo: '/home/404' },
  // {
  //   path: 'user/account',
  //   loadChildren: () => import('./account/account.module').then((m) => m.AccountModule),
  // },
];
