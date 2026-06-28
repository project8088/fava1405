import { AfterViewInit, Component, OnInit } from '@angular/core';
declare var $: any;
@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit, AfterViewInit {
 
  constructor() { }

  ngOnInit(): void {

   }

  ngAfterViewInit() {
    $("#carouselExampleIndicators").carousel({
      wrap: true,
      pause: true,
      interval: 20000
    });
  }


}
