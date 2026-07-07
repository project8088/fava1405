import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { ServerApis } from '../../../../core/server-apis';
import { AppBase } from '@app/app.base';

declare var $: any;

@Component({
  selector: 'home-top-companies',
  templateUrl: './top-companies.component.html',
  styleUrls: ['./top-companies.component.scss'],
  standalone: false,
})
export class HomeTopCompaniesListComponent extends AppBase implements OnInit {
  data: any[] = [];
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  baseUrl: string = ServerApis.baseUrl;

  constructor() {
    super();
  }

  ngOnInit() {
    this.getList();
  }

  getList() {
    this.isLoadingResults = true;
    return this.dataService.get(ServerApis.getTopCompanies).subscribe(
      (response) => {
        this.isLoadingResults = false;
        if (response.isSuccess && response.data) {
          this.data = response.data ? response.data : [];
          setTimeout(() => {
            this.owlprovince();
          }, 200);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.isLoadingResults = false;
      },
    );
  }

  //popular doctors_index_page
  owlprovince() {
    $('#owl-province').owlCarousel({
      rtl: true,
      loop: true,
      nav: true,
      autoplay: true,
      autoplayHoverPause: true,
      smartSpeed: 2000,
      autoplayTimeout: 8000,
      responsiveClass: true,
      responsive: {
        0: {
          items: 1,
        },
        410: {
          items: 2,
        },
        640: {
          items: 3,
        },
        770: {
          items: 4,
        },
        //600: {
        //    items: 5,
        //},
        //700: {
        //    items: 6,
        //},
        //847: {
        //    items: 7,
        //},
        992: {
          items: 5,
        },
        1098: {
          items: 6,
        },
        1367: {
          items: 6,
        },
      },
    });
  }
}
