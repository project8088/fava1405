import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup } from '@angular/forms';
import { ServerApis } from '../../../../core/server-apis';
import { merge, of as observableOf } from 'rxjs';
import { switchMap, startWith, map, catchError } from 'rxjs/operators';
import Swal from 'sweetalert2';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.scss'],
    standalone: false
})
export class AdminTransactionListComponent extends AppBase implements AfterViewInit, OnInit {
  displayedColumns: string[] = [
    'row',
    'orderId',
    'transactionBankReferenceId',
    'amountTransaction',
    'transactionOnDate',
    'transactionState',
    'transactionFor',
    'acceptationTransactionOnDate',
    'transactionBy',
    'operation',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  searchForm: FormGroup;
  transactionStateList: any[] = [];
  transactionForList: any[] = [];
  constructor(
) {
      super();
    this.searchForm = this.fb.group({
      nationCode: [''],
      fromDate: [null],
      toDate: [null],
      orderId: [''],
      referenceId: [''],
      transactionState: [null],
      transactionFor: [null],
    });
  }

  ngOnInit() {
    this.dataService.getEnums().subscribe((resp) => {
      this.transactionStateList = resp.transactionState ? resp.transactionState : [];
      this.transactionForList = resp.transactionFor ? resp.transactionFor : [];
    });
  }
  ngAfterViewInit() {
    this.getList();
  }

  getList() {
    var param: any = this.searchForm.value;
    param.offset = this.paginator ? this.paginator.pageIndex : 0;
    param.count = this.paginator ? this.paginator.pageSize : 10;

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.getAllTransactions, param);
        }),
        map((response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.transactions ? response.data.transactions : [];
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

  checkTransaction(row) {
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
          .get(ServerApis.checkTransaction, {
            id: row.id,
          })
          .subscribe(
            (response) => {
              if (response.isSuccess) {
                this.toastrService.success(response.messages);
                this.getList();
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            },
            (error) => {},
          );
      }
    });
  }

  createCardForCitizen(row) {
    Swal.fire({
      title: 'تائید',
      text: 'آیا برای بازبینی تراکنش بابت صدور کارت مطمئن هستید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService
          .get(ServerApis.createCardForCitizen, {
            id: row.id,
          })
          .subscribe(
            (response) => {
              if (response.isSuccess) {
                this.toastrService.success(response.messages);
                this.getList();
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            },
            (error) => {},
          );
      }
    });
  }
}
