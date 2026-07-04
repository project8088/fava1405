import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { HelperService } from '@core/services/helper.service';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { RegisterServiceModel } from '@core/models/register-service.model';
import { ServerApis } from '../../../../core/server-apis';
import Swal from 'sweetalert2';
import { CitizenProfileDialogComponent } from '../../../../shared/_dialog/citizen-profile/citizen-profile.component';
import { CardProfileDialogComponent } from '../../../../shared/_dialog/card-profile/card-profile.component';
import { CardAddOrUpadateQueueDialogComponent } from '../dialog/add-update-queue/add-update-queue.component';
import { CardDeliveryQueueOperatorDialogComponent } from '../dialog/delivery-queue-operator/delivery-queue-operator.component';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'card-card-course-queue-list',
  templateUrl: './card-course-queue-list.component.html',
  styleUrls: ['./card-course-queue-list.component.scss'],
    standalone: false
})
export class CardCardCourseQueuelistComponent extends AppBase implements AfterViewInit {
  displayedColumns: string[] = [
    'row',
    'name',
    'cardTypeId',
    'queueInputType',
    'indexOrder',
    'onDate',
    'isActive',
    'isLock',
    'cardCount',
    'groups',
    'operation',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  courseId: string;
  baseEnums: any = {};
  baseUrl = ServerApis.baseUrl;
  printUrl: string = ServerApis.prinQueueForPost;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;
  constructor(
    private customValidator: CustomFormValidators,
    private helperService: HelperService,
  ) {
      super();
    this.route.params.subscribe((p) => {
      this.courseId = p.id;
    });

    this.searchForm = this.fb.group({
      name: [''],
      courseId: [0],
    });
  }

  ngAfterViewInit() {
    this.getList();
  }

  getList() {
    var param: any = this.searchForm.value;
    param.offset = this.paginator ? this.paginator.pageIndex : 0;
    param.count = this.paginator ? this.paginator.pageSize : 2000;
    param.courseId = +this.courseId;

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.getPagedeDistributionQueue, param);
        }),
        map((response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.items ? response.data.items : [];
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

  openCitizenProfile(row) {
    this.matDialog.open(CitizenProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        id: row.citizenId,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }

  openAddCardQueueDialog(item) {
    this.matDialog
      .open(CardAddOrUpadateQueueDialogComponent, {
        panelClass: 'custom-dialog',
        minWidth: '600px',
        data: {
          item: item,
          courseId: this.courseId,
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          this.getList();
        }
      });
  }

  delete(row) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا برای حذف اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        row.loading = true;
        this.dataService
          .get(ServerApis.removeCardQueue, {
            id: row.id,
          })
          .subscribe(
            (response) => {
              row.loading = false;
              if (response.isSuccess) {
                this.toastrService.success('با موفقیت حذف شد.');
                this.getList();
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
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

  cardDeliveryQueueOperator(item) {
    this.matDialog
      .open(CardDeliveryQueueOperatorDialogComponent, {
        panelClass: 'custom-dialog',
        minWidth: '600px',
        data: {
          info: item,
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          this.getList();
        }
      });
  }
}
