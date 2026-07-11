import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { SideNavMenuItem } from '@core/models/menuItems';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'menu-dynamic',
  templateUrl: './menu-dynamic.component.html',
  styleUrls: ['./menu-dynamic.component.scss'],
  standalone: false,
})
export class MenuDynamicComponent extends AppBase implements OnInit, OnDestroy {
  @Input('menu') menuItems: SideNavMenuItem[] = [];

  constructor() {
    super();
  }

  ngOnInit(): void {}

  toggle(item: SideNavMenuItem, ev: Event) {
    item.isOpen = !item.isOpen;
    ev.preventDefault();
    ev.stopPropagation();
  }
  stopPagination(ev: Event) {
    ev.preventDefault();
    ev.stopPropagation();
  }

  ngOnDestroy(): void {}

  checkPermission(item: SideNavMenuItem): boolean {
    return this.authService.checkPermission(item.permission);
  }
}
