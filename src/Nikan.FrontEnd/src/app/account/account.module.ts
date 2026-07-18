import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AccountComponent } from './account.component';
import { AccountRoutingModule } from './account-routing.module';
import { CommonModule } from '@angular/common';
import { CoreModule } from '@core/core.module';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { LoginComponent } from './login/login.component';
import { NgModule } from '@angular/core';
import { RegisterCompanyComponent } from './register-company/register-company.component';
import { MaterialModule } from '@core/material/material.module';

@NgModule({
  declarations: [
    LoginComponent,
    ForgotPasswordComponent,
    AccountComponent,
    RegisterCompanyComponent,
  ],
  imports: [
    CommonModule,
    AccountRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    MaterialModule,
    CoreModule,
  ],
})
export class AccountModule {}
