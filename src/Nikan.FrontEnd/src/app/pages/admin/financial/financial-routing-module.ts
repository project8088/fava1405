import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminTransactionListComponent } from './transaction-list/transaction-list.component';
import { TransactionDetailsComponent } from './transaction-details/transaction-details.component';

const routes: Routes = [
  { path: 'transaction-list', component: AdminTransactionListComponent },
  { path: 'transaction-details/:id', component: TransactionDetailsComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FinancialRoutingModule {}
