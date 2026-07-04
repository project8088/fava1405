import { Component, OnInit } from '@angular/core';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-about-us',
  templateUrl: './about-us.component.html',
  styleUrls: ['./about-us.component.scss'],
  standalone: false,
})
export class AboutUsComponent extends AppBase implements OnInit {
  constructor() {
    super();
  }

  ngOnInit(): void {}
}
