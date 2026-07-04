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
  selector: 'manage-group-citizen',
  templateUrl: './manage-group-citizen.component.html',
  styleUrls: ['./manage-group-citizen.component.scss'],
})
export class AdminManageGroupsCitizenComponent implements OnInit {
  loading: boolean;
  citizengroupList: any[] = [];
  groupList: any[] = [];
  userCode: string = '';
  form: FormGroup;
  isSaving: boolean;

  constructor(
    private fb: FormBuilder,
    private customValidators: CustomFormValidators,
    private dataService: DataService,
    private toastrService: ToastrService,
    private route: ActivatedRoute,
  ) {
    this.form = this.fb.group({
      groupId: [null, [Validators.required]],
      expireDate: [null],
    });

    this.route.params.subscribe((p) => {
      if (p.id != '0' && p.id) this.userCode = p.id;
      this.getcitizengroupList();
    });
  }

  ngOnInit(): void {
    this.dataService.get(ServerApis.getGroups).subscribe((response) => {
      this.groupList = response.data ? response.data : [];
    });
  }

  getcitizengroupList() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getGroupsCitizensInfo, {
        userCode: this.userCode,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.citizengroupList = response.data ? response.data : [];
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

  save() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
      return false;
    }
    var form = this.form.value;
    this.isSaving = true;
    let dataToPost = {
      GroupId: form.groupId,
      userCode: this.userCode,
      expireDate: form.expireDate,
    };
    this.dataService.post(ServerApis.addCitizenToGroup, dataToPost).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
          this.form.reset();
          this.getcitizengroupList();
        } else {
          var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.isSaving = false;
      },
    );
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
        let dataToPost = {
          GroupId: row.groupId,
          CitizenId: row.citizenId,
        };

        this.dataService.post(ServerApis.removeCitizenFromGroup, dataToPost).subscribe(
          (response) => {
            row.loading = false;
            if (response.isSuccess) {
              this.toastrService.success('حذف اطلاعات با موفقیت انجام شد.');
              this.getcitizengroupList();
            } else {
              let msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
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
}
