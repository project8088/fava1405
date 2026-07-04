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
  selector: 'adm-organization-unit-groups',
  templateUrl: './organization-unit-groups.component.html',
  styleUrls: ['./organization-unit-groups.component.scss'],
})
export class AdminOrganizationUnitGroupsComponent implements OnInit {
  loading: boolean;
  userGroupAccessList: any[] = [];
  groupList: any[] = [];
  unitId: string = '';
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
    });

    this.route.params.subscribe((p) => {
      if (p.id != '0' && p.id) this.unitId = p.id;
      this.getuserRoleList();
    });
  }

  ngOnInit(): void {
    this.dataService.get(ServerApis.getGroups).subscribe((response) => {
      this.groupList = response.data ? response.data : [];
    });
  }

  getuserRoleList() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getAllUnitGroups, {
        id: this.unitId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.userGroupAccessList = response.data ? response.data : [];
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
      groupId: form.groupId,
      unitId: this.unitId,
    };
    this.dataService.post(ServerApis.addGroupToUnitGroup, dataToPost).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.form.reset();
          this.getuserRoleList();
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

        this.dataService
          .get(ServerApis.removeUnitGroup, { unitId: row.unitId, groupId: row.groupId })
          .subscribe(
            (response) => {
              row.loading = false;
              if (response.isSuccess) {
                this.toastrService.success('حذف اطلاعات با موفقیت انجام شد.');
                this.getuserRoleList();
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
