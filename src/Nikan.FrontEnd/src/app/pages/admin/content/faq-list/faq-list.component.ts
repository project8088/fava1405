import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { FormGroup } from '@angular/forms';
import Swal from 'sweetalert2';
import { ServerApis } from '@core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-faq-list',
  templateUrl: './faq-list.component.html',
  styleUrls: ['./faq-list.component.scss'],
  standalone: false,
})
export class AdminFaqListComponent extends AppBase implements OnInit, AfterViewInit {
    loading?: boolean;
  displayedColumns: string[] = [
    'row',
    'title',
    'questionGroup',
    'viewCount',
    'isActive',
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
    this.dataService.get(ServerApis.getFaqList, {}).subscribe(
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
      (error:any) => {
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
      text: 'آیا می خواهید "' + row.title + '" را حذف کنید؟',
      showConfirmButton: true,
      confirmButtonText: 'بله',
      showCancelButton: true,
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService.get(ServerApis.removeFaq, { id: row.id }).subscribe(
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
          (error:any) => {
            this.toastrService.error('حذف اطلاعات با خطا مواجه شده است!');
          },
        );
      }
    });
  }
}
