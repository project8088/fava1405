import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { OWL_OPTIONS } from '../../owal.config';

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

  owlOptions = OWL_OPTIONS;
  constructor() {
    super();
  }

  ngOnInit(): void {
    this.dataService.get(ServerApis.getAllMainPageSlideShow, {}).subscribe((response) => {
      if (response.data) this.slideShow = response.data ?? [];
    });
  }

  ngAfterViewInit() {}
}
