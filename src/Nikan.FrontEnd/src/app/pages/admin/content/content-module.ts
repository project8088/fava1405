import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContentRoutingModule } from './content-routing-module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { CoreModule } from '@core/core.module';
import { MaterialModule } from '@core/material/material.module';
import { AdminAddOrUpdateFaqComponent } from './add-or-update-faq/add-or-update-faq.component';
import { AdminAddOrUpdateNewsComponent } from './add-or-update-news/add-or-update-news.component';
import { AdminAddOrUpdateNotificationComponent } from './add-or-update-notification/add-or-update-notification.component';
import { AdminAddOrUpdatePageComponent } from './add-or-update-page/add-or-update-page.component';
import { AdminFaqGroupsComponent } from './faq-groups/faq-groups.component';
import { AdminFaqListComponent } from './faq-list/faq-list.component';
import { AdminAddOrUpdateMenuDialogComponent } from './menu-management/dialog/add-update-menu/add-update-menu.component';
import { AdminMenuManagementComponent } from './menu-management/menu-management.component';
import { AdminTreeMenuComponent } from './menu-management/tree-menu/tree-menu.component';
import { AdminNewsCommentsComponent } from './news-comments/news-comments.component';
import { AdminNewsGroupsComponent } from './news-groups/news-groups.component';
import { AdminNewsListComponent } from './news-list/news-list.component';
import { AdminNotificationListComponent } from './notification-list/notification-list.component';
import { AdminPageListComponent } from './page-list/page-list.component';
import { AdminAddOrUpdateSlideShowDialogComponent } from './slide-show-list/dialog/add-update-slide/add-update-slide.component';
import { AdminSlideShowListComponent } from './slide-show-list/slide-show-list.component';
import { ManageAttachmentDialogComponent } from './_dialogs/manage-attachment/manage-attachment.component';
import { HtmlEditorModule } from '@core/public-component/html-editor/html-editor.module';

@NgModule({
  declarations: [
    AdminNotificationListComponent,
    AdminAddOrUpdateNotificationComponent,
    AdminPageListComponent,
    AdminAddOrUpdatePageComponent,
    AdminNewsCommentsComponent,
    AdminFaqListComponent,
    AdminAddOrUpdateFaqComponent,
    AdminFaqGroupsComponent,
    AdminNewsListComponent,
    AdminAddOrUpdateNewsComponent,
    AdminNewsGroupsComponent,
    AdminMenuManagementComponent,
    AdminAddOrUpdateMenuDialogComponent,
    AdminTreeMenuComponent,
    AdminSlideShowListComponent,
    AdminAddOrUpdateSlideShowDialogComponent,
    ManageAttachmentDialogComponent,
  ],
  imports: [
    CommonModule,
    ContentRoutingModule,
    MaterialModule,
    CoreModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    HtmlEditorModule,
  ],
})
export class ContentModule {}
