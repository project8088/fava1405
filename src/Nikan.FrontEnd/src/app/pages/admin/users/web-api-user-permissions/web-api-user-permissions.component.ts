import { Component, OnInit } from '@angular/core';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'adm-web-api-user-permissions',
  templateUrl: './web-api-user-permissions.component.html',
  styleUrls: ['./web-api-user-permissions.component.scss'],
  standalone: false,
})
export class AdminWebApiUserPermissionsComponent extends AppBase implements OnInit {
  userId?: string;
  data: any[] = [];
  isLoadingResults: boolean = true;
  isSaving = false;
  constructor(private customValidator: CustomFormValidators) {
    super();
    this.route.params.subscribe((p) => {
      this.userId = p['id'];
    });
  }

  ngOnInit() {
    this.getList();
  }
  getList() {
    this.isLoadingResults = true;
    this.data = [];
    this.dataService
            .get(ServerApis.getWebApiPermissionList, {
              userId: this.userId,
            })
      .pipe(
        finalize(() => {
          this.isLoadingResults = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response: any) => {
                if (response) {
                  this.data = response;
                } else {
                  let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                  this.toastrService.error(msg);
                }
              }, (error: any) => {
              });
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
            .post(ServerApis.addWebApiUserPermissions, {
              userId: this.userId,
              Permissions: selectedPermissions,
            })
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
                if (response.isSuccess) {
                  this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
                } else {
                  let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                  this.toastrService.error(msg);
                }
              }, (error: any) => {
              });
  }
}
