import { RouterModule, Routes } from '@angular/router';

import { BoughtCardsComponent } from './bought-cards/bought-cards.component';
import { CardDetailComponent } from './card-detail/card-detail.component';
import { CitizenCardComponent } from './citizen-card.component';
import { CitizenCardListComponent } from './card-list/card-list.component';
import { NgModule } from '@angular/core';

const routes: Routes = [
  {
    path: '',
    component: CitizenCardComponent,
  },

  {
    path: 'bought-cards',
    component: BoughtCardsComponent,
  },
  {
    path: 'available-cards',
    component: CitizenCardListComponent,
  },
  {
    path: 'card-details',
    component: CardDetailComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CitizenCardRoutingModule {}
