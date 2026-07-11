import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
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
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
