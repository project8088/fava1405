import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DataService } from '../../../../core/services/data-service.service';
import { ToastrService } from 'ngx-toastr';
import { ServerApis } from '../../../../core/server-apis';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { merge, of as observableOf } from 'rxjs';
import { switchMap, startWith, map, catchError } from 'rxjs/operators';
import { CitizenProfileDialogComponent } from '../../../../shared/_dialog/citizen-profile/citizen-profile.component';
import { CitizenRefundInfoDialogComponent } from '../dialog/refund-info/refund-info.component';

@Component({
  selector: 'app-citizen-refund-access-details-list',
  templateUrl: './citizen-refund-access-details-list.component.html',
  styleUrls: ['./citizen-refund-access-details-list.component.scss'],
})
export class CitizenRefundAccessDetailsListComponent implements AfterViewInit, OnInit {
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
  importId: string;

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
    private dataService: DataService,
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private matDialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute,
  ) {
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
      if (p.importId) {
        this.importId = p.importId;
        this.getList();
      }
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

  openRefundInfoDialog(item) {
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
