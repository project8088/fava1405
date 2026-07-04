import { Component, OnInit } from '@angular/core';
import { ServerApis } from 'src/app/core/server-apis';
import { DataService } from 'src/app/core/services/data-service.service';
declare var $: any;

@Component({
  selector: 'home-personels',
  templateUrl: './personels.component.html',
  styleUrls: ['./personels.component.scss'],
})
export class HomePersonelsListComponent implements OnInit {
  loadingManagers: boolean;
  managerList: any[] = [];
  baseUrl: string = ServerApis.baseUrl;

  constructor(private dataService: DataService) {}

  ngOnInit() {
    this.getManagerList();
  }

  getManagerList() {
    this.loadingManagers = true;
    this.dataService.get(ServerApis.getManagersList, {}).subscribe(
      (response) => {
        this.loadingManagers = false;
        this.managerList = response.data ? response.data : [];
        setTimeout(() => {
          this.owlService();
        }, 200);
      },
      (error) => {
        this.loadingManagers = false;
      },
    );
  }

  owlService() {
    $('#owl-Service2').owlCarousel({
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
