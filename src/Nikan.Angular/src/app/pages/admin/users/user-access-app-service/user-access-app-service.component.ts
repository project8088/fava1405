import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { ServerApis } from '../../../../core/server-apis';
import { DataService } from '../../../../core/services/data-service.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { RequireMatch } from '../../../../core/custom-validator/requireMatch';
import Swal from 'sweetalert2';

@Component({
  selector: 'user-access-app-service',
  templateUrl: './user-access-app-service.component.html',
  styleUrls: ['./user-access-app-service.component.scss']
})
export class AdminUserAppAccessServiceComponent implements OnInit {

  loading: boolean;
  userAppAccessList: any[] = [];
  appList: any[] = [];
  userId: string = '';
  form: FormGroup;
  isSaving: boolean;

  constructor(
    private fb: FormBuilder,
    private customValidators: CustomFormValidators,
    private dataService: DataService,
    private toastrService: ToastrService,
    private route: ActivatedRoute
  ) {
    this.form = this.fb.group({
      serviceId: [null, [Validators.required]],

    });




    this.route.params.subscribe(p => {
      if (p.id != '0' && p.id)
        this.userId = p.id;
      this.getuserRoleList();
    });
  }




  ngOnInit(): void {
    this.dataService.get(ServerApis.getBaseListAppService).subscribe(response => {
      this.appList = response.data ? response.data : [];
    });

  }



  getuserRoleList() {
    this.loading = true;
    this.dataService.get(ServerApis.getAllUserAppService, {
      id: this.userId
    }).subscribe(response => {
      this.loading = false;
      if (response.isSuccess && response.data) {
        this.userAppAccessList = response.data ? response.data : [];

      } else {
        var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
        this.toastrService.error(msg);
      }
    }, error => {
      this.loading = false;
    });
  }



  save() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
      return false;
    }
    var form = this.form.value;
    this.isSaving = true;
    let dataToPost = {
      ServiceId: form.serviceId,
      UserId: this.userId
    };
    this.dataService.post(ServerApis.addUserAccessService, dataToPost).subscribe(response => {
      this.isSaving = false;
      if (response.isSuccess) {
        this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
        this.form.reset();
        this.getuserRoleList();
      } else {
        var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
        this.toastrService.error(msg);
      }
    }, error => {
      this.isSaving = false;
    });

  }




  delete(row) {

    Swal.fire({
      title: 'حذف',
      text: 'آیا برای حذف اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر'
    }).then(result => {
      if (result.value) {
        row.loading = true;
        
        this.dataService.get(ServerApis.deleteUserAccessService, { userId: row.userId, serviceId: row.serviceId }).subscribe(response => {
          row.loading = false;
          if (response.isSuccess) {
            this.toastrService.success('حذف اطلاعات با موفقیت انجام شد.');
            this.getuserRoleList();
          } else {
            let msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        }, error => {
          row.loading = false;
        })
      }
    });
  }



}
