import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LayoutsModule } from '../layouts/layouts.module';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatStepperModule } from '@angular/material/stepper';
import { MaterialModule } from '../core/material/material.module';
import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { TermsComponent } from './terms/terms.component';
import { UserRegisterComponent } from './userregister.component';
import { UserRegisterRoutingModule } from './userregister-routing.module';
import { RegisterComponent } from './register/register.component';
import { PreregisterComponent } from './preregister/preregister.component';
import { CoreModule } from '../core/core.module';
import { BotDetectCaptchaModule } from '../account/bot-detect/botdetect-captcha.module';

@NgModule({
  declarations: [PreregisterComponent, RegisterComponent, TermsComponent, UserRegisterComponent],
  imports: [
    CommonModule,
    CoreModule,
    UserRegisterRoutingModule,
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
    BotDetectCaptchaModule,
  ],
})
export class UserRegisterModule {}
