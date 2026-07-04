import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { DataService } from '@core/services/data-service.service';
declare var $: any;

@Component({
  selector: 'home-slider',
  templateUrl: './slider.component.html',
  styleUrls: ['./slider.component.scss'],
})
export class SliderComponent implements OnInit, AfterViewInit {
  datetime: string = '';
  slideShow: any[] = [];
  baseUrl: string = ServerApis.baseUrl;

  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    this.dataService.get(ServerApis.getAllMainPageSlideShow, {}).subscribe((response) => {
      if (response.data) this.slideShow = response.data ?? [];
      setTimeout(() => {
        $('#owl-slider').owlCarousel({
          items: 1,
          loop: true,
          autoplay: true,
          nav: true,
          smartSpeed: 2000,
          autoplayTimeout: 3000,
          animateOut: 'fadeOut',
        });
      }, 100);
    });
  }

  ngAfterViewInit() {}
}
