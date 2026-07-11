import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoreModule } from '@core/core.module';
import { MaterialModule } from '@core/material/material.module';
import { SharedModule } from '@app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { WebUserDashboardComponent } from './dashboard/dashboard.component';
import { WebUserRoutingModule } from './webuser-routing.module';
import { WebUserComponent } from './webuser.component';
import { WebUserHelpServiceDetailsComponent } from './help-service/help-service.component';

@NgModule({
  declarations: [WebUserComponent, WebUserDashboardComponent, WebUserHelpServiceDetailsComponent],
  imports: [
    CoreModule,
    CommonModule,
    WebUserRoutingModule,
    MaterialModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class WebUserModule {}
