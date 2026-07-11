import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GroupsRoutingModule } from './groups-routing-module';
import { AdminGroupTransferDialogComponent } from './_dialogs/group-transfer/group-transfer.component';
import { AdminAddOrUpdateGroupsComponent } from './add-or-update-groups/add-or-update-groups.component';
import { AdminGroupListComponent } from './groups-list/groups-list.component';
import { AdminImportNationCodeGroupsExcelDialogComponent } from './_dialogs/nationCode-groups-import-excel/import-nationCode-groups-excel.component';
import { AdminNationCodeGroupsExcelBatchFileListComponent } from './nationCode-groups-excel-batch-file/nationCode-groups-excel-batch-file.component';
import { MaterialModule } from '@core/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '@core/core.module';
import { SharedModule } from '@app/shared/shared.module';

@NgModule({
  declarations: [
    AdminGroupListComponent,
    AdminGroupTransferDialogComponent,
    AdminAddOrUpdateGroupsComponent,
    AdminImportNationCodeGroupsExcelDialogComponent,
    AdminNationCodeGroupsExcelBatchFileListComponent,
  ],
  imports: [
    CommonModule,
    GroupsRoutingModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    CoreModule,
    SharedModule,
  ],
})
export class GroupsModule {}
