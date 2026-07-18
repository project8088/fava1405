import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing-module';
import { AboutUsComponent } from './about-us/about-us.component';
import { BankCallBackComponent } from './bank-call-back/bank-call-back.component';
import { CompanyPageComponent } from './company-page/company-page.component';
import { ContactUsComponent } from './contact-us/contact-us.component';
import { HomeCompaniesListComponent } from './company-list/company-list.component';
import { HomeComponent } from './home.component';
import { HomeManagersListComponent } from './index/managers/managers.component';
import { HomePersonelsListComponent } from './personels/personels.component';
import { HomeServicesListComponent } from './index/services/services.component';
import { HomeTopCompaniesListComponent } from './index/top-companies/top-companies.component';
import { IndexComponent } from './index/index.component';
import { MainNewsDetailsComponent } from './news-details/news-details.component';
import { MainNewsListComponent } from './news-list/news-list.component';
import { MainPageDetailsComponent } from './page-details/page-details.component';
import { MainProductDetailsComponent } from './product-details/product-details.component';
import { PersonalBiographyComponent } from './personal-biography/personal-biography.component';
import { ProductListComponent } from './product-list/product-list.component';
import { SliderComponent } from './index/slider/slider.component';
import { TicketComponent } from './ticket/ticket.component';
import { TicketAnswerComponent } from './ticket-answer/ticket-answer.component';
import { FaqListComponent } from './faq/faq-list.component';
import { MaterialModule } from '@core/material/material.module';
import { SharedModule } from '@app/shared/shared.module';
import { CoreModule } from '@core/core.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LayoutsModule } from '@app/layouts/layouts.module';
import { CarouselModule } from 'ngx-owl-carousel-o';

@NgModule({
  declarations: [
    HomeComponent,
    SliderComponent,
    IndexComponent,
    HomeManagersListComponent,
    HomeServicesListComponent,
    HomeTopCompaniesListComponent,

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
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HomeRoutingModule,
    MaterialModule,
    SharedModule,
    CoreModule,
    LayoutsModule,
    CarouselModule,
  ],
})
export class HomeModule {}
