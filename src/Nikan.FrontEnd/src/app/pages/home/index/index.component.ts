import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { NewsDto } from '../../../core/models/news';
import { DataService } from '../../../core/services/data-service.service';
import { ServerApis } from '../../../core/server-apis';
import { SiteSettingViewModel } from '../../../core/models/setting';
declare var $: any;

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit, AfterViewInit, OnDestroy {
  lastNews: NewsDto[] = [];
  loadingNews: boolean;
  baseUrl: string = ServerApis.baseUrl;

  setting: SiteSettingViewModel;

  constructor(
    private dataService: DataService
  ) { }

  ngOnInit(): void {
    this.getLastNews();

    this.dataService.getSetting().subscribe(response => {
      this.setting = response;

    });


  }
  ngOnDestroy() {
    $(window).off('scroll');
  }


  ngAfterViewInit() {
     
  }






  getLastNews() {
    this.loadingNews = true;
    this.dataService.get(ServerApis.getLastNews, {}).subscribe(response => {
      this.loadingNews = false;
      this.lastNews = response.data ? response.data : [];
      setTimeout(() => {
        this.owltopnews();
      }, 200);
    }, error => {
      this.loadingNews = false;
    });
  }





  //popular doctors_index_page
  owltopnews() {
    $('#owl-topnews').owlCarousel({
      rtl: true,
      loop: true,
      nav: true,
      autoplay: true,
      autoplayHoverPause: true,
      smartSpeed: 2000,
      autoplayTimeout: 5000,
      responsiveClass: true,
      responsive: {
        0: {
            items: 1,
        },
        510: {
            items: 2,
        },
        830: {
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
            items: 4
        }
          , 1367: {
              items: 5,
          }
    }
    });
  }










}
