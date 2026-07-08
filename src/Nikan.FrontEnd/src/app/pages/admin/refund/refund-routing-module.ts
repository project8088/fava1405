import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminRefundAccessDetailsListComponent } from './refund-access-details-list/refund-access-details-list.component';
import { AdminRefundAccessListComponent } from './refund-access-list/refund-access-list.component';
import { AdminRefundAccessSearchListComponent } from './refund-access-search-list/refund-access-search-list.component';
import { AdminRefundExcelBatchFileDetailsComponent } from './refund-excel-batch-file-details/refund-excel-batch-file-details.component';
import { AdminRefundExcelBatchFileListComponent } from './refund-excel-batch-file/refund-excel-batch-file.component';
import { AdminRefundUsersComponent } from './refund-users/refund-users.component';

const routes: Routes = [
  { path: 'refund-file-list', component: AdminRefundExcelBatchFileListComponent },
  {
    path: 'refund-file-details/:importId',
    component: AdminRefundExcelBatchFileDetailsComponent,
  },
  { path: 'refund-access-list', component: AdminRefundAccessListComponent },
  { path: 'refund-access-details/:importId', component: AdminRefundAccessDetailsListComponent },
  { path: 'search-refund', component: AdminRefundAccessSearchListComponent },

  { path: 'refund-users', component: AdminRefundUsersComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RefundRoutingModule {}
