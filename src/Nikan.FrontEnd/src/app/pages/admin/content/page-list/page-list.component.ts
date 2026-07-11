import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import Swal from 'sweetalert2';
import { ManageAttachmentDialogComponent } from '../_dialogs/manage-attachment/manage-attachment.component';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-page-list',
  templateUrl: './page-list.component.html',
  styleUrls: ['./page-list.component.scss'],
  standalone: false,
})
export class AdminPageListComponent extends AppBase implements AfterViewInit {
  displayedColumns: string[] = [
    'row',
    'title',
    'description',
    'createdOnDate',
    'clicks',
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
          return this.dataService.get(ServerApis.getPagedWebPageItems, param);
        }),
        map((response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.webPages ? response.data.webPages : [];
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

  deletePage(row: any) {
    var text = 'آیا برای حذف کردن صفحه "' + row.title + '" اطمینان دارید؟';

    Swal.fire({
      title: 'حذف صفحه',
      text: text,
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService
          .get(ServerApis.removeWebPage, {
            id: row.id,
          })
          .subscribe(
            (response) => {
              if (response.isSuccess) {
                this.toastrService.success('حذف با موفقیت انجام شد.');
                this.getList();
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            },
            (error: any) => {},
          );
      }
    });
  }

  manageAttachment(item: any) {
    this.matDialog
      .open(ManageAttachmentDialogComponent, {
        data: {
          guid: item.attachmentFileGroup,
        },
        minWidth: '80%',
        panelClass: 'custom-dialog',
      })
      .afterClosed()
      .subscribe((response) => {});
  }
}
