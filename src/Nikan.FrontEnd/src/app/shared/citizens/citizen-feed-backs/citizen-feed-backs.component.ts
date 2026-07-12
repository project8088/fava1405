import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'app-citizen-feed-back-list',
  templateUrl: './citizen-feed-backs.component.html',
  styleUrls: ['./citizen-feed-backs.component.scss'],
  standalone: false,
})
export class AppCitizenFeedBackListComponent extends AppBase implements AfterViewInit {
  listfeedback: any[] = [];
  isSavingfeedback: boolean = false;
  groupfeedbackList: any[] = [];
  userCode: string = '';
  loadingfeedback: boolean = false;
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  feedbackfrm: FormGroup;
  constructor() {
    super();
    this.feedbackfrm = this.fb.group({
      feedbackDescription: [null, [Validators.required]],
      feedbackId: [null, [Validators.required]],
    });

    this.getBaseListFeedbacke();

    this.route.params.subscribe((p) => {
      this.userCode = p['id'] ? p['id'] : null;
    });
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.getListFeedbacks();
  }

  getListFeedbacks() {
    this.loadingfeedback = true;
    this.listfeedback = [];
    this.dataService.get(ServerApis.getAllCitizenFeedbacks, { userCode: this.userCode })
      .pipe(
        finalize(() => {
          this.loadingfeedback = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response.isSuccess) {
                this.listfeedback = response.data ? response.data : [];
              } else {
                let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            }, (error: any) => {
            });
  }
  savefeedback() {
    if (this.feedbackfrm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.feedbackfrm.markAllAsTouched();
      return;
    }

    var formValue = this.feedbackfrm.value;
    this.isSavingfeedback = true;
    this.dataService
            .post(ServerApis.addFeedbacke, {
              userCode: this.userCode,
              feedbackId: formValue.feedbackId,
              feedbackDescription: formValue.feedbackDescription ? formValue.feedbackDescription : '',
            })
      .pipe(
        finalize(() => {
          this.isSavingfeedback = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
                if (response.isSuccess) {
                  this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
                  this.getListFeedbacks();
                } else {
                  var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
                  this.toastrService.error(msg);
                }
              }, (error: any) => {
              });
  }
  getBaseListFeedbacke() {
    this.dataService.get(ServerApis.getBaseListFeedbacke).subscribe((response) => {
      if (response.isSuccess) this.groupfeedbackList = response.data ? response.data : [];
      else {
        let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
        this.toastrService.error(msg);
      }
    });
  }

  pageEvent(event: PageEvent) {
    this.getListFeedbacks();
  }
}
