import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CompanyComponent } from './company.component';
import { CompanyRoutingModule } from './company-routing.module';
import { CoreModule } from '@core/core.module';
import { MaterialModule } from '@core/material/material.module';
import { SharedModule } from '../../shared/shared.module';

import { CompnayDashboardComponent } from './dashboard/dashboard.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CompanyProfileComponent } from './company-profile/company-profile.component';
import { CompanyAddressInfoComponent } from './company-profile/address-info/address-info.component';
import { CompanyMainInfoComponent } from './company-profile/main-info/main-info.component';
import { CompanyBaseInfoComponent } from './company-profile/base-info/base-info.component';

import { CompanyContactUsListComponent } from './contact-list/contact-list.component';
import { CompanyActivityInfoComponent } from './company-profile/activity-info/activity-info.component';
import { ViewMessageDialogComponent } from './contact-list/view-message-dialog/view-message-dialog.component';

import { CompanyPersonalUsersComponent } from './personal/personal-users/personal-users.component';
import { CompanyAddOrUpdatePersonalComponent } from './personal/add-or-update-personal/add-or-update-personal.component';
import { CompanySignatureInfoComponent } from './company-profile/signature-info/signature-info.component';
import { CompanyAdditionalInfoComponent } from './company-profile/additional-info/additional-info.component';
import { CompanyUserListComponent } from './users/user-list/user-list.component';
import { CompanyAddUserDialogComponent } from './users/dialogs/add-user/add-user.component';
import { CompanyUpdateUserDialogComponent } from './users/dialogs/update-user/update-user.component';
import { CompanyChangePasswordDialogComponent } from './users/dialogs/change-user-password/change-user-password.component';
import { CompanyProductGroupsListComponent } from './products/product-groups/product-groups.component';
import { CompanyAddUpdateProductGroupDialogComponent } from './products/_dialogs/add-update-product-group/add-update-product-group.component';
import { CompanyProductListComponent } from './products/product-list/product-list.component';
import { CompanyAddOrUpdateProductComponent } from './products/add-or-update-product/add-or-update-product.component';

import { CompanyTransactionListComponent } from './transaction-list/transaction-list.component';
import { CompanyCitizenExcelBatchFileListComponent } from './citizen-excel-batch-file/citizen-excel-batch-file.component';
import { CompanyImportExcelDialogComponent } from './_dialogs/importPersonel-excel/importPersonel-excel.component';
import { CompanyCitizenExcelBatchFileDetailsComponent } from './citizen-excel-batch-file-details/citizen-excel-batch-file-details.component';

@NgModule({
  declarations: [
    CompanyComponent,
    CompnayDashboardComponent,
    CompanyProfileComponent,
    CompanyAddressInfoComponent,
    CompanyMainInfoComponent,
    CompanyBaseInfoComponent,
    CompanyContactUsListComponent,
    CompanyActivityInfoComponent,
    ViewMessageDialogComponent,

    CompanyPersonalUsersComponent,
    CompanyAddOrUpdatePersonalComponent,
    CompanySignatureInfoComponent,
    CompanyAdditionalInfoComponent,
    CompanyUserListComponent,
    CompanyAddUserDialogComponent,
    CompanyUpdateUserDialogComponent,
    CompanyChangePasswordDialogComponent,
    CompanyProductGroupsListComponent,
    CompanyAddUpdateProductGroupDialogComponent,
    CompanyProductListComponent,
    CompanyAddOrUpdateProductComponent,
    CompanyTransactionListComponent,
    CompanyCitizenExcelBatchFileListComponent,
    CompanyImportExcelDialogComponent,
    CompanyCitizenExcelBatchFileDetailsComponent,
  ],
  imports: [
    CoreModule,
    CommonModule,
    CompanyRoutingModule,
    MaterialModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
  ]
})
export class CompanyModule {}
