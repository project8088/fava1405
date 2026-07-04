import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'crd-tree-user-card-permission',
  templateUrl: './tree-user-card-permission.component.html',
  styleUrls: ['./tree-user-card-permission.component.scss'],
    standalone: false
})
export class CardTreeCardPermissionComponent extends AppBase implements OnInit {
  @Input('permissionItems') permissionItems: any;

  @Output() onUpdate = new EventEmitter<boolean>();

  constructor(
) {
      super();}

  ngOnInit() {}

  updateAllComplete(item) {
    item.allComplete = item.value != null && item.value.every((t) => t.selected);
  }

  someComplete(item): boolean {
    if (!item.value) {
      return false;
    }
    return item.value.filter((t) => t.selected).length > 0 && !item.allComplete;
  }

  allComplete(item): boolean {
    return (item.allComplete = item.value != null && item.value.every((t) => t.selected));
  }

  setAll(item, selected: boolean) {
    if (!item.value) {
      return;
    }
    item.value.forEach((t) => (t.selected = selected));
  }
}
