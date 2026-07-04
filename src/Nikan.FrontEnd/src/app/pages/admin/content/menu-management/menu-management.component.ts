import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '../../../../core/server-apis';
import Swal from 'sweetalert2';
import { AdminAddOrUpdateMenuDialogComponent } from './dialog/add-update-menu/add-update-menu.component';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-menu-management',
  templateUrl: './menu-management.component.html',
  styleUrls: ['./menu-management.component.scss'],
})
export class AdminMenuManagementComponent extends AppBase implements OnInit {
  data: any[] = [];
  isLoadingResults: boolean = true;

  constructor(
) {
      super();}

  ngOnInit() {
    this.getList();
  }

  getList() {
    this.isLoadingResults = true;
    this.data = [];
    this.dataService.get(ServerApis.getAllMenuItems, {}).subscribe(
      (response) => {
        this.isLoadingResults = false;
        if (response.isSuccess) {
          var data = response.data ? response.data : [];
          data.sort((a, b) => {
            if (a.tabOrder > b.tabOrder) return 1;
            else if (a.tabOrder < b.tabOrder) return -1;
            else return 0;
          });
          this.data = this.getNestedChildren(data);
          //  console.log(this.data);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.isLoadingResults = false;
      },
    );
  }

  getNestedChildren(arr, parentId = null) {
    var out = [];
    for (var i in arr) {
      if (arr[i].parentId == parentId) {
        var children = this.getNestedChildren(arr, arr[i].id);

        arr[i].children = children;

        out.push(arr[i]);
      }
    }
    return out;
  }

  openMenuDialog(menu) {
    this.matDialog
      .open(AdminAddOrUpdateMenuDialogComponent, {
        data: menu,
        panelClass: 'custom-dialog',
      })
      .afterClosed()
      .subscribe((resp) => {
        if (resp) this.getList();
      });
  }
}
