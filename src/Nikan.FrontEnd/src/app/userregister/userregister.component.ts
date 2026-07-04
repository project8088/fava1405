import { Component, OnInit } from '@angular/core';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-userregister',
  templateUrl: './userregister.component.html',
  styleUrls: ['./userregister.component.scss'],
    standalone: false
})
export class UserRegisterComponent extends AppBase implements OnInit {
  constructor() {
      super();}

  ngOnInit(): void {}
}
