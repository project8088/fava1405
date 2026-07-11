import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import Swal from 'sweetalert2';
import { AuthUser } from '@core/authentication/user.model';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-ticket-activity',
  templateUrl: './ticket-activity.component.html',
  styleUrls: ['./ticket-activity.component.scss'],
  standalone: false,
})
export class TicketActivityComponent extends AppBase implements OnInit {
  frm: FormGroup;
  isSaving = false;
  id: string = '';
  loading?: boolean;
  list: any[] = [];
  user?: AuthUser | null;
  constructor() {
    super();
    this.user = this.authService.currentUserValue;

    this.route.params.subscribe((p) => {
      this.id = p['id'];
      this.getList();
    });

    this.frm = this.fb.group({
      description: ['', [Validators.required]],
      minute: ['', [Validators.required]],
      onDate: ['', [Validators.required]],
      isSubtractFromContract: [true],
    });
  }

  ngOnInit(): void {}

  getList() {
    this.loading = true;
    this.list = [];
    this.dataService.get(ServerApis.getTicketActivity, { ticketId: this.id }).subscribe(
      (response) => {
        this.loading = false;
        if (response.isSuccess) {
          this.list = response.data ? response.data : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error: any) => {
        this.loading = false;
      },
    );
  }

  delete(row: any) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا برای حذف "' + row.description + '" اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService
          .get(ServerApis.removeTicketActivity, {
            id: row.id,
          })
          .subscribe(
            (response) => {
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
            (error: any) => {},
          );
      }
    });
  }

  save() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return;
    }
    this.isSaving = true;
    var form = this.frm.value;

    this.dataService
      .post(ServerApis.addTicketActivity, {
        TicketId: this.id,
        description: form.description,
        isSubtractFromContract: form.isSubtractFromContract ? form.isSubtractFromContract : false,
        onDate: this.dataService.formatDate(form.onDate),
        minute: form.minute,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
            this.frm.reset({ commentText: '', isSubtractFromContract: true });
            this.getList();
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {
          this.isSaving = false;
        },
      );
  }
}
