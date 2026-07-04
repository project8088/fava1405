import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DataService } from '../../../../core/services/data-service.service';
import { ToastrService } from 'ngx-toastr';
import { ServerApis } from '../../../../core/server-apis';
import Swal from 'sweetalert2';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AdminImportCitizenExcelDialogComponent } from '../dialog/citizen-import-excel/import-citizen-excel.component';
@Component({
  selector: 'adm-citizen-excel-batch-file',
  templateUrl: './citizen-excel-batch-file.component.html',
  styleUrls: ['./citizen-excel-batch-file.component.scss'],
})
export class AdminCitizenExcelBatchFileListComponent implements AfterViewInit {
  loading: boolean;
  displayedColumns: string[] = [
    'row',
    'importId',
    'fileName',
    'importBy',
    'onDate',
    'count',
    'isConfirm',
    'operation',
  ];

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
  ) {
    this.searchForm = this.fb.group({
      title: [''],
    });
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.getList();
  }

  getList() {
    this.isLoadingResults = true;
    this.data = [];
    this.dataService.get(ServerApis.citizenImportFileList, {}).subscribe(
      (response) => {
        this.isLoadingResults = false;
        if (response.isSuccess) {
          this.data = response.data ? response.data : [];
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

  delete(row) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا می خواهید "' + row.importId + '" را حذف کنید؟',
      showConfirmButton: true,
      confirmButtonText: 'بله',
      showCancelButton: true,
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService
          .get(ServerApis.removeCitizenImportFile, { importId: row.importId })
          .subscribe(
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

  openExcelDialog() {
    this.matDialog.open(AdminImportCitizenExcelDialogComponent, {
      panelClass: 'custom-dialog',
      width: '60%',
    });
  }

  confirm(item) {
    Swal.fire({
      title: 'آیا برای تائید ثبت نام دسته ایی دارید؟',
      text: ' تایید دسته ایی ثبت نام ',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        item.loading = true;
        this.dataService
          .get(ServerApis.confirmCitizenfileexcel, {
            importId: item.importId,
          })
          .subscribe(
            (response) => {
              item.loading = false;
              if (response.isSuccess) {
                this.toastrService.success(response.messages);
                this.getList();
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            },
            (error) => {
              item.loading = false;
            },
          );
      }
    });
  }
}
