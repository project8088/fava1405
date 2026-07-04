import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { AuthService } from '@core/authentication/auth.service';
import Swal from 'sweetalert2';
import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { AdminUpdateUserDialogComponent } from '../dialogs/update-user/update-user.component';
import { AdminChangePasswordDialogComponent } from '../dialogs/change-user-password/change-user-password.component';
import { AdminAddUserDialogComponent } from '../dialogs/add-user/add-user.component';
@Component({
  selector: 'adm-web-api-user-permissions',
  templateUrl: './web-api-user-permissions.component.html',
  styleUrls: ['./web-api-user-permissions.component.scss'],
})
export class AdminWebApiUserPermissionsComponent implements OnInit {
  userId: string;
  data: any[] = [];
  isLoadingResults: boolean = true;
  isSaving: boolean;
  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private router: Router,
    private fb: FormBuilder,
    private customValidator: CustomFormValidators,
    private route: ActivatedRoute,
  ) {
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
      .get(ServerApis.getWebApiPermissionList, {
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
      .post(ServerApis.addWebApiUserPermissions, {
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
