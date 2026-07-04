import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BoughtCardsComponent } from './bought-cards/bought-cards.component';
import { CardDetailComponent } from './card-detail/card-detail.component';
import { CitizenCardComponent } from './citizen-card.component';
import { CitizenCardListComponent } from './card-list/card-list.component';
import { CitizenCardRoutingModule } from './citizen-card-routing.module';
import { CommonModule } from '@angular/common';
import { CoreModule } from 'src/app/core/core.module';
import { MaterialModule } from 'src/app/core/material/material.module';
import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    CitizenCardListComponent,
    CitizenCardComponent,
    BoughtCardsComponent,
    CardDetailComponent,
  ],
  imports: [
    CommonModule,
    CitizenCardRoutingModule,
    MaterialModule,
    SharedModule,
    CoreModule,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class CitizenCardModule {}
