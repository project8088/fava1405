import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminDashboardComponent } from './dashboard/dashboard.component';
import { AdminComponent } from './admin.component';
import { AdminConfigComponent } from './config/config.component';
import { AdminContactUsListComponent } from './contact-list/contact-list.component';
import { AdminSettingComponent } from './setting/setting.component';
import { AdminSmsListComponent } from './sms-list/sms-list.component';
import { SmsSettingComponent } from './sms-setting/sms-setting.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

      { path: 'dashboard', component: AdminDashboardComponent },

      { path: 'contact-us', component: AdminContactUsListComponent },
      { path: 'setting', component: AdminSettingComponent },
      { path: 'sms-setting', component: SmsSettingComponent },

      { path: 'config', component: AdminConfigComponent },
      { path: 'sms-list', component: AdminSmsListComponent },

      {
        path: 'users',
        loadChildren: () => import('./users/users-module').then((m) => m.UsersModule),
      },
      {
        path: 'citizen',
        loadChildren: () => import('./citizens/citizen-module').then((m) => m.CitizenModule),
      },
      {
        path: 'refund',
        loadChildren: () => import('./refund/refund-module').then((m) => m.RefundModule),
      },
      {
        path: 'financial',
        loadChildren: () => import('./financial/financial-module').then((m) => m.FinancialModule),
      },
      {
        path: 'content',
        loadChildren: () => import('./content/content-module').then((m) => m.ContentModule),
      },
      {
        path: 'groups',
        loadChildren: () => import('./groups/groups-module').then((m) => m.GroupsModule),
      },
      {
        path: 'organization',
        loadChildren: () =>
          import('./organization/organization-module').then((m) => m.OrganizationModule),
      },
      {
        path: 'sabte-ahval',
        loadChildren: () =>
          import('./sabte-ahval/sabte-ahval-module').then((m) => m.SabteAhvalModule),
      },
      {
        path: 'manzelat',
        loadChildren: () => import('./manzelat/manzelat-module').then((m) => m.ManzelatModule),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
