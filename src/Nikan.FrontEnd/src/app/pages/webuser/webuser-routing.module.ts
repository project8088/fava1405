import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChangeCurrentUserPasswordComponent } from '@app/shared/change-current-user-password/change-current-user-password.component';
import { WebUserComponent } from './webuser.component';
import { WebUserDashboardComponent } from './dashboard/dashboard.component';
import { WebUserHelpServiceDetailsComponent } from './help-service/help-service.component';

const routes: Routes = [
  {
    path: '',
    component: WebUserComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: WebUserDashboardComponent },
      { path: 'help-details/:id', component: WebUserHelpServiceDetailsComponent },
      { path: 'change-password', component: ChangeCurrentUserPasswordComponent },
    ],
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WebUserRoutingModule {}
