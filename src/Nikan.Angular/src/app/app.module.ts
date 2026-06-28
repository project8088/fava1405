import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AboutUsComponent } from './pages/home/about-us/about-us.component';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AuthInterceptor } from './core/authentication/auth.interceptor';
import { BankCallBackComponent } from './pages/home/bank-call-back/bank-call-back.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { CompanyPageComponent } from './pages/home/company-page/company-page.component';
import { ContactUsComponent } from './pages/home/contact-us/contact-us.component';
import { CoreModule } from './core/core.module';
import { HomeCompaniesListComponent } from './pages/home/company-list/company-list.component';
import { HomeComponent } from './pages/home/home.component';
import { HomeManagersListComponent } from './pages/home/index/managers/managers.component';
import { HomePersonelsListComponent } from './pages/home/personels/personels.component';
import { HomeServicesListComponent } from './pages/home/index/services/services.component';
import { HomeTopCompaniesListComponent } from './pages/home/index/top-companies/top-companies.component';
import { IndexComponent } from './pages/home/index/index.component';
 
import { LayoutsModule } from './layouts/layouts.module';
 import { MainNewsDetailsComponent } from './pages/home/news-details/news-details.component';
import { MainNewsListComponent } from './pages/home/news-list/news-list.component';
import { MainPageDetailsComponent } from './pages/home/page-details/page-details.component';
import { MainProductDetailsComponent } from './pages/home/product-details/product-details.component';
import { MaterialModule } from './core/material/material.module';
import { NgModule } from '@angular/core';
import { NotFoundComponent } from './pages/home/error/not-found/not-found.component';
import { PersonalBiographyComponent } from './pages/home/personal-biography/personal-biography.component';
import { ProductListComponent } from './pages/home/product-list/product-list.component';
import { RedirectComponent } from './pages/redirect/redirect.component';
import { RouterModule } from '@angular/router';
import { ServiceWorkerModule } from '@angular/service-worker';
import { SharedModule } from './shared/shared.module';
import { SliderComponent } from './pages/home/index/slider/slider.component';
import { TestComponent } from './pages/test/test.component';
import { ToastrModule } from 'ngx-toastr';
import { environment } from '../environments/environment';
import { TicketComponent } from './pages/home/ticket/ticket.component';
import { TicketAnswerComponent } from './pages/home/ticket-answer/ticket-answer.component';
import { FaqListComponent } from './pages/home/faq/faq-list.component';
 
@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    SliderComponent,
    IndexComponent,
    HomeManagersListComponent,
    HomeServicesListComponent,
    HomeTopCompaniesListComponent,
   
    NotFoundComponent,
    MainNewsListComponent,
    MainNewsDetailsComponent,
    ContactUsComponent,
    AboutUsComponent,
    TicketComponent,
    TicketAnswerComponent,
    FaqListComponent,
    CompanyPageComponent,
    MainPageDetailsComponent,
    HomeCompaniesListComponent,
    PersonalBiographyComponent,
    ProductListComponent,
    MainProductDetailsComponent,
    BankCallBackComponent,
    HomePersonelsListComponent, 
    TestComponent,
    RedirectComponent,

  ],
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      closeButton: true,
      progressBar: true,
      timeOut: 10000,
      positionClass: 'toast-bottom-center',
      preventDuplicates: true,
    }),
    HttpClientModule,
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: false }),
    LayoutsModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    CoreModule,
    SharedModule,
    RouterModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
