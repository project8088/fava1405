import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminAddOrUpdateFaqComponent } from './add-or-update-faq/add-or-update-faq.component';
import { AdminAddOrUpdateNewsComponent } from './add-or-update-news/add-or-update-news.component';
import { AdminAddOrUpdateNotificationComponent } from './add-or-update-notification/add-or-update-notification.component';
import { AdminAddOrUpdatePageComponent } from './add-or-update-page/add-or-update-page.component';
import { AdminFaqGroupsComponent } from './faq-groups/faq-groups.component';
import { AdminFaqListComponent } from './faq-list/faq-list.component';
import { AdminMenuManagementComponent } from './menu-management/menu-management.component';
import { AdminNewsCommentsComponent } from './news-comments/news-comments.component';
import { AdminNewsGroupsComponent } from './news-groups/news-groups.component';
import { AdminNewsListComponent } from './news-list/news-list.component';
import { AdminNotificationListComponent } from './notification-list/notification-list.component';
import { AdminPageListComponent } from './page-list/page-list.component';
import { AdminSlideShowListComponent } from './slide-show-list/slide-show-list.component';

const routes: Routes = [
  { path: 'news-list', component: AdminNewsListComponent },
  { path: 'addupdate-news/:id', component: AdminAddOrUpdateNewsComponent },
  { path: 'news-comments/:id', component: AdminNewsCommentsComponent },
  { path: 'news-groups', component: AdminNewsGroupsComponent },
  { path: 'pages', component: AdminPageListComponent },
  { path: 'addupdate-page/:id', component: AdminAddOrUpdatePageComponent },
  { path: 'slide-show', component: AdminSlideShowListComponent },

  { path: 'notifications', component: AdminNotificationListComponent },
  { path: 'addupdate-notification/:id', component: AdminAddOrUpdateNotificationComponent },
  { path: 'faq-groups', component: AdminFaqGroupsComponent },
  { path: 'faq-list', component: AdminFaqListComponent },
  { path: 'add-update-faq/:id', component: AdminAddOrUpdateFaqComponent },
  { path: 'menu-management', component: AdminMenuManagementComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ContentRoutingModule {}
