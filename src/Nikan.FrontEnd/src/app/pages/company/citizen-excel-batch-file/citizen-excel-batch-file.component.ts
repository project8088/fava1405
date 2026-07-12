import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup } from '@angular/forms';
import Swal from 'sweetalert2';
import { ServerApis } from '@core/server-apis';
import { CompanyImportExcelDialogComponent } from '../_dialogs/importPersonel-excel/importPersonel-excel.component';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-citizen-excel-batch-file',
  templateUrl: './citizen-excel-batch-file.component.html',
  styleUrls: ['./citizen-excel-batch-file.component.scss'],
  standalone: false,
})
export class CompanyCitizenExcelBatchFileListComponent extends AppBase implements AfterViewInit {
  loading?: boolean;
  displayedColumns: string[] = [
    'row',
    'exportFileName',
    'importByUser',
    'creationDate',
    'countRow',
    'fileAccept',
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
    this.dataService
      .get(ServerApis.personnelImportFileList, {})
      .pipe(
        finalize(() => {
          this.isLoadingResults = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
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
        (error: any) => {},
      );
  }

  pageEvent(event: PageEvent) {
    this.getList();
  }

  applyFilter() {
    this.dataSource.filter = this.searchForm.get('title')?.value;
  }

  delete(row: any) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا می خواهید "' + row.id + '" را حذف کنید؟',
      showConfirmButton: true,
      confirmButtonText: 'بله',
      showCancelButton: true,
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService.get(ServerApis.removeImportFile, { importId: row.id }).subscribe(
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
          (error: any) => {
            this.toastrService.error('حذف اطلاعات با خطا مواجه شده است!');
          },
        );
      }
    });
  }

  openExcelDialog() {
    this.matDialog.open(CompanyImportExcelDialogComponent, {
      panelClass: 'custom-dialog',
      width: '60%',
    });
  }
}
