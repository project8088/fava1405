import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AccountComponent } from './account.component';
import { AccountRoutingModule } from './account-routing.module';
import { CommonModule } from '@angular/common';
import { CoreModule } from '../core/core.module';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { LayoutsModule } from '../layouts/layouts.module';
import { LoginComponent } from './login/login.component';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatStepperModule } from '@angular/material/stepper';
import { MaterialModule } from '../core/material/material.module';
import { NgModule } from '@angular/core';
import { RegisterCompanyComponent } from './register-company/register-company.component';
import { SharedModule } from '../shared/shared.module';
import { BotDetectCaptchaModule } from './bot-detect/botdetect-captcha.module';

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
    MatSelectModule,
    MatFormFieldModule,
    MatRadioModule,
    MatInputModule,
    MatCheckboxModule,
    MatButtonModule,
    ReactiveFormsModule,
    SharedModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatStepperModule,
    MatDatepickerModule,
    FormsModule,
    MaterialModule,
    LayoutsModule,
    CoreModule,
    BotDetectCaptchaModule,
  ],
})
export class AccountModule {}
