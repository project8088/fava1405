import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.scss'],
  imports: [RouterLink],
})
export class NotFoundComponent extends AppBase implements OnInit {
  constructor() {
    super();
  }

  ngOnInit(): void {}
}
