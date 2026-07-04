import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '../../../../../core/custom-validator/form-validation';
import { DataService } from '../../../../../core/services/data-service.service';
import { ServerApis } from '../../../../../core/server-apis';

@Component({
  selector: 'app-adm-add-user-dialog',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss'],
})
export class AdminAddUserDialogComponent implements OnInit {
  isSaving: boolean;
  userForm: FormGroup;
  loading: boolean = true;

  organizationList: any = ([] = []);
  unitList: any = ([] = []);
  periorityList: any[] = [];
  loadingUnit: boolean;
  loadingData: boolean = true;
  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<AdminAddUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private customValidator: CustomFormValidators,
    private dataService: DataService,
  ) {
    this.userForm = this.fb.group(
      {
        id: [null],
        displayName: [null, [Validators.required]],
        username: [
          null,
          [Validators.required, this.customValidator.checkEnglishAndNumberCharacters],
        ],
        password: [null, [Validators.required]],
        confirmPassword: [null, [Validators.required]],

        mobile: [null, [Validators.required, this.customValidator.checkMobileNumber]],
        email: [null, [this.customValidator.checkEmail]],

        organization: [null],
        organizationalUnit: [null],

        isGuardRole: [true],
      },
      { validator: this.checkPasswords },
    );
  }

  /**
   * بررسی یکی بودن کلمه عبور و تائید آن
   */
  checkPasswords(group: FormGroup) {
    let pass = group.controls.password.value;
    let confirmPassword = group.controls.confirmPassword.value;

    return pass === confirmPassword ? null : { notSame: true };
  }

  ngOnInit() {
    this.getOrganizations();
  }

  saveInfo() {
    if (this.userForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.userForm.markAllAsTouched();
      return false;
    }
    var formValue = this.userForm.value;

    this.isSaving = true;

    this.dataService
      .post(ServerApis.adminRegisterUser, {
        DisplayName: formValue.displayName,
        Email: formValue.email,
        MobileNumber: formValue.mobile,
        UserName: formValue.username,
        Password: formValue.password,
        IsGuardRole: formValue.isGuardRole,
        OrganizationalUnitId: formValue.organizationalUnit.key,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response && response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
            this.matDialogRef.close(true);
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

  getOrganizations() {
    this.loadingData = true;

    this.dataService.get(ServerApis.getAllOrganizational, {}).subscribe(
      (response) => {
        this.loadingData = false;
        if (response.isSuccess) {
          this.organizationList = response.data ? response.data : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loadingData = false;
      },
    );
  }

  getUnitsOfOrganization() {
    this.loadingUnit = true;

    this.dataService
      .get(ServerApis.getAllOrganizationalUnitByOrganId, {
        organId: this.userForm.get('organization').value.key,
      })
      .subscribe(
        (response) => {
          this.loadingUnit = false;
          if (response.isSuccess) {
            this.unitList = response.data ? response.data : [];
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loadingUnit = false;
        },
      );
  }
}
