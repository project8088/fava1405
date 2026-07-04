import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import Swal from 'sweetalert2';
import { ServerApis } from '../../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-news-comments',
  templateUrl: './news-comments.component.html',
  styleUrls: ['./news-comments.component.scss'],
  standalone: false,
})
export class AdminNewsCommentsComponent extends AppBase implements OnInit {
  newsId: string;

  displayedColumns: string[] = [
    'row',
    'userIP',
    'emailAddress',
    'fullName',
    'commentMessage',
    'isPublish',
    'operation',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;

  groupList: any[] = [];

  constructor(private customValidator: CustomFormValidators) {
    super();
    this.searchForm = this.fb.group({
      query: [null, []],
    });

    this.route.params.subscribe((p) => {
      if (p['id'] && p['id'] != '0') {
        this.newsId = p['id'];
        this.getCommentList();
      } else {
        toastrService.error('خبر یافت نشد.');
        this.router.navigate(['/admin/news-list']);
      }
    });
  }

  ngOnInit() {}

  getCommentList() {
    this.isLoadingResults = true;
    this.data = [];
    this.dataService.get(ServerApis.getNewsComments, { id: this.newsId }).subscribe(
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
    this.getCommentList();
  }

  applyFilter() {
    this.dataSource.filter = this.searchForm.get('query')?.value;
  }

  newJob(row) {
    this.router.navigate(['/placement/new-job'], {
      queryParams: {
        id: '',
        companyId: row.id,
        companyTitle: row.companyName,
      },
    });
  }

  publishComment(row) {
    Swal.fire({
      title: 'تغییر وضعیت',
      html:
        '<p>آیا برای انتشار خبر اطمینان دارید؟</p> <p><small class="text-info">' +
        row.commentMessage +
        '</small></p>',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.publish(row, true);
      }
    });
  }

  publish(row, IsPublish) {
    this.dataService
      .post(ServerApis.publishComment, {
        commentId: row.id,
        IsPublish: IsPublish,
      })
      .subscribe(
        (response) => {
          if (response.isSuccess) {
            this.toastrService.success('تغییر وضعیت با موفقیت انجام شد.');
            this.getCommentList();
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {},
      );
  }
}
