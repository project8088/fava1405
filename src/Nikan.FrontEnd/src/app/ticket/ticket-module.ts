import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TicketRoutingModule } from './ticket-routing-module';
import { SendTicketDialogComponent } from './_dialogs/send-ticket/send-ticket.component';
import { TicketListComponent } from './ticket-list/ticket-list.component';
import { TicketActivityComponent } from './view-ticket/tabs/ticket-activity/ticket-activity.component';
import { TicketCommentsComponent } from './view-ticket/tabs/ticket-comments/ticket-comments.component';
import { TicketResponseComponent } from './view-ticket/tabs/ticket-response/ticket-response.component';
import { TicketDetailsComponent } from './view-ticket/ticket-details.component';
import { MaterialModule } from '@core/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminTicketSubjectsComponent } from './ticket-subjects/ticket-subjects.component';
import { CoreModule } from '@core/core.module';
import { SharedModule } from '@app/shared/shared.module';

@NgModule({
  declarations: [
    AdminTicketSubjectsComponent,
    SendTicketDialogComponent,
    TicketListComponent,
    TicketDetailsComponent,
    TicketResponseComponent,
    TicketActivityComponent,
    TicketCommentsComponent,
  ],
  imports: [
    CommonModule,
    TicketRoutingModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    CoreModule,
    SharedModule,
  ],
})
export class TicketModule {}
