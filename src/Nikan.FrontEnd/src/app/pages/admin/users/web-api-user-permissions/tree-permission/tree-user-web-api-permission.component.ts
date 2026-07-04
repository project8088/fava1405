import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
import { ServerApis } from '../../../../../core/server-apis';
import Swal from 'sweetalert2';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-tree-user-web-api-permission',
  templateUrl: './tree-user-web-api-permission.component.html',
  styleUrls: ['./tree-user-web-api-permission.component.scss'],
})
export class AdminTreeWebApiPermissionComponent extends AppBase implements OnInit {
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
