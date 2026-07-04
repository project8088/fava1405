import { Routes } from '@angular/router';
import { AuthGuard } from './core/authentication/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/account/login', pathMatch: 'full' },
  {
    path: 'home',
    component: HomeComponent,
    children: [
      { path: '', component: IndexComponent },
      { path: 'contact-us', component: ContactUsComponent },

      { path: '404', component: NotFoundComponent },

      { path: 'page/:slug', component: MainPageDetailsComponent },

      { path: 'faq', component: FaqListComponent },
      { path: 'ticket', component: TicketComponent },
      { path: 'ticket-answer', component: TicketAnswerComponent },

      {
        path: 'bankcallback',
        component: BankCallBackComponent,
        data: { id: '' },
      },
    ],
  },

  {
    path: 'redirect',
    loadComponent:()=>import('./pages/redirect/redirect.component').then(c=>c.RedirectComponent)
  },

  {
    path: 'citizen',
    loadChildren: () => import('./pages/citizen/citizen.module').then((m) => m.CitizenModule),
    // canActivate: [AuthGuard],
  },

  {
    path: 'account',
    loadChildren: () => import('./account/account.module').then((m) => m.AccountModule),
  },
  {
    path: 'userregister',
    loadChildren: () =>
      import('./userregister/userregister.module').then((m) => m.UserRegisterModule),
  },
  {
    path: 'admin',
    canActivate: [AuthGuard],
    loadChildren: () => import('./pages/admin/admin.module').then((m) => m.AdminModule),
  },

  {
    path: 'webuser',
    canActivate: [AuthGuard],
    loadChildren: () => import('./pages/webuser/webuser.module').then((m) => m.WebUserModule),
  },

  {
    path: 'company',
    canActivate: [AuthGuard],
    loadChildren: () => import('./pages/company/company.module').then((m) => m.CompanyModule),
  },

  {
    path: 'card',
    canActivate: [AuthGuard],
    loadChildren: () => import('./pages/card/card.module').then((m) => m.CardModule),
  },

  { path: '**', redirectTo: '/home/404' },
  {
    path: 'user/account',
    loadChildren: () => import('./account/account.module').then((m) => m.AccountModule),
  },
];
