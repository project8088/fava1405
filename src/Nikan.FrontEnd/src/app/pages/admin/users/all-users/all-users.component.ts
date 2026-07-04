import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { ServerApis } from '../../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { AdminUpdateUserDialogComponent } from '../dialogs/update-user/update-user.component';
import { AdminChangePasswordDialogComponent } from '../dialogs/change-user-password/change-user-password.component';
import { AdminAddUserDialogComponent } from '../dialogs/add-user/add-user.component';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-all-users',
  templateUrl: './all-users.component.html',
  styleUrls: ['./all-users.component.scss'],
  standalone: false,
})
export class AdminAllUsersComponent extends AppBase implements OnInit {
  displayedColumns: string[] = [
    'row',
    'userName',
    'displayName',
    'emailAddress',
    'mobileNumber',
    'roles',
    'userAccountState',
    'createdOnDate',
    'lastLoggedIn',
    'operation',
  ];

  roleList: any[] = [];
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;

  groupList: any[] = [];

  constructor(private customValidator: CustomFormValidators) {
    super();
    this.searchForm = this.fb.group({
      fromDate: [null],
      toDate: [null],
      username: [''],
      roleId: [null],
      userAccountState: [null],
    });
  }

  ngOnInit() {
    this.dataService.get(ServerApis.getAllRols).subscribe((response) => {
      this.roleList = response.data ? response.data : [];
    });

    this.getList();
  }

  getList() {
    var param: any = this.searchForm.value;
    param.offset = this.paginator ? this.paginator.pageIndex : 0;
    param.count = this.paginator ? this.paginator.pageSize : 10;
    if (param.fromDate) param.fromDate = this.dataService.formatDate(param.fromDate);
    if (param.toDate) param.toDate = this.dataService.formatDate(param.toDate);

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.searchUsers, param);
        }),
        map((response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.items ? response.data.items : [];
            this.listCount = response.data.totalItems ? response.data.totalItems : 0;
            // debugger;
            return items;
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        }),
        catchError((err) => {
          this.isLoadingResults = false;
          return observableOf([]);
        }),
      )
      .subscribe((data) => {
        this.data = data;
      });
  }

  pageEvent(event: PageEvent) {
    this.getList();
  }

  applyFilter() {
    if (this.paginator) {
      this.paginator.firstPage();
    }
    this.getList();
  }

  openAddUserDialog() {
    this.matDialog
      .open(AdminAddUserDialogComponent, {
        panelClass: 'custom-dialog',
        data: {},
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getList();
      });
  }

  openUpdateUserDialog(row) {
    this.matDialog
      .open(AdminUpdateUserDialogComponent, {
        panelClass: 'custom-dialog',
        data: {
          userId: row.userId,
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getList();
      });
  }

  openChangePasswordDialog(row) {
    this.matDialog.open(AdminChangePasswordDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        userId: row.userId,
        userName: row.userName,
        displayName: row.displayName,
      },
    });
  }
}
