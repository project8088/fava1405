import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { OnlyDecimalDirective, OnlyNumberDirective } from './directive/numbers-only';

import { ArrayRandomizePipe } from './pipe/randomize.pipe';
import { ArraySortPipe } from './pipe/array-sort.pipe';
import { AuthInterceptor } from './authentication/auth.interceptor';
import { CarLicensePlateComponent } from './public-component/car-license-plate/car-license-plate.component';
import { CommonModule } from '@angular/common';
import { CustomFormValidators } from './custom-validator/form-validation';
import { CustomModelValidators } from './custom-validator/model-validator';
import { EnumStringPipe } from './pipe/enumString.pipe';
import { ErrorInterceptor } from './authentication/error.interceptor';
import { InputAutoCompleteComponent } from './public-component/input-auto-complete/input-auto-complete.component';
import { InputCompanyAutoCompleteComponent } from './public-component/input-company/input-company.component';
import { MaterialModule } from './material/material.module';
import { OnlyPersianCharacterDirective } from './directive/persian-only';
import { SafePipe } from './pipe/safehtml';
import { TomanPipe } from './pipe/toman.pipe';
import { SimpleJalaliDatepickerComponent } from './public-component/simple-datepicker/simple-datepicker.component';
import { InputRefundUsersAutoCompleteComponent } from './public-component/input-refund-users/input-refund-users.component';
import { LuxonFormatPipe } from './pipe/luxon-format.pipe';
import { LuxonFromNowPipe } from './pipe/luxon-from-now.pipe';
import { ExportToExcelDirective } from './directive/export-to-excel.directive';

@NgModule({
  declarations: [
    LuxonFormatPipe,
    LuxonFromNowPipe,
    TomanPipe,
    ArraySortPipe,
    ArrayRandomizePipe,
    SafePipe,
    EnumStringPipe,
    OnlyNumberDirective,
    OnlyDecimalDirective,
    OnlyPersianCharacterDirective,
    InputAutoCompleteComponent,
    CarLicensePlateComponent,
    SimpleJalaliDatepickerComponent,
    InputCompanyAutoCompleteComponent,
    InputRefundUsersAutoCompleteComponent,
    ExportToExcelDirective
  ],
  imports: [CommonModule, MaterialModule, FormsModule, ReactiveFormsModule],
  providers: [
    CustomFormValidators,
    CustomModelValidators,
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
  exports: [
    LuxonFormatPipe,
    LuxonFromNowPipe,
    MaterialModule,
    TomanPipe,
    ArraySortPipe,
    ArrayRandomizePipe,
    SafePipe,
    EnumStringPipe,
    OnlyNumberDirective,
    OnlyDecimalDirective,
    OnlyPersianCharacterDirective,
    InputAutoCompleteComponent,
    CarLicensePlateComponent,
    SimpleJalaliDatepickerComponent,
    InputCompanyAutoCompleteComponent,
    InputRefundUsersAutoCompleteComponent,
    ExportToExcelDirective
  ],
})
export class CoreModule {}
