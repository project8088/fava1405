import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import Swal from 'sweetalert2';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'card-free-request-list',
  templateUrl: './free-request-list.component.html',
  styleUrls: ['./free-request-list.component.scss'],
  standalone: false,
})
export class CardFreeRequestCardListComponent extends AppBase implements OnInit, AfterViewInit {
    loading?: boolean;
  displayedColumns: string[] = [
    'row',
    'discountTitle',
    'cardType',
    'group',
    'creationDate',
    'creationBy',
    'letterNumber',
    'freeCardApplicantOrganization',
    'accepted',
    'operation',
  ];

  baseUrl: string = ServerApis.baseUrl;
  cardTypeId: string;

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
    this.route.params.subscribe((p) => {
      this.cardTypeId = p['id'];
    });

    this.searchForm = this.fb.group({});
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.getList();
  }

  getList() {
    var param: any = this.searchForm.value;
    param.offset = this.paginator ? this.paginator.pageIndex : 0;
    param.count = this.paginator ? this.paginator.pageSize : 10;
    param.cardTypeId = +this.cardTypeId;
    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.pagedRequestFreeCardLsit, param);
        }),
        map((response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.items ? response.data.items : [];
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

  delete(row:any) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا می خواهید "' + row.discountTitle + '" را حذف کنید؟',
      showConfirmButton: true,
      confirmButtonText: 'بله',
      showCancelButton: true,
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService.get(ServerApis.removeRequestFreeCard, { id: row.id }).subscribe(
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

  acceptedRequest(item:any) {
    Swal.fire({
      title: 'آیا برای تائید و صدور کارت رایگان اطمینان دارید؟',
      text: 'در صورت تائید صدور، دیگر امکان ویرایش و حذف وجود نخواهد داشت. و کارت ها برای شهروندان این درخواست صادر  خواهد شد. ',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        item.loading = true;
        this.dataService
          .get(ServerApis.acceptedRequestFreeCard, {
            id: item.id,
          })
          .subscribe(
            (response) => {
              item.loading = false;
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
            (error) => {
              item.loading = false;
            },
          );
      }
    });
  }
}
