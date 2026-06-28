import { RouterModule, Routes } from '@angular/router'; 
import { NgModule } from '@angular/core';
import { TermsComponent } from './terms/terms.component';
import { UserRegisterComponent } from './userregister.component';
import { PreregisterComponent } from './preregister/preregister.component';
import { RegisterComponent } from './register/register.component';

const routes: Routes = [
  {
    path: '',
    component: UserRegisterComponent,
    children: [
      { path: '', component: PreregisterComponent }, 
      { path: 'preregister', component: PreregisterComponent },
      { path: 'register', component: RegisterComponent }, 
      { path: 'terms', component: TermsComponent }
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserRegisterRoutingModule {}
