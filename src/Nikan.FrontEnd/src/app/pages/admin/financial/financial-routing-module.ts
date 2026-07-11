import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminTransactionListComponent } from './transaction-list/transaction-list.component';
import { TransactionDetailsComponent } from './transaction-details/transaction-details.component';
import { AdminPaySettingComponent } from './pay-setting/pay-setting.component';
import { AdminPayTestComponent } from './pay-test/pay-test.component';

const routes: Routes = [
  { path: 'transaction-list', component: AdminTransactionListComponent },
  { path: 'transaction-details/:id', component: TransactionDetailsComponent },

  { path: 'pay-setting', component: AdminPaySettingComponent },
  { path: 'pay-test', component: AdminPayTestComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FinancialRoutingModule {}
