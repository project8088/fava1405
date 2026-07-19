import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home.component';
import { BankCallBackComponent } from './bank-call-back/bank-call-back.component';
import { ContactUsComponent } from './contact-us/contact-us.component';
import { FaqListComponent } from './faq/faq-list.component';
import { IndexComponent } from './index/index.component';
import { MainPageDetailsComponent } from './page-details/page-details.component';
import { TicketAnswerComponent } from './ticket-answer/ticket-answer.component';
import { TicketComponent } from './ticket/ticket.component';
import { CompanyPageComponent } from './company-page/company-page.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    children: [
      { path: '', component: IndexComponent },
      { path: 'contact-us', component: ContactUsComponent },
      { path: 'page/:slug', component: MainPageDetailsComponent },
      { path: 'faq', component: FaqListComponent },
      { path: 'ticket', component: TicketComponent },
      { path: 'ticket-answer', component: TicketAnswerComponent },
      { path: 'company/:id', component: CompanyPageComponent },
      {
        path: 'bankcallback',
        component: BankCallBackComponent,
        data: { id: '' },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HomeRoutingModule {}
