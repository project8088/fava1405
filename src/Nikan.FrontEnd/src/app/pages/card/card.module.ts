import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardDashboardComponent } from './dashboard/dashboard.component';

import { CoreModule } from '@core/core.module';
import { CardRoutingModule } from './card-routing.module';
import { MaterialModule } from '@core/material/material.module';
import { SharedModule } from '@app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardCitizenCardAdvancedSearchComponent } from './citizens-cards/citizen-card-advanced-search/citizen-card-advanced-search.component';
import { CardTransactionListComponent } from './financial/transaction-list/transaction-list.component';
import { CardImportCardNumberDialogComponent } from './citizens-cards/dialog/import-card-number/import-card-number.component';
import { CardDeliveredCitizenCardDialogComponent } from './citizens-cards/dialog/delivered-citizen-card/delivered-citizen-card.component';
import { CardCancellationCitizenCardDialogComponent } from './citizens-cards/dialog/cancellation-citizen-card/cancellation-citizen-card.component';
import { CardBackCitizenCardDialogComponent } from './citizens-cards/dialog/back-citizen-card/back-citizen-card.component';
import { CardCitizenCardExportSearchComponent } from './citizens-cards/citizen-card-export-search/citizen-card-export-search.component';
import { CardCitizenCardExportDetailsComponent } from './citizens-cards/citizen-card-export-details/citizen-card-export-details.component';
import { CardAddUserDialogComponent } from './users/dialogs/add-user/add-user.component';
import { CardUsersComponent } from './users/card-users/card-users.component';
import { CardCitizensPicturesComponent } from './card-citizens-pictures/card-citizens-pictures.component';
import { CardNewExportCardDialogComponent } from './citizens-cards/dialog/new-export-card/new-export-card.component';
import { CardCitizenSearchComponent } from './citizens-search/citizens-search.component';
import { CardCitizenCardDistributeCourseListComponent } from './citizens-cards/citizen-card-distribute-course-list/citizen-card-distribute-course-list.component';
import { CardShahrvandiCitizenCardExportDetailsComponent } from './citizens-cards/citizen-shahrvandi-card-export-details/citizen-shahrvandi-card-export-details.component';
import { CardManzalatCitizenCardExportDetailsComponent } from './citizens-cards/citizen-manzalat-card-export-details/citizen-manzalat-card-export-details.component';
import { CardAddCardCoursesDialogComponent } from './citizens-cards/dialog/add-card-courses/add-card-courses.component';
import { CardAddOrUpadateQueueDialogComponent } from './citizens-cards/dialog/add-update-queue/add-update-queue.component';
import { CardCardCourseQueuelistComponent } from './citizens-cards/card-course-queue-list/card-course-queue-list.component';
import { CardCitizenCardSearchInQueueComponent } from './citizens-cards/citizen-card-search-in-queue/citizen-card-search-in-queue.component';
import { CardCitizenCardInQueueComponent } from './citizens-cards/citizen-card-in-queue/citizen-card-in-queue.component';
import { CardDeliveryQueueOperatorDialogComponent } from './citizens-cards/dialog/delivery-queue-operator/delivery-queue-operator.component';
import { CardCarddistributionComponent } from './citizens-cards/card-distribution/card-distribution.component';
import { CardUserPermissionsComponent } from './users/card-user-permissions/card-user-permissions.component';
import { CardTreeCardPermissionComponent } from './users/card-user-permissions/tree-permission/tree-user-card-permission.component';
import { OrderCardListComponent } from './store/card-list/card-list.component';
import { OrderAddOrUpdateCardComponent } from './store/add-or-update-card/add-or-update-card.component';
import { CardDiscountListComponent } from './store/card-discount-list/card-discount-list.component';
import { CardAddCardDiscountDialogComponent } from './store/dialog/add-card-discount/add-card-discount.component';
import { CardUpdateCitizenMobileNumberDialogComponent } from './dialog/update-citizen-mobile-number/update-citizen-mobile-number.component';
import { CardFreeRequestCardListComponent } from './free-card/free-request-list/free-request-list.component';
import { CardAddOrUpdateFreeCardComponent } from './free-card/add-or-update-free-card/add-or-update-free-card.component';
import { CardFreeCardCitizensComponent } from './free-card/free-card-citizens/free-card-citizens.component';
import { CardUpdateCitizenSabtStateByCardDialogComponent } from './dialog/update-citizen-sabt-state-by-card/update-citizen-sabt-state-by-card.component';
import { AdminAddOrUpdateCardComponent } from './add-or-update-card/add-or-update-card.component';
import { AdminCardListComponent } from './card-list/card-list.component';

@NgModule({
  declarations: [
    CardDashboardComponent,
    CardCitizenCardAdvancedSearchComponent,
    CardTransactionListComponent,
    CardImportCardNumberDialogComponent,
    CardDeliveredCitizenCardDialogComponent,
    CardCancellationCitizenCardDialogComponent,
    CardBackCitizenCardDialogComponent,
    CardCitizenCardExportSearchComponent,
    CardCitizenCardExportDetailsComponent,
    CardAddUserDialogComponent,
    CardUsersComponent,
    CardCitizensPicturesComponent,
    CardNewExportCardDialogComponent,
    CardCitizenSearchComponent,
    CardCitizenCardDistributeCourseListComponent,
    CardShahrvandiCitizenCardExportDetailsComponent,
    CardManzalatCitizenCardExportDetailsComponent,
    CardAddCardCoursesDialogComponent,
    CardAddOrUpadateQueueDialogComponent,
    CardCardCourseQueuelistComponent,
    CardCitizenCardSearchInQueueComponent,
    CardCitizenCardInQueueComponent,
    CardDeliveryQueueOperatorDialogComponent,
    CardCarddistributionComponent,
    CardUserPermissionsComponent,
    CardTreeCardPermissionComponent,
    OrderCardListComponent,
    OrderAddOrUpdateCardComponent,
    CardDiscountListComponent,
    CardAddCardDiscountDialogComponent,
    CardUpdateCitizenMobileNumberDialogComponent,
    CardFreeRequestCardListComponent,
    CardAddOrUpdateFreeCardComponent,
    CardFreeCardCitizensComponent,
    CardUpdateCitizenSabtStateByCardDialogComponent,
    AdminCardListComponent,
    AdminAddOrUpdateCardComponent,
  ],
  imports: [
    CoreModule,
    CommonModule,
    CardRoutingModule,
    MaterialModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class CardModule {}
