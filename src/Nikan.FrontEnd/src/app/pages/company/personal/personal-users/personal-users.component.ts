import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import Swal from 'sweetalert2';
import { ServerApis } from '../../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { AuthUser } from '../../../../core/authentication/user.model';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-personal-users',
  templateUrl: './personal-users.component.html',
  styleUrls: ['./personal-users.component.scss'],
})
export class CompanyPersonalUsersComponent extends AppBase implements OnInit, AfterViewInit {
  displayedColumns: string[] = [];

  companyId: string = '';

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;

  groupList: any[] = [];
  user: AuthUser;
  baseUrl: string = ServerApis.baseUrl;
  imageUrl: string = '';
  constructor(
    private customValidator: CustomFormValidators
  ) {
      super();
    this.user = this.authService.currentUserValue;
    this.displayedColumns = [
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

    this.route.params.subscribe((p) => {
      this.companyId = p.id;
    });

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

  getAttachmentId(ev) {
    this.imageUrl = ev.uploadUrl;
  }

  getList() {
    var param: any = this.searchForm.value;
    param.offset = this.paginator ? this.paginator.pageIndex : 0;
    param.count = this.paginator ? this.paginator.pageSize : 10;
    if (param.fromDate) param.fromDate = this.dataService.formatDate(param.fromDate);
    if (param.toDate) param.toDate = this.dataService.formatDate(param.toDate);
    if (this.companyId && this.companyId != '0') param.companyId = this.companyId;
    else param.companyId = null;

    var url = ServerApis.searchCompanyPersonel;
    if (this.user.isAdmin) url = ServerApis.allCompanyPersonel;

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(url, param);
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

  removePersonal(row) {
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
        this.dataService.get(ServerApis.removePersonel, { id: row.id }).subscribe(
          (response) => {
            row.loading = false;
            if (response.isSuccess) {
              this.toastrService.success('حذف اطلاعات با موفقیت انجام شد.');
              this.getList();
            } else {
              let msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
              this.toastrService.error(msg);
            }
          },
          (error) => {
            row.loading = false;
          },
        );
      }
    });
  }

  back() {
    this.router.navigate(['/admin/companies']);
  }
}
