import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CustomFormValidators } from '../../../core/custom-validator/form-validation';
import { AuthService } from 'src/app/core/authentication/auth.service';
import Swal from 'sweetalert2';
import { DataService } from '../../../core/services/data-service.service';
import { ServerApis } from '../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
@Component({
  selector: 'home-company-list',
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.scss'],
})
export class HomeCompaniesListComponent implements AfterViewInit {
  displayedColumns: string[] = [
    'row',
    'companyName',
    'managerName',
    'companyRepresentative',
    'cellNumber',
    'city',
    'operation',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;
  baseUrl: string = ServerApis.baseUrl;
  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private fb: FormBuilder,
  ) {
    this.searchForm = this.fb.group({
      fromDate: [null],
      toDate: [null],
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
          return this.dataService.get(ServerApis.searchCompanies, param);
        }),
        map((response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.companies ? response.data.companies : [];
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
