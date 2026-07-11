import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CompanyComponent } from './company.component';

import { CompnayDashboardComponent } from './dashboard/dashboard.component';
import { CompanyProfileComponent } from './company-profile/company-profile.component';
import { CompanyInfoComponent } from '@app/shared/company-info/company-info.component';

import { ChangeCurrentUserPasswordComponent } from '@app/shared/change-current-user-password/change-current-user-password.component';
import { TicketDetailsComponent } from '@app/shared/ticket/view-ticket/ticket-details.component';
import { TicketListComponent } from '@app/shared/ticket/ticket-list/ticket-list.component';

import { CompanyContactUsListComponent } from './contact-list/contact-list.component';

import { CompanyPersonalUsersComponent } from './personal/personal-users/personal-users.component';
import { CompanyAddOrUpdatePersonalComponent } from './personal/add-or-update-personal/add-or-update-personal.component';
import { CompanyUserListComponent } from './users/user-list/user-list.component';

import { StoreDetailsComponent } from '@app/shared/store-details/store-details.component';
import { AuthGuard } from '@core/authentication/auth.guard';
import { CompanyProductGroupsListComponent } from './products/product-groups/product-groups.component';
import { CompanyProductListComponent } from './products/product-list/product-list.component';
import { CompanyAddOrUpdateProductComponent } from './products/add-or-update-product/add-or-update-product.component';

import { CompanyTransactionListComponent } from './transaction-list/transaction-list.component';
import { TransactionDetailsComponent } from '@app/shared/transaction-details/transaction-details.component';
import { CompanyCitizenExcelBatchFileListComponent } from './citizen-excel-batch-file/citizen-excel-batch-file.component';
import { CompanyCitizenExcelBatchFileDetailsComponent } from './citizen-excel-batch-file-details/citizen-excel-batch-file-details.component';
import { AdminRegisterCompanyComponent } from './register-company/register-company.component';
import { AdminCompaniesListComponent } from './company-list/company-list.component';

const routes: Routes = [
  {
    path: '',
    component: CompanyComponent,
    canActivateChild: [AuthGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'register-company', component: AdminRegisterCompanyComponent },
      { path: 'companies', component: AdminCompaniesListComponent },

      { path: 'dashboard', component: CompnayDashboardComponent },
      { path: 'company-profile/:id', component: CompanyProfileComponent },
      { path: 'company-info/:id', component: CompanyInfoComponent },
      { path: 'tickets', component: TicketListComponent },
      { path: 'ticket-details/:id', component: TicketDetailsComponent },
      { path: 'contact-us', component: CompanyContactUsListComponent },
      { path: 'personal/:id', component: CompanyPersonalUsersComponent },
      { path: 'addupdate-personal/:companyId/:id', component: CompanyAddOrUpdatePersonalComponent },

      { path: 'users/:id', component: CompanyUserListComponent },
      { path: 'change-password', component: ChangeCurrentUserPasswordComponent },
      { path: 'store/:id', component: StoreDetailsComponent },
      { path: 'product-groups', component: CompanyProductGroupsListComponent },
      { path: 'products', component: CompanyProductListComponent },
      { path: 'addupdate-product/:id', component: CompanyAddOrUpdateProductComponent },
      { path: 'transaction-list', component: CompanyTransactionListComponent },
      { path: 'citizenExcelBatchFile-list', component: CompanyCitizenExcelBatchFileListComponent },
      { path: 'transaction-details/:id', component: TransactionDetailsComponent },
      {
        path: 'citizen-excel-files-details/:id',
        component: CompanyCitizenExcelBatchFileDetailsComponent,
      },
    ],
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CompanyRoutingModule {}
