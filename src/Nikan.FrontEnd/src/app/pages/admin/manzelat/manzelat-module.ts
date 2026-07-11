import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ManzelatRoutingModule } from './manzelat-routing-module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '@core/core.module';
import { MaterialModule } from '@core/material/material.module';
import { AdminManzalatBaseFromListComponent } from './manzalat-base-list/manzalat-base-list.component';
import { ManzalatSettingComponent } from './manzalat-setting/manzalat-setting.component';
import { AdminUpdateManzalatBaseFormComponent } from './update-manzalat-base-form/update-manzalat-base-form.component';

@NgModule({
  declarations: [
    ManzalatSettingComponent,
    AdminUpdateManzalatBaseFormComponent,
    AdminManzalatBaseFromListComponent,
  ],
  imports: [
    CommonModule,
    ManzelatRoutingModule,
    CoreModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class ManzelatModule {}
