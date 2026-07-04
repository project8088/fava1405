import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { ToastrService } from 'ngx-toastr';

import Swal from 'sweetalert2';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../../../core/services/data-service.service';
import { ServerApis } from '../../../core/server-apis';
@Component({
  selector: 'app-citizen-excel-batch-file-details',
  templateUrl: './citizen-excel-batch-file-details.component.html',
  styleUrls: ['./citizen-excel-batch-file-details.component.scss'],
})
export class CompanyCitizenExcelBatchFileDetailsComponent implements AfterViewInit {
  loading: boolean;
  displayedColumns: string[] = [
    'row',
    'gender',
    'nationCode',
    'firstName',
    'lastName',
    'fatherName',
    'birthDate',
    'mobile',
    'jobTitle',
    'address',
  ];

  importId: string;
  info: any = {};
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;
  events: any[] = [];
  constructor(
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private router: Router,
    private dataService: DataService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
  ) {
    this.searchForm = this.fb.group({
      title: [''],
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe((p) => {
      this.importId = p.id;
      this.getList();
    });
  }

  ngAfterViewInit() {}

  getList() {
    this.isLoadingResults = true;
    this.data = [];
    this.dataService
      .get(ServerApis.personnelImportFileDetails, { importId: this.importId })
      .subscribe(
        (response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            this.info = response.data;
            this.data = response.data.personnelInfo ? response.data.personnelInfo : [];
            this.dataSource.data = this.data;
            this.listCount = this.data.length;
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.isLoadingResults = false;
        },
      );
  }

  pageEvent(event: PageEvent) {
    this.getList();
  }

  applyFilter() {
    this.dataSource.filter = this.searchForm.get('title').value;
  }

  confirm() {
    Swal.fire({
      title: 'تائید',
      text: 'آیا برای تائید فایل اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService
          .get(ServerApis.confirmCitizenFile, {
            importId: this.importId,
          })
          .subscribe(
            (response) => {
              if (response.isSuccess) {
                this.toastrService.success('با موفقیت تائید شد.');
                this.getList();
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            },
            (error) => {},
          );
      }
    });
  }

  delete() {
    Swal.fire({
      title: 'حذف',
      text: 'آیا می خواهید "' + this.importId + '" را حذف کنید؟',
      showConfirmButton: true,
      confirmButtonText: 'بله',
      showCancelButton: true,
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService.get(ServerApis.removeImportFile, { importId: this.importId }).subscribe(
          (response) => {
            if (response.isSuccess) {
              this.toastrService.success('حذف اطلاعات با موفقیت انجام شد.');
              this.getList();
            } else {
              let msg = response.messages
                ? response.messages
                : 'متاسفانه خطایی در سرور رخ داده است!';
              this.toastrService.error(msg);
            }
          },
          (error) => {
            this.toastrService.error('حذف اطلاعات با خطا مواجه شده است!');
          },
        );
      }
    });
  }
}
