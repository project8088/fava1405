import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { catchError, finalize, map, startWith, switchMap } from 'rxjs/operators';
import { merge, of as observableOf } from 'rxjs';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-citizen-transaction-list',
  templateUrl: './citizen-transaction-list.component.html',
  styleUrls: ['./citizen-transaction-list.component.scss'],
  standalone: false,
})
export class AdminCitizenTransactionListComponent extends AppBase implements AfterViewInit, OnInit {
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
  citizenId: string = '';

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
    this.route.params.subscribe((p) => {
      this.citizenId = p['id'] ? p['id'] : null;
    });

    this.searchForm = this.fb.group({
      orderId: [''],
      referenceId: [''],
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
    param.citizenId = this.citizenId;

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.getAllCitizenTransactions, param);
        }),
        finalize(() => {
          this.isLoadingResults = false;
          this.chdr.detectChanges();
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
}
