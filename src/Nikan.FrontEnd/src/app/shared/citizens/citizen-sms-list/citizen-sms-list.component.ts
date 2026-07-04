import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { AuthService } from '@core/authentication/auth.service';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import Swal from 'sweetalert2';
import { ServerApis } from '../../../core/server-apis';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-citizen-sms-list',
  templateUrl: './citizen-sms-list.component.html',
  styleUrls: ['./citizen-sms-list.component.scss'],
    standalone: false
})
export class AppCitizenSmsListComponent extends AppBase implements OnInit {
  search: string = '';
  paging: any = {};
  userCode: string;
  displayedColumns: string[] = [
    'row',
    'messageText',
    'mobiles',
    'statusText',
    'sender',
    'sendOnDate',
    'cost',
    'userName',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  searchForm: FormGroup;
  constructor(
) {
      super();
    this.route.params.subscribe((p) => {
      this.userCode = p.id ? p.id : null;
    });

    this.searchForm = this.fb.group({
      citizenId: [null],
      fromDate: [null],
      toDate: [null],
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

    param.userCode = this.userCode;

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.getCitizenPagedSmsList, param);
        }),
        map((response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.smsList ? response.data.smsList : [];
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
}
