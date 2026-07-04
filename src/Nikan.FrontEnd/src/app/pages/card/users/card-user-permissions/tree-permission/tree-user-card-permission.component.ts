import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
import { DataService } from '../../../../../core/services/data-service.service';
import { ToastrService } from 'ngx-toastr';
import { ServerApis } from '../../../../../core/server-apis';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';

@Component({
  selector: 'crd-tree-user-card-permission',
  templateUrl: './tree-user-card-permission.component.html',
  styleUrls: ['./tree-user-card-permission.component.scss'],
})
export class CardTreeCardPermissionComponent implements OnInit {
  @Input('permissionItems') permissionItems: any;

  @Output() onUpdate = new EventEmitter<boolean>();

  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private matDialog: MatDialog,
  ) {}

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
