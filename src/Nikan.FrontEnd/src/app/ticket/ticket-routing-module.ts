import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminTicketSubjectsComponent } from './ticket-subjects/ticket-subjects.component';
import { TicketListComponent } from './ticket-list/ticket-list.component';
import { TicketDetailsComponent } from './view-ticket/ticket-details.component';

const routes: Routes = [
  { path: '', component: TicketListComponent },
  { path: 'ticket-details/:id', component: TicketDetailsComponent },
  { path: 'ticket-subjects', component: AdminTicketSubjectsComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TicketRoutingModule {}
