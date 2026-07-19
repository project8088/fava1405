import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { merge, of as observableOf } from 'rxjs';
import { switchMap, startWith, map, catchError, finalize } from 'rxjs/operators';
import { AppBase } from '@app/app.base';
import { CitizenRefundInfoDialogComponent } from '@app/shared/_dialog/refund-info/refund-info.component';

@Component({
  selector: 'app-citizen-refund-access-details-list',
  templateUrl: './citizen-refund-access-details-list.component.html',
  styleUrls: ['./citizen-refund-access-details-list.component.scss'],
  standalone: false,
})
export class CitizenRefundAccessDetailsListComponent
  extends AppBase
  implements AfterViewInit, OnInit
{
  displayedColumns: string[] = [
    'row',
    'orderId',
    'transactionCode',
    'totalRefundAmount',
    'ownerNationCode',
    'ownerName',
    'ownerMobileNumber',
    'refundCardNumber',
    'refundOnDate',
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
      nationCode: [''],
      refundState: [null],
    });
    this.route.params.subscribe((p) => {
      this.importId = p['importId'];
      this.getList();
    });
  }

  ngOnInit() {}
  ngAfterViewInit() {
    this.getList();
  }

  getList() {
    var param: any = this.searchForm.value;
    param.offset = this.paginator ? this.paginator.pageIndex : 0;
    param.count = this.paginator ? this.paginator.pageSize : 100;
    param.importId = +this.importId;

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.refundAccessDetailsList, param);
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
