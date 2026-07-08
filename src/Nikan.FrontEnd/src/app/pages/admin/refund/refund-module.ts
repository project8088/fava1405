import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RefundRoutingModule } from './refund-routing-module';
import { AdminRefundExcelBatchFileDetailsComponent } from './refund-excel-batch-file-details/refund-excel-batch-file-details.component';
import { AdminRefundExcelBatchFileListComponent } from './refund-excel-batch-file/refund-excel-batch-file.component';
import { AdminImportRefundExcelDialogComponent } from './dialog/refund-import-excel/import-refund-excel.component';
import { AdminRefundAccessListComponent } from './refund-access-list/refund-access-list.component';
import { AdminRefundAccessDetailsListComponent } from './refund-access-details-list/refund-access-details-list.component';
import { AdminChangeRefundAccessDialogComponent } from './dialog/change-refund-access/change-refund-access.component';
import { AdminChangeRefundDialogComponent } from './dialog/change-refund/change-refund.component';
import { AdminReportRefundDialogComponent } from './dialog/report-refund/report-refund.component';
import { AdminRefundAccessSearchListComponent } from './refund-access-search-list/refund-access-search-list.component';
import { AdminAddRefundTransactionDialogComponent } from './dialog/add-refund-transaction/add-refund-transaction.component';
import { AdminRefundUsersComponent } from './refund-users/refund-users.component';
import { AdminAddRefundUserDialogComponent } from './dialog/add-refund-user/add-refund-user.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { CoreModule } from '@core/core.module';
import { MaterialModule } from '@core/material/material.module';

@NgModule({
  declarations: [
    AdminRefundUsersComponent,
    AdminAddRefundUserDialogComponent,
    AdminRefundAccessSearchListComponent,
    AdminRefundExcelBatchFileDetailsComponent,
    AdminRefundExcelBatchFileListComponent,
    AdminImportRefundExcelDialogComponent,
    AdminRefundAccessListComponent,
    AdminRefundAccessDetailsListComponent,
    AdminChangeRefundAccessDialogComponent,
    AdminChangeRefundDialogComponent,
    AdminReportRefundDialogComponent,
    AdminAddRefundTransactionDialogComponent,
  ],
  imports: [
    CommonModule,
    RefundRoutingModule,
    MaterialModule,
    CoreModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
  ],
})
export class RefundModule {}
