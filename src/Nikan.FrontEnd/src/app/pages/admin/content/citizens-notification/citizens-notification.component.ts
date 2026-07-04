import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ServerApis } from '../../../../core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-citizens-notification',
  templateUrl: './citizens-notification.component.html',
  styleUrls: ['./citizens-notification.component.scss'],
  standalone: false,
})
export class AdminAddOrUpdateCitizensNotificationComponent
  extends AppBase
  implements OnInit, AfterViewInit
{
  notificationId: string;
  notyForm: FormGroup;

  notification: any = {};
  companyList: any[] = [];
  baseEnums: any = {};
  isSaving=false;
  loading: boolean;

  constructor() {
    super();
    this.route.params.subscribe((p) => {
      if (p.id && p.id != '0') {
        this.notificationId = p.id;
        this.getNotificationInfo();
        this.getCitizenNotifications();
      } else {
        this.notificationId = '';
      }
    });

    this.notyForm = this.fb.group({
      nationalCode: [null],
      groupId: [null],
    });
  }

  ngOnInit(): void {}
  ngAfterViewInit() {
    this.getGroups();
  }

  getGroups() {
    this.dataService.get(ServerApis.getGroups).subscribe(
      (response) => {
        this.baseEnums.citizenGroups = response.data;
      },
      (error) => {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  getNotificationInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getNotification, {
        id: this.notificationId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.notification = response.data;
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loading = false;
        },
      );
  }

  getCitizenNotifications() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getCitizenNotifications, {
        id: this.notificationId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.companyList = response.data ? response.data : [];
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loading = false;
        },
      );
  }

  /**
   * for bind object in select
   * @param item
   */
  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.key === c2.key : c1 === c2;
  }

  save() {
    if (this.notyForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.notyForm.markAllAsTouched();
      return false;
    }

    let form = this.notyForm.value;
    let params: any = {
      id: this.notificationId ? +this.notificationId : '',
      nationalCode: form.nationalCode,
      groupId: form.groupId,
    };

    debugger;

    this.isSaving = true;
    this.dataService.post(ServerApis.addCitizensNotifactions, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.getCitizenNotifications();
          // this.router.navigate(['/admin/notifications']);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.isSaving = false;
      },
    );
  }

  delete(item) {
    item.loading = true;
    this.dataService.get(ServerApis.removeCitizensNotifactions, { id: item.id }).subscribe(
      (response) => {
        item.loading = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت حذف شد.');
          this.getCitizenNotifications();
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        item.loading = false;
      },
    );
  }
}
