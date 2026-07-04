import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

declare var $: any;

@Component({
  selector: 'home-slider',
  templateUrl: './slider.component.html',
  styleUrls: ['./slider.component.scss'],
  standalone: false,
})
export class SliderComponent extends AppBase implements OnInit, AfterViewInit {
  datetime: string = '';
  slideShow: any[] = [];
  baseUrl: string = ServerApis.baseUrl;

  constructor() {
    super();
  }

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
