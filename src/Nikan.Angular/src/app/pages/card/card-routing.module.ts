import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
 
import { ChangeCurrentUserPasswordComponent } from '../../shared/change-current-user-password/change-current-user-password.component';
import { TicketListComponent } from '../../shared/ticket/ticket-list/ticket-list.component';
import { TicketDetailsComponent } from '../../shared/ticket/view-ticket/ticket-details.component';
import { CardComponent } from './card.component';
import { CardDashboardComponent } from './dashboard/dashboard.component';
import { CardCitizenCardAdvancedSearchComponent } from './citizens-cards/citizen-card-advanced-search/citizen-card-advanced-search.component';
import { CardTransactionListComponent } from './financial/transaction-list/transaction-list.component';
import { TransactionDetailsComponent } from '../../shared/transaction-details/transaction-details.component';
import { CardCitizenCardExportSearchComponent } from './citizens-cards/citizen-card-export-search/citizen-card-export-search.component';
import { CardCitizenCardExportDetailsComponent } from './citizens-cards/citizen-card-export-details/citizen-card-export-details.component';
import { CardUsersComponent } from './users/card-users/card-users.component';
import { CardCitizensPicturesComponent } from './card-citizens-pictures/card-citizens-pictures.component';
import { CardCitizenSearchComponent } from './citizens-search/citizens-search.component';
import { CardCitizenCardDistributeCourseListComponent } from './citizens-cards/citizen-card-distribute-course-list/citizen-card-distribute-course-list.component';
import { CardShahrvandiCitizenCardExportDetailsComponent } from './citizens-cards/citizen-shahrvandi-card-export-details/citizen-shahrvandi-card-export-details.component';
import { CardManzalatCitizenCardExportDetailsComponent } from './citizens-cards/citizen-manzalat-card-export-details/citizen-manzalat-card-export-details.component';
import { CardCardCourseQueuelistComponent } from './citizens-cards/card-course-queue-list/card-course-queue-list.component';
import { CardCitizenCardSearchInQueueComponent } from './citizens-cards/citizen-card-search-in-queue/citizen-card-search-in-queue.component';
import { CardCitizenCardInQueueComponent } from './citizens-cards/citizen-card-in-queue/citizen-card-in-queue.component';
import { CardCarddistributionComponent } from './citizens-cards/card-distribution/card-distribution.component';
import { CardUserPermissionsComponent } from './users/card-user-permissions/card-user-permissions.component';
import { OrderCardListComponent } from './store/card-list/card-list.component';
import { OrderAddOrUpdateCardComponent } from './store/add-or-update-card/add-or-update-card.component';
import { CardDiscountListComponent } from './store/card-discount-list/card-discount-list.component';
import { AppShowCitizenComponent } from '../../shared/citizens/show-citizens/show-citizen.component';
import { CardFreeRequestCardListComponent } from './free-card/free-request-list/free-request-list.component';
import { CardAddOrUpdateFreeCardComponent } from './free-card/add-or-update-free-card/add-or-update-free-card.component';
import { CardFreeCardCitizensComponent } from './free-card/free-card-citizens/free-card-citizens.component';
 


const routes: Routes = [
  {
    path: '', component: CardComponent, children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: CardDashboardComponent }, 
      { path: 'tickets', component: TicketListComponent },
      { path: 'advanced-search-card-citizen', component: CardCitizenCardAdvancedSearchComponent },
      { path: 'transaction-list', component: CardTransactionListComponent },
      { path: 'card-users', component: CardUsersComponent },
      { path: 'transaction-details/:id', component: TransactionDetailsComponent },
      { path: 'ticket-details/:id', component: TicketDetailsComponent },
      { path: 'card-citizens-pictures', component: CardCitizensPicturesComponent },
      { path: 'export-card-citizen', component: CardCitizenCardExportSearchComponent },
      { path: 'export-details-citizen-card/:id', component: CardCitizenCardExportDetailsComponent },
      { path: 'export-details-citizen-card-for-print/:id', component: CardShahrvandiCitizenCardExportDetailsComponent },
      { path: 'export-details-manzalat-card-for-print/:id', component: CardManzalatCitizenCardExportDetailsComponent },
      { path: 'card-course-queue-list/:id', component: CardCardCourseQueuelistComponent },


      { path: 'change-password', component: ChangeCurrentUserPasswordComponent },
      { path: 'search-citizen', component: CardCitizenSearchComponent },
      { path: 'card-distribute-course-list', component: CardCitizenCardDistributeCourseListComponent },

      { path: 'citizen-card-search-in-queue', component: CardCitizenCardSearchInQueueComponent },
      { path: 'citizen-card-in-queue/:id', component: CardCitizenCardInQueueComponent },
      { path: 'card-distribution/:id', component: CardCarddistributionComponent },



      { path: 'order-card-list', component: OrderCardListComponent },
      { path: 'order-addupdate-card/:cardTypeId/:id', component: OrderAddOrUpdateCardComponent },
      { path: 'card-discount-list/:id', component: CardDiscountListComponent },


      { path: 'show-citizen/:id', component: AppShowCitizenComponent },



      { path: 'free-request-card-list', component: CardFreeRequestCardListComponent },
      { path: 'free-request-addupdate/:id', component: CardAddOrUpdateFreeCardComponent },
      { path: 'free-request-citizens/:id', component: CardFreeCardCitizensComponent },




      { path: 'card-permissions/:id', component: CardUserPermissionsComponent },

    ]
  }
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CardRoutingModule { }
