import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AdminAddOrUpdateMenuDialogComponent } from './dialog/add-update-menu/add-update-menu.component';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'adm-menu-management',
  templateUrl: './menu-management.component.html',
  styleUrls: ['./menu-management.component.scss'],
  standalone: false,
})
export class AdminMenuManagementComponent extends AppBase implements OnInit {
  data: any[] = [];
  isLoadingResults: boolean = true;

  constructor() {
    super();
  }

  ngOnInit() {
    this.getList();
  }

  getList() {
    this.isLoadingResults = true;
    this.data = [];
    this.dataService.get(ServerApis.getAllMenuItems, {})
      .pipe(
        finalize(() => {
          this.isLoadingResults = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response.isSuccess) {
                var data = response.data ? response.data : [];
                data.sort((a: any, b: any) => {
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
            }, (error: any) => {
            });
  }

  getNestedChildren(arr: any[], parentId = null) {
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

  openMenuDialog(menu: any) {
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
