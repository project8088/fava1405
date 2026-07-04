import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '../../../../../core/custom-validator/form-validation';
import { ServerApis } from '../../../../../core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-adm-add-webapi-users-dialog',
  templateUrl: './add-webapi-users.component.html',
  styleUrls: ['./add-webapi-users.component.scss'],
  standalone: false,
})
export class AdminAddWebApiUserDialogComponent extends AppBase implements OnInit {
  isSaving=false;
  userForm: FormGroup;
  loading: boolean = true;
  appList: any[] = [];
  baseEnums: any = {};
  organizationList: any = ([] = []);
  unitList: any = ([] = []);
  periorityList: any[] = [];
  loadingUnit: boolean;
  loadingData: boolean = true;
  constructor(
    private matDialogRef: MatDialogRef<AdminAddWebApiUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.userForm = this.fb.group(
      {
        id: [null],
        displayName: [null, [Validators.required]],
        username: [
          null,
          [Validators.required, this.customValidator.checkEnglishAndNumberCharacters],
        ],
        password: [null, [Validators.required]],
        serviceId: [null, [Validators.required]],
        confirmPassword: [null, [Validators.required]],
        mobile: [null, [Validators.required, this.customValidator.checkMobileNumber]],
        email: [null, [this.customValidator.checkEmail]],
        organization: [null],
        organizationalUnit: [null],
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
    this.dataService.get(ServerApis.getBaseListAppService).subscribe((response) => {
      this.appList = response.data ? response.data : [];
    });
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
      .post(ServerApis.webApiUserRegister, {
        DisplayName: formValue.displayName,
        Email: formValue.email,
        MobileNumber: formValue.mobile,
        AccessServiceId: +formValue.serviceId,
        UserName: formValue.username,
        Password: formValue.password,
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
  getAppRegisterList() {
    this.dataService.get(ServerApis.getBaseListAppService).subscribe(
      (response) => {
        this.baseEnums.registerTypes = response.data;
      },
      (error) => {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }
}
