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
  selector: 'adm-manager-users',
  templateUrl: './manager-users.component.html',
  styleUrls: ['./manager-users.component.scss'],
  standalone: false,
})
export class AdminManagerUsersComponent extends AppBase implements OnInit, AfterViewInit {
  displayedColumns: string[] = [
    'row',
    'imageUrl',
    'personelCode',
    'displayName',
    'fatherName',
    'nationCode',
    'userCompany',
    'organizationalPosition',
    'mobileNumber',
    'cellNumber',
    'email',
    'city',
    'isManagementMembers',
    'operation',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  searchForm: FormGroup;
  baseUrl: string = ServerApis.baseUrl;
  groupList: any[] = [];

  constructor(private customValidator: CustomFormValidators) {
    super();
    this.searchForm = this.fb.group({
      fromDate: [null],
      toDate: [null],
      title: [''],
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

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.searchAdminPersonel, param);
        }),
        map((response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.members ? response.data.members : [];
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

  removePersonal(row: any) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا برای حذف "' + row.firstName + ' ' + row.lastName + '" اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        row.loading = true;
        this.dataService
          .get(ServerApis.removePersonel, { id: row.id })
          .pipe(
            finalize(() => {
              row.loading = false;
              this.chdr.detectChanges();
            }),
          )
          .subscribe((response) => {
            if (response.isSuccess) {
              this.toastrService.success('حذف اطلاعات با موفقیت انجام شد.');
              this.getList();
            } else {
              let msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
              this.toastrService.error(msg);
            }
          });
      }
    });
  }
}
