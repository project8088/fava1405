import { Component, OnInit } from '@angular/core';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-user-permissions',
  templateUrl: './user-permissions.component.html',
  styleUrls: ['./user-permissions.component.scss'],
  standalone: false,
})
export class AdminUserPermissionsComponent extends AppBase implements OnInit {
  groupId: string = '';
  data: any[] = [];
  isLoadingResults: boolean = true;
  isSaving = false;
  constructor(private customValidator: CustomFormValidators) {
    super();
    this.route.params.subscribe((p) => {
      this.groupId = p['id'];
    });
  }

  ngOnInit() {
    this.getList();
  }
  getList() {
    this.isLoadingResults = true;
    this.data = [];
    this.dataService
      .get(ServerApis.getPermissionList, {
        groupId: this.groupId,
      })
      .subscribe(
        (response: any) => {
          this.isLoadingResults = false;
          if (response) {
            this.data = response;
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {
          this.isLoadingResults = false;
        },
      );
  }

  savePermissions() {
    this.isSaving = true;
    var selectedPermissions = [];
    for (var item in this.data) {
      for (var child of this.data[item]) {
        if (child.selected) selectedPermissions.push(child.key);
      }
    }
    this.dataService
      .post(ServerApis.addPermissions, {
        groupId: this.groupId,
        Permissions: selectedPermissions,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
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
