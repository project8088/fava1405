import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { merge, of as observableOf } from 'rxjs';
import { switchMap, startWith, map, catchError, finalize } from 'rxjs/operators';
import { CitizenProfileDialogComponent } from '@app/shared/_dialog/citizen-profile/citizen-profile.component';
import { AdminChangeRefundDialogComponent } from '../dialog/change-refund/change-refund.component';
import { AdminReportRefundDialogComponent } from '../dialog/report-refund/report-refund.component';
import Swal from 'sweetalert2';
import { AppBase } from '@app/app.base';
import { CitizenRefundInfoDialogComponent } from '@app/shared/_dialog/refund-info/refund-info.component';

@Component({
  selector: 'adm-refund-access-search-list',
  templateUrl: './refund-access-search-list.component.html',
  styleUrls: ['./refund-access-search-list.component.scss'],
  standalone: false,
})
export class AdminRefundAccessSearchListComponent extends AppBase implements AfterViewInit, OnInit {
  displayedColumns: string[] = [
    'row',
    'letterNumber',
    'orderId',
    'transactionCode',
    'totalRefundAmount',
    'ownerName',
    'refundCardNumber',
    'refundOnDate',
    'refundByUser',
    'isClosed',
    'refundState',
    'operation',
  ];
  importId: string = '';

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  searchForm: FormGroup;
  transactionStateList: any[] = [];
  transactionForList: any[] = [];
  constructor() {
    super();
    this.searchForm = this.fb.group({
      importId: [0],
      fromDate: [null],
      toDate: [null],
      transactionCode: [''],
      orderId: [''],
      unitName: [''],
      name: [''],
      nationCode: [''],
      refundState: [null],
    });
  }

  ngOnInit() {}
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
          return this.dataService.get(ServerApis.allRefundAccessPagesList, param);
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

  openChangeRefundDialog(item: any) {
    this.matDialog
      .open(AdminChangeRefundDialogComponent, {
        panelClass: 'custom-dialog',
        data: {
          info: item,
        },
        width: '600px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getList();
      });
  }
  openReportDialog() {
    this.matDialog
      .open(AdminReportRefundDialogComponent, {
        panelClass: 'custom-dialog',
        data: {
          id: 0, //all
        },
        width: '600px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getList();
      });
  }
  getCardNumber(row: any) {
    Swal.fire({
      title: 'تائید',
      text: 'آیا برای استعلام اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService
          .get(ServerApis.getCardNumber, {
            refundId: row.refundId,
          })
          .subscribe((response) => {
            if (response.isSuccess) {
              this.toastrService.success(response.messages);
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

  openRefundInfoDialog(item: any) {
    this.matDialog
      .open(CitizenRefundInfoDialogComponent, {
        panelClass: 'custom-dialog',
        data: {
          info: item,
        },
        width: '600px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getList();
      });
  }
}
