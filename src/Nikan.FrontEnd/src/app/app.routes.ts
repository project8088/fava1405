import { Routes } from '@angular/router';
import { AuthGuard } from '@core/authentication/auth.guard';
export const routes: Routes = [
  { path: '', redirectTo: '/account/login', pathMatch: 'full' },
  {
    path: 'home',
    loadChildren: () => import('./pages/home/home-module').then((m) => m.HomeModule),
  },

  {
    path: 'account',
    loadChildren: () => import('./account/account.module').then((m) => m.AccountModule),
  },
  {
    path: 'redirect',
    loadComponent: () =>
      import('./pages/redirect/redirect.component').then((c) => c.RedirectComponent),
  },
  {
    path: 'userregister',
    loadChildren: () =>
      import('./user-register/userregister.module').then((m) => m.UserRegisterModule),
  },
  {
    path: 'user/account',
    loadChildren: () => import('./account/account.module').then((m) => m.AccountModule),
  },
  {
    path: '',
    loadComponent: () => import('./pages/panel.component').then((c) => c.PanelComponent),
    children: [
      {
        path: 'admin',
        canActivate: [AuthGuard],
        loadChildren: () => import('./pages/admin/admin-module').then((m) => m.AdminModule),
      },
      {
        path: 'citizen',
        loadChildren: () => import('./pages/citizen/citizen.module').then((m) => m.CitizenModule),
        // canActivate: [AuthGuard],
      },
      {
        path: 'company',
        canActivate: [AuthGuard],
        loadChildren: () => import('./pages/company/company.module').then((m) => m.CompanyModule),
      },
      {
        path: 'webuser',
        canActivate: [AuthGuard],
        loadChildren: () => import('./pages/webuser/webuser.module').then((m) => m.WebUserModule),
      },

      {
        path: 'card',
        canActivate: [AuthGuard],
        loadChildren: () => import('./pages/card/card.module').then((m) => m.CardModule),
      },
    ],
  },

  {
    path: '**',
    loadComponent: () => import('./not-found/not-found.component').then((c) => c.NotFoundComponent),
  },
];
