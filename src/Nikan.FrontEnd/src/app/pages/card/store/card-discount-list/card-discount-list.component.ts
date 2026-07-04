import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { ServerApis } from '../../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { CardAddCardDiscountDialogComponent } from '../dialog/add-card-discount/add-card-discount.component';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'card-card-discount-list',
  templateUrl: './card-discount-list.component.html',
  styleUrls: ['./card-discount-list.component.scss'],
  standalone: false,
})
export class CardDiscountListComponent extends AppBase implements OnInit, AfterViewInit {
    loading?: boolean;
  displayedColumns: string[] = [
    'row',
    'discountTitle',
    'cardType',
    'creationDate',
    'discountIsActive',
    'byUser',
    'discountPercent',
    'postalPercentInCity',
    'startDate',
    'endDate',
    'operation',
  ];

  baseUrl: string = ServerApis.baseUrl;
  cardTypeId: string;

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;
  events: any[] = [];
  constructor() {
    super();
    this.route.params.subscribe((p) => {
      this.cardTypeId = p['id'];
    });

    this.searchForm = this.fb.group({});
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.getList();
  }

  getList() {
    var param: any = this.searchForm.value;
    param.offset = this.paginator ? this.paginator.pageIndex : 0;
    param.count = this.paginator ? this.paginator.pageSize : 10;
    param.cardTypeId = +this.cardTypeId;
    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.pagedDiscountCardList, param);
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

  openDialog(item) {
    this.matDialog
      .open(CardAddCardDiscountDialogComponent, {
        panelClass: 'custom-dialog',
        minWidth: '600px',
        data: {
          id: item.id ? item.id : '',
          cardTypeId: +this.cardTypeId,
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          this.getList();
        }
      });
  }
}
