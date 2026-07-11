import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FinancialRoutingModule } from './financial-routing-module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SharedModule } from '@app/shared/shared.module';
import { CoreModule } from '@core/core.module';
import { MaterialModule } from '@core/material/material.module';
import { AdminTransactionListComponent } from './transaction-list/transaction-list.component';
import { TransactionDetailsComponent } from './transaction-details/transaction-details.component';
import { AdminPaySettingComponent } from './pay-setting/pay-setting.component';
import { AdminPayTestComponent } from './pay-test/pay-test.component';

@NgModule({
  declarations: [
    AdminTransactionListComponent,
    TransactionDetailsComponent,
    AdminPayTestComponent,
    AdminPaySettingComponent,
  ],
  imports: [
    CommonModule,
    FinancialRoutingModule,
    MaterialModule,
    CoreModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class FinancialModule {}
