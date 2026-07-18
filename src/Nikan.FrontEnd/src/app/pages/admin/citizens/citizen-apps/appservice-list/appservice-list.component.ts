import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, of as observableOf } from 'rxjs';
import { catchError, finalize, map, startWith, switchMap } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import Swal from 'sweetalert2';
import { ServerApis } from '@core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-appservice-list',
  templateUrl: './appservice-list.component.html',
  styleUrls: ['./appservice-list.component.scss'],
  standalone: false,
})
export class AdminAppserviceListComponent extends AppBase implements AfterViewInit, OnInit {
  displayedColumns: string[] = [
    'row',
    'serviceId',
    'serviceName',
    'isActive',
    'isLinkService',
    'isMain',
    'operation',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  searchForm: FormGroup;
  constructor(private customValidator: CustomFormValidators) {
    super();
    this.searchForm = this.fb.group({
      serviceName: [''],
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

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.getAllAppService, param);
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

  delete(row: any) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا می خواهید "' + row.serviceName + '" را حذف کنید؟',
      showConfirmButton: true,
      confirmButtonText: 'بله',
      showCancelButton: true,
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService
          .get(ServerApis.removeAppService, { id: row.serviceId })
          .pipe(
            finalize(() => {
              this.chdr.detectChanges();
            }),
          )
          .subscribe((response) => {
            if (response.isSuccess) {
              this.toastrService.success('حذف اطلاعات با موفقیت انجام شد.');
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
}
