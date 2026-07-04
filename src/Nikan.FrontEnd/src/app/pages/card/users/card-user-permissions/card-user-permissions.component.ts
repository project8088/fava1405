import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { AuthService } from '@core/authentication/auth.service';
import Swal from 'sweetalert2';
import { ServerApis } from '../../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-card-user-permissions',
  templateUrl: './card-user-permissions.component.html',
  styleUrls: ['./card-user-permissions.component.scss'],
    standalone: false
})
export class CardUserPermissionsComponent extends AppBase implements OnInit {
  userId: string;
  data: any[] = [];
  isLoadingResults: boolean = true;
  isSaving: boolean;
  constructor(
    private customValidator: CustomFormValidators
  ) {
      super();
    this.route.params.subscribe((p) => {
      this.userId = p.id;
    });
  }

  ngOnInit() {
    this.getList();
  }
  getList() {
    this.isLoadingResults = true;
    this.data = [];
    this.dataService
      .get(ServerApis.getCardPermissionList, {
        userId: this.userId,
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
        (error) => {
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
      .post(ServerApis.addCardUserPermissions, {
        userId: this.userId,
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
        (error) => {
          this.isSaving = false;
        },
      );
  }
}
