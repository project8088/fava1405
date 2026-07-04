import { AfterViewInit, Component, OnInit } from '@angular/core';
import { AppBase } from "@app/app.base";

declare var $: any;
@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
})
export class AccountComponent extends AppBase implements OnInit, AfterViewInit {
  constructor() {
      super();}

  ngOnInit(): void {}

  ngAfterViewInit() {
    $('#carouselExampleIndicators').carousel({
      wrap: true,
      pause: true,
      interval: 20000,
    });
  }
}
