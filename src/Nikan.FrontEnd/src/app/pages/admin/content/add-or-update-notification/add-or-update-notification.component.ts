import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'adm-add-or-update-notification',
  templateUrl: './add-or-update-notification.component.html',
  styleUrls: ['./add-or-update-notification.component.scss'],
  standalone: false,
})
export class AdminAddOrUpdateNotificationComponent
  extends AppBase
  implements OnInit, AfterViewInit
{
  isUpdate = false;
  notificationId: string = '';
  notyForm: FormGroup;

  isSaving = false;
  imageUrl: string = '';
  loading?: boolean;
  baseUrl = ServerApis.baseUrl;

  constructor() {
    super();
    this.route.params.subscribe((p) => {
      if (p['id'] && p['id'] != '0') {
        this.isUpdate = true;
        this.notificationId = p['id'];
        this.getNotificationInfo();
      } else {
        this.notificationId = '';
        this.isUpdate = false;
      }
    });

    this.notyForm = this.fb.group({
      id: [null],
      notificationNumber: [null, [Validators.required]],
      title: [null, [Validators.required, Validators.maxLength(500)]],
      description: [null, [Validators.required, Validators.maxLength(2000)]],
      body: [null, []],
      publishDate: [null, [Validators.required]],
      endDate: [null, [Validators.required]],
      isActive: [true, []],
      isPrivate: [false, []],
    });
  }

  ngOnInit(): void {}

  ngAfterViewInit() {}

  getNotificationInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getNotification, {
        id: this.notificationId,
        forEdit: true,
      })
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess && response.data) {
          this.notyForm.setValue({
            id: response.data.id,
            title: response.data.title,
            description: response.data.description,
            body: response.data.body,
            isActive: response.data.isActive,
            isPrivate: response.data.isPrivate,
            publishDate: response.data.publishDate ? new Date(response.data.publishDate) : null,
            endDate: response.data.endDate ? new Date(response.data.endDate) : null,
            notificationNumber: response.data.notificationNumber
              ? response.data.notificationNumber
              : '',
          });
          this.imageUrl = response.data.imageUrl;
        } else {
          var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      });
  }

  /**
   * for bind object in select
   * @param item
   */
  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.key === c2.key : c1 === c2;
  }

  getAttachmentId(ev: { uploadUrl: string }) {
    this.imageUrl = ev.uploadUrl;
  }

  save() {
    if (this.notyForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.notyForm.markAllAsTouched();
      return;
    }

    let form = this.notyForm.value;
    let params: any = {
      id: this.notificationId ? +this.notificationId : '',
      title: form.title,
      body: form.body,
      description: form.description,
      isActive: form.isActive,
      isPrivate: form.isPrivate,
      publishDate: this.dataService.formatDate(form.publishDate),
      endDate: this.dataService.formatDate(form.endDate),
      imageUrl: this.imageUrl,
      notificationNumber: form.notificationNumber,
    };
    this.isSaving = true;
    this.dataService
      .post(ServerApis.addOrUpdateNotifications, params)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.router.navigate(['/admin/content/notifications']);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      });
  }
}
