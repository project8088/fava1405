import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminDashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

  { path: 'dashboard', component: AdminDashboardComponent },

  {
    path: 'user',
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
    loadChildren: () => import('./sabte-ahval/sabte-ahval-module').then((m) => m.SabteAhvalModule),
  },
  {
    path: 'manzelat',
    loadChildren: () => import('./manzelat/manzelat-module').then((m) => m.ManzelatModule),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
