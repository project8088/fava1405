import { Component, OnInit, Input } from '@angular/core';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'main-tree-menu',
  templateUrl: './tree-menu.component.html',
  styleUrls: ['./tree-menu.component.scss'],
    standalone: false
})
export class MainTreeMenuComponent extends AppBase implements OnInit {
  @Input('menuItems') menuItems: any[];
  @Input('hasChild') hasChild: boolean = false;

  constructor() {
      super();}

  ngOnInit() {}
}
