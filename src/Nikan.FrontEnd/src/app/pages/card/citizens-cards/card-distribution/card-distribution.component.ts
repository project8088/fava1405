import { AfterViewInit, Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { AuthService } from '@core/authentication/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { DataService } from '../../../../core/services/data-service.service';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { ServerApis } from '../../../../core/server-apis';
import { CitizenProfileDialogComponent } from '../../../../shared/_dialog/citizen-profile/citizen-profile.component';
import { CardProfileDialogComponent } from '../../../../shared/_dialog/card-profile/card-profile.component';

@Component({
  selector: 'app-card-distribution',
  templateUrl: './card-distribution.component.html',
  styleUrls: ['./card-distribution.component.scss'],
})
export class CardCarddistributionComponent implements AfterViewInit {
  @ViewChild('value') searchElement: ElementRef;

  isSend: boolean;

  loadingqueue: boolean;
  queuelist: any[] = [];
  frm: FormGroup;

  courseId: string;
  queueId: string;

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  loading: boolean = true;
  card: any = {};
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;
  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private router: Router,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private customValidator: CustomFormValidators,
  ) {
    this.route.params.subscribe((p) => {
      this.courseId = p.id;
    });

    this.frm = this.fb.group({
      courseId: [''],
      cardId: [''],
      citizenId: [null],
      deliverType: [null],
      cardTypeId: [null],
      deliveringCenterId: [''],
    });

    this.searchForm = this.fb.group({
      searchCitizensType: [null],
      value: [''],
      autopost: [false],
    });
  }

  ngAfterViewInit() {
    this.getQueueListInCourse();
  }

  getinfo() {
    this.queueId = '';
    this.loading = true;

    var param: any = this.searchForm.value;
    this.searchForm.get('value').disable();
    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          return this.dataService.get(ServerApis.searchCardForQueue, param);
        }),
        map((response) => {
          if (response.isSuccess && response.data) {
            this.card = response.data ? response.data : {};
            this.searchForm.get('value').enable();
            this.searchForm.get('value').setValue('');
            this.searchElement.nativeElement.focus();
            this.loading = false;
            if (param.autopost == true) {
              this.sendCardToQueue(
                response.data.citizenCardInfoId,
                response.data.citizenId,
                response.data.deliverType,
                response.data.cardTypeId,
                response.data.deliveringCenterId,
              );
            }
          } else {
            this.card = null;
            this.loading = true;
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
            this.searchForm.get('value').enable();
            this.searchForm.get('value').setValue('');
            this.searchElement.nativeElement.focus();
            this.loading = false;
          }
        }),
        catchError((err) => {
          this.loading = false;
          this.searchForm.get('value').enable();
          this.searchForm.get('value').setValue('');
          this.searchElement.nativeElement.focus();
          return observableOf([]);
        }),
      )
      .subscribe((data) => {});
  }

  sendCardToQueue(cardId, citizenId, deliverType, cardTypeId, deliveringCenterId) {
    this.queueId = '';
    this.isSend = true;
    var url = ServerApis.sendCardToQueue;
    var params = this.frm.value;
    params.courseId = +this.courseId;
    params.id = cardId;
    params.citizenId = citizenId;
    params.deliverType = deliverType;
    params.cardTypeId = cardTypeId;
    params.deliveringCenterId = deliveringCenterId;

    this.dataService.post(url, params).subscribe(
      (response) => {
        this.isSend = false;
        if (response && response.isSuccess) {
          this.toastrService.success(response.messages);
          this.queueId = response.data.queueId;
          this.getQueueListInCourse();
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.isSend = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  getQueueListInCourse() {
    this.loadingqueue = true;
    this.dataService.get(ServerApis.getQueueListInCourse, { courseId: this.courseId }).subscribe(
      (response) => {
        this.loadingqueue = false;
        if (response.isSuccess) {
          this.queuelist = response.data ? response.data : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loadingqueue = false;
      },
    );
  }

  openCitizenProfile(row) {
    this.matDialog.open(CitizenProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        userCode: row.userCode,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }

  openCardProfile(row) {
    this.matDialog.open(CardProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        id: row.id,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }

  delete(cardId) {
    this.dataService
      .get(ServerApis.removeCardFromQueueByCourseId, { courseId: this.courseId, cardId: cardId })
      .subscribe(
        (response) => {
          if (response.isSuccess) {
            this.toastrService.success('حذف کارت از صف با موفقیت انجام شد.');
            this.getQueueListInCourse();
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.toastrService.error('حذف اطلاعات با خطا مواجه شده است!');
        },
      );
  }
}
