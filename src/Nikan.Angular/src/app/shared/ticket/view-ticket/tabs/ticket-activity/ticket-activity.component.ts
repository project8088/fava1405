import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ServerApis } from '../../../../../core/server-apis';
import { DataService } from '../../../../../core/services/data-service.service';
import Swal from 'sweetalert2';
import { ActivatedRoute } from '@angular/router';
import { AuthUser } from '../../../../../core/authentication/user.model';
import { AuthService } from '../../../../../core/authentication/auth.service';

@Component({
  selector: 'app-ticket-activity',
  templateUrl: './ticket-activity.component.html',
  styleUrls: ['./ticket-activity.component.scss']
})
export class TicketActivityComponent implements OnInit {

  frm: FormGroup;
  isSaving: boolean;
  id: string = '';
  loading: boolean;
  list: any[] = [];
  user: AuthUser;
  constructor(
    private fb: FormBuilder,
    private toastrService: ToastrService,
    private dataService: DataService,
    private route: ActivatedRoute,
    private authService: AuthService
  ) {
    this.user = this.authService.currentUserValue;

    this.route.params.subscribe(p => {
      this.id = p.id;
      this.getList();

    });

    this.frm = this.fb.group(
      {
       description: ['', [Validators.required]],
       minute: ['', [Validators.required]],
        onDate: ['', [Validators.required]],
        isSubtractFromContract: [true]
      });
  }

  ngOnInit(): void {
  }



  getList() {
    this.loading = true;
    this.list = [];
    this.dataService.get(ServerApis.getTicketActivity, { ticketId: this.id }).subscribe(response => {
      this.loading = false;
      if (response.isSuccess) {
        this.list = response.data ? response.data : [];

      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
      this.loading = false;

    });
  }




  delete(row) {

    Swal.fire({
      title: 'حذف',
      text: 'آیا برای حذف "' + row.description + '" اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then(result => {
      if (result.value) {
        this.dataService.get(ServerApis.removeTicketActivity, {
          id: row.id,
        }).subscribe(response => {
          if (response.isSuccess) {
            this.toastrService.success('با موفقیت حذف شد.');
            this.getList();
          } else {
            let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
            this.toastrService.error(msg);
          }
        }, error => {
        });
      }

    });
  }


  save() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return false;
    }
    this.isSaving = true;
    var form = this.frm.value;

    this.dataService.post(ServerApis.addTicketActivity, {
      TicketId: this.id,
      description: form.description,
      isSubtractFromContract: form.isSubtractFromContract ? form.isSubtractFromContract : false,
      onDate: this.dataService.formatDate(form.onDate),
      minute: form.minute
    }).subscribe(response => {
      this.isSaving = false;
      if (response.isSuccess) {
        this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
        this.frm.reset({ commentText: '', isSubtractFromContract: true });
        this.getList();
      } else {
        let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
        this.toastrService.error(msg);
      }
    }, error => {
      this.isSaving = false;
    });

  }


}
