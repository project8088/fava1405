import { Component, OnInit, Input, ElementRef, OnDestroy } from '@angular/core';
import { AuthService } from 'src/app/core/authentication/auth.service';
import { SideNavMenuItem } from '../../../core/models/menuItems';

@Component({
  selector: 'menu-dynamic',
  templateUrl: './menu-dynamic.component.html',
  styleUrls: ['./menu-dynamic.component.scss']
})
export class MenuDynamicComponent implements OnInit, OnDestroy {
  @Input('menu') menuItems: SideNavMenuItem[];


  constructor(
    private authService: AuthService
  ) {
  }


  ngOnInit(): void {
  }


  toggle(item: SideNavMenuItem, ev: Event) {
    item.isOpen = !item.isOpen;
    ev.preventDefault();
    ev.stopPropagation();
  }
  stopPagination(ev: Event) {
    ev.preventDefault();
    ev.stopPropagation();
  }

  ngOnDestroy(): void {
  }





  checkPermission(item: SideNavMenuItem): boolean {
    return this.authService.checkPermission(item.permission);
  }

}
