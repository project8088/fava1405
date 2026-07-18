import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import Swal from 'sweetalert2';
import { merge, of as observableOf } from 'rxjs';
import { switchMap, startWith, map, catchError, finalize } from 'rxjs/operators';
import { AdminAddSabtAhvalDialogComponent } from '../dialog/add-sabtAhval/add-sabtAhval.component';
import { CitizenProfileDialogComponent } from '@app/shared/_dialog/citizen-profile/citizen-profile.component';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-sabtAhval-list',
  templateUrl: './sabtAhval-list.component.html',
  styleUrls: ['./sabtAhval-list.component.scss'],
  standalone: false,
})
export class AdminSabtAhvalListComponent extends AppBase implements AfterViewInit {
  displayedColumns: string[] = [
    'row',
    'exportNumber',
    'exportType',
    'exportBy',
    'countRow',
    'creationDate',
    'receiveBy',
    'sendOnDate',
    'receiveOnDate',
    'acceptCount',
    'groupName',
    'operation',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  searchForm: FormGroup;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor() {
    super();
    this.searchForm = this.fb.group({
      fromDate: [null],
      toDate: [null],
      exportNumber: [null],
      title: [''],
    });
  }

  ngAfterViewInit() {
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
          return this.dataService.get(ServerApis.getAllExportSabtAhval, param);
        }),
        finalize(() => {
          this.isLoadingResults = false;
          this.chdr.detectChanges();
        }),
        map((response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.items ? response.data.items : [];
            this.listCount = response.data.totalItems ? response.data.totalItems : 0;
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
        this.dataSource.data = data;
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

  delete(row: any) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا برای حذف اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService
          .get(ServerApis.removeExport, {
            id: row.id,
          })
          .subscribe((response) => {
            if (response.isSuccess) {
              this.toastrService.success('با موفقیت حذف شد.');
              this.getList();
            } else {
              let msg = response.messages
                ? response.messages
                : 'متاسفانه خطایی در سرور رخ داده است!';
              this.toastrService.error(msg);
            }
          });
      }
    });
  }

  openDialog(item: any) {
    this.matDialog
      .open(AdminAddSabtAhvalDialogComponent, {
        panelClass: 'custom-dialog',
        minWidth: '600px',
        data: {},
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          this.getList();
        }
      });
  }

  openCitizenProfile(userCode: string) {
    this.matDialog.open(CitizenProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        userCode: userCode,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }

  send(item: any) {
    Swal.fire({
      title: 'آیا برای ارسال اطلاعات به صف بررسی موافق هستید ؟',
      text: 'این عملیات ممکن است زمان زیادی برای بررسی لازم داشته باشد',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        item.loading = true;
        this.dataService
          .get(ServerApis.sendOnlineAuthentication, {
            exportId: item.id,
          })
          .subscribe(
            (response) => {
              item.loading = false;
              if (response.isSuccess) {
                this.toastrService.success('با موفقیت به صف بررسی ارسال  شد.');
                this.getList();
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            },
            (error: any) => {
              item.loading = false;
            },
          );
      }
    });
  }

  sendChekStateLife(item: any) {
    Swal.fire({
      title: 'آیا برای ارسال اطلاعات به صف بررسی موافق هستید ؟',
      text: 'این عملیات ممکن است زمان زیادی برای بررسی لازم داشته باشد',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        item.loading = true;
        this.dataService
          .get(ServerApis.sendOnlineAuthenticationByBagRezvanService, {
            exportId: item.id,
          })
          .subscribe(
            (response) => {
              item.loading = false;
              if (response.isSuccess) {
                this.toastrService.success('با موفقیت به صف بررسی ارسال  شد.');
                this.getList();
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            },
            (error: any) => {
              item.loading = false;
            },
          );
      }
    });
  }
}
