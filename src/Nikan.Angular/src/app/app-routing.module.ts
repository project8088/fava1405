import { RouterModule, Routes } from '@angular/router';

import { AboutUsComponent } from './pages/home/about-us/about-us.component';
import { AuthGuard } from './core/authentication/auth.guard';
import { BankCallBackComponent } from './pages/home/bank-call-back/bank-call-back.component';
import { CompanyPageComponent } from './pages/home/company-page/company-page.component';
import { ContactUsComponent } from './pages/home/contact-us/contact-us.component';
import { HomeCompaniesListComponent } from './pages/home/company-list/company-list.component';
import { HomeComponent } from './pages/home/home.component';
import { IndexComponent } from './pages/home/index/index.component';
 
import { MainNewsDetailsComponent } from './pages/home/news-details/news-details.component';
import { MainNewsListComponent } from './pages/home/news-list/news-list.component';
import { MainPageDetailsComponent } from './pages/home/page-details/page-details.component';
import { MainProductDetailsComponent } from './pages/home/product-details/product-details.component';
import { NgModule } from '@angular/core';
import { NotFoundComponent } from './pages/home/error/not-found/not-found.component';
import { PersonalBiographyComponent } from './pages/home/personal-biography/personal-biography.component';
import { ProductListComponent } from './pages/home/product-list/product-list.component';
import { RedirectComponent } from './pages/redirect/redirect.component';
import { TestComponent } from './pages/test/test.component';
import { FaqListComponent } from './pages/home/faq/faq-list.component';
import { TicketAnswerComponent } from './pages/home/ticket-answer/ticket-answer.component';
import { TicketComponent } from './pages/home/ticket/ticket.component';
 
const routes: Routes = [
  { path: 'test', component: TestComponent },
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
    component: RedirectComponent,
  },

  {
    path: 'citizen',
    loadChildren: () =>
      import('./pages/citizen/citizen.module').then((m) => m.CitizenModule),
    // canActivate: [AuthGuard],
  },

  {
    path: 'account',
    loadChildren: () =>
      import('./account/account.module').then((m) => m.AccountModule),
  },
  {
    path: 'userregister',
    loadChildren: () =>
      import('./userregister/userregister.module').then((m) => m.UserRegisterModule),
  },
  {
    path: 'admin',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./pages/admin/admin.module').then((m) => m.AdminModule),
  },

  {
    path: 'webuser',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./pages/webuser/webuser.module').then((m) => m.WebUserModule),
  },


  {
    path: 'company',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./pages/company/company.module').then((m) => m.CompanyModule),
  },
   
  {
    path: 'card',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./pages/card/card.module').then((m) => m.CardModule),
  },

  { path: '**', redirectTo: '/home/404' },
  {
    path: 'user/account',
    loadChildren: () =>
      import('./account/account.module').then((m) => m.AccountModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
