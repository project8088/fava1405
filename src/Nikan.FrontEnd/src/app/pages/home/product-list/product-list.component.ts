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
import Swal from 'sweetalert2';
import { DataService } from '../../../core/services/data-service.service';
import { ServerApis } from '../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
@Component({
  selector: 'main-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss'],
})
export class ProductListComponent implements AfterViewInit, OnInit {
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;

  groupList: any[] = [];
  baseUrl: string = ServerApis.baseUrl;
  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private router: Router,
    private fb: FormBuilder,
    private customValidator: CustomFormValidators,
  ) {
    this.searchForm = this.fb.group({
      parentId: [null],
      title: [null],
    });
  }

  ngOnInit() {
    this.dataService.get(ServerApis.getProductParentGroups, {}).subscribe((response) => {
      if (response.isSuccess) this.groupList = response.data ? response.data : [];
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
          return this.dataService.get(ServerApis.getAllProductForView, param);
        }),
        map((response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.products ? response.data.products : [];
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
          this.toastrService.error('دريافت اطلاعات از سرور با خطا مواجه شده است!');
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

  openProductByParent(item) {
    if (this.paginator) {
      this.paginator.firstPage();
    }
    this.searchForm.get('parentId').setValue(item.key);
    this.getList();
  }
}
