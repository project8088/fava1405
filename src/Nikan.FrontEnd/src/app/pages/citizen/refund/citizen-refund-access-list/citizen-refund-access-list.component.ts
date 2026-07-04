import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DataService } from '../../../../core/services/data-service.service';
import { ToastrService } from 'ngx-toastr';
import { ServerApis } from '../../../../core/server-apis';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { merge, of as observableOf } from 'rxjs';
import { switchMap, startWith, map, catchError } from 'rxjs/operators';
import { AdminReportRefundDialogComponent } from '../../../admin/Refund/dialog/report-refund/report-refund.component';

@Component({
  selector: 'app-citizen-refund-access-list',
  templateUrl: './citizen-refund-access-list.component.html',
  styleUrls: ['./citizen-refund-access-list.component.scss'],
})
export class CitizenRefundAccessListComponent implements AfterViewInit, OnInit {
  displayedColumns: string[] = [
    'row',
    'letterNumber',
    'unitName',
    'className',
    'accessByCitizen',
    'onDate',
    'citizenAccess',
    'isClosed',
    'count',
    'countRefund',
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
    private dataService: DataService,
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private matDialog: MatDialog,
    private router: Router,
  ) {
    this.searchForm = this.fb.group({
      letterNumber: [''],
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

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.getRefundCitizenAccessPageList, param);
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

  openReportDialog(item) {
    this.matDialog
      .open(AdminReportRefundDialogComponent, {
        panelClass: 'custom-dialog',
        data: {
          id: item.id,
        },
        width: '600px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getList();
      });
  }
}
