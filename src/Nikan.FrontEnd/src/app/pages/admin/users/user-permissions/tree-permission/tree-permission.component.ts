import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-tree-permission',
  templateUrl: './tree-permission.component.html',
  styleUrls: ['./tree-permission.component.scss'],
  standalone: false,
})
export class AdminTreePermissionComponent extends AppBase implements OnInit {
  @Input('permissionItems') permissionItems: any;

  @Output() onUpdate = new EventEmitter<boolean>();

  constructor() {
    super();
  }

  ngOnInit() {}

  updateAllComplete(item:any) {
    item.allComplete = item.value != null && item.value.every((t:any) => t.selected);
  }

  someComplete(item:any): boolean {
    if (!item.value) {
        return ;
    }
    return item.value.filter((t:any) => t.selected).length > 0 && !item.allComplete;
  }

  allComplete(item:any): boolean {
    return (item.allComplete = item.value != null && item.value.every((t:any) => t.selected));
  }

  setAll(item:any, selected: boolean) {
    if (!item.value) {
      return;
    }
    item.value.forEach((t:any) => (t.selected = selected));
  }
}
