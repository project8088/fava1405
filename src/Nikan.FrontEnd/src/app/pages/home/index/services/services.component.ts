import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';

import { DataService } from '../../../../core/services/data-service.service';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { ServerApis } from '../../../../core/server-apis';
import { ToastrService } from 'ngx-toastr';

declare var $: any;

@Component({
  selector: 'home-services',
  templateUrl: './services.component.html',
  styleUrls: ['./services.component.scss'],
})
export class HomeServicesListComponent implements OnInit {
  data: any[] = [];
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  baseUrl: string = ServerApis.baseUrl;

  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private router: Router,
  ) {}

  ngOnInit() {
    this.getList();
  }

  getList() {
    this.isLoadingResults = true;
    return this.dataService.get(ServerApis.getAppRegisterList).subscribe(
      (response) => {
        this.isLoadingResults = false;
        if (response.isSuccess && response.data) {
          this.data = response.data ? response.data : [];
          setTimeout(() => {
            this.owlService();
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

  owlService() {
    $('#owl-Service3').owlCarousel({
      rtl: true,
      loop: true,
      nav: true,
      autoplay: true,
      autoplayHoverPause: true,
      smartSpeed: 1000,
      autoplayTimeout: 8000,
      responsiveClass: true,
      responsive: {
        0: {
          items: 1,
        },
        480: {
          items: 2,
        },
        768: {
          items: 3,
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
        //992: {
        //    items: 5,
        //},
        1200: {
          items: 5,
        },
        1367: {
          items: 5,
        },
      },
    });
  }
}
