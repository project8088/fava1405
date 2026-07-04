import { Component, OnInit } from '@angular/core';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss'],
})
export class CitizenNotificationsComponent extends AppBase implements OnInit {
  constructor() {
      super();}

  ngOnInit(): void {}
}
