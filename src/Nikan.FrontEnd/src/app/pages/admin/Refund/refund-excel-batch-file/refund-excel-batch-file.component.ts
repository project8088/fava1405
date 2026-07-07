import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup } from '@angular/forms';
import { ServerApis } from '../../../../core/server-apis';
import Swal from 'sweetalert2';
import { AdminImportRefundExcelDialogComponent } from '../dialog/refund-import-excel/import-refund-excel.component';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-refund-excel-batch-file',
  templateUrl: './refund-excel-batch-file.component.html',
  styleUrls: ['./refund-excel-batch-file.component.scss'],
  standalone: false,
})
export class AdminRefundExcelBatchFileListComponent extends AppBase implements AfterViewInit {
    loading?: boolean;
  displayedColumns: string[] = [
    'row',
    'importId',
    'fileName',
    'importBy',
    'onDate',
    'isConfirm',
    'operation',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  searchForm: FormGroup;
  events: any[] = [];
  constructor() {
    super();
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
    this.dataService.get(ServerApis.refundImportFileList, {}).subscribe(
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
    this.dataSource.filter = this.searchForm.get('title')?.value;
  }

  delete(row:any) {
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
          .get(ServerApis.removeRefundImportFile, { importId: row.importId })
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
    this.matDialog.open(AdminImportRefundExcelDialogComponent, {
      panelClass: 'custom-dialog',
      width: '60%',
    });
  }
}
