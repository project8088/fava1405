import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  MatAutocompleteSelectedEvent,
  MatAutocompleteTrigger,
} from '@angular/material/autocomplete';
import { BaseDataModel } from '@core/models/base-data-model';
import { Observable } from 'rxjs';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-adm-update-user-dialog',
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.scss'],
  standalone: false,
})
export class AdminUpdateUserDialogComponent extends AppBase implements OnInit {
  isSaving=false;
  userForm: FormGroup;
  userId?: string;
  loading: boolean = true;
  bankEnums: any[] = [];
  loadingEnums: boolean = true;

  loadinProvince: boolean;
  provinceList: BaseDataModel[] = [];
  filteredProvince=new Observable<any[]>();

  loadinState: boolean;
  stateList: BaseDataModel[] = [];
  filteredState=new Observable<any[]>();

  loadinPlacement: boolean;
  placementList: BaseDataModel[] = [];
  filteredPlacement=new Observable<any[]>();

  selectedOffices: any[] = [];

  userAccountState: any[] = [];
  organizationList: any = ([] = []);
  unitList: any = ([] = []);
  periorityList: any[] = [];
  loadingUnit: boolean=false;
  loadingData: boolean = true;
  constructor(
    private matDialogRef: MatDialogRef<AdminUpdateUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    if (_data) {
      this.userId = _data.userId;
      this.getUserInfo();
    }

    this.userForm = this.fb.group({
      id: [null],
      displayName: [null, [Validators.required]],
      username: [null, [Validators.required, this.customValidator.checkEnglishORNumberCharacters]],
      mobile: [null, [Validators.required, this.customValidator.checkMobileNumber]],
      email: [null, [this.customValidator.checkEmail]],
      userState: ['', [Validators.required]],
      organization: [null, [Validators.required]],
      organizationalUnit: [null, [Validators.required]],
      deactivationDate: [null],
      userStateDescriptionForUser: [''],
      userStateDescriptionForAdmin: [''],
    });
  }

  ngOnInit() {
    this.getOrganizations();
    this.dataService.getEnums().subscribe((response) => {
      this.userAccountState = response.userAccountState ? response.userAccountState : [];
    });
  }

  getUserInfo() {
    this.loading = true;
    //todo
    this.dataService.get(ServerApis.getUserAccountInfo, { userId: this.userId }).subscribe(
      (response) => {
        this.loading = false;
        if (response.isSuccess) {
          this.userForm.setValue({
            id: response.data.userId,
            displayName: response.data.displayName,
            username: response.data.userName,
            mobile: response.data.mobileNumber,
            email: response.data.emailAddress,
            userState: response.data.userAccountState,
            deactivationDate: response.data.deactivationDate,
            userStateDescriptionForUser: response.data.userStateDescriptionForUser
              ? response.data.userStateDescriptionForUser
              : '',
            userStateDescriptionForAdmin: response.data.userStateDescriptionForAdmin
              ? response.data.userStateDescriptionForAdmin
              : '',
            organization: response.data.organization,
            organizationalUnit: response.data.organizationalUnit,
          });

          this.getUnitsOfOrganization();
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
          this.matDialogRef.close(false);
        }
      },
      (error:any) => {
        this.loading = false;
        this.matDialogRef.close(false);
      },
    );
  }

  displayFn(item:any): string {
    return item && item.text ? item.text : '';
  }

  saveInfo() {
    if (this.userForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.userForm.markAllAsTouched();
        return ;
    }

    var formValue = this.userForm.value;

    this.isSaving = true;
    this.dataService
      .post(ServerApis.updateAccount, {
        UserId: this.userId,
        DisplayName: formValue.displayName,
        deactivationDate: formValue.deactivationDate,
        MobileNumber: formValue.mobile,
        Email: formValue.email,
        UserAccountState: formValue.userState,
        OrganizationalUnitId: formValue.organizationalUnit.key,
        userStateDescriptionForUser: formValue.userStateDescriptionForUser,
        userStateDescriptionForAdmin: formValue.userStateDescriptionForAdmin,
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
        (error:any) => {
          this.isSaving = false;
        },
      );
  }

  changeUserState() {
    if (this.userForm.get('userState')?.value == 0 || this.userForm.get('userState')?.value == 3) {
      this.userForm.get('userStateDescriptionForUser')?.setValidators([Validators.required]);
      this.userForm.get('userStateDescriptionForAdmin')?.setValidators([Validators.required]);
      this.userForm.get('userStateDescriptionForUser')?.updateValueAndValidity();
      this.userForm.get('userStateDescriptionForAdmin')?.updateValueAndValidity();
    } else {
      this.userForm.get('userStateDescriptionForUser')?.clearValidators();
      this.userForm.get('userStateDescriptionForAdmin')?.clearValidators();
      this.userForm.get('userStateDescriptionForUser')?.updateValueAndValidity();
      this.userForm.get('userStateDescriptionForAdmin')?.updateValueAndValidity();
    }
  }

  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.key === c2.key : c1 === c2;
  }

  /**
   * حذف از اتوکاملیت چیپ
   * @param fruit
   */
  removeAutoChip(list: any[], item: any): void {
    const index = list.indexOf(item);

    if (index >= 0) {
      list.splice(index, 1);
    }
  }

  /**
   * انتخاب اتوکاملیت و اضافه کردن به لیست چیپ
   * @param list
   * @param formControl
   * @param input
   * @param event
   * @param Trigger
   */
  selectedAutoChip(
    list: any[],
    formControl:string,
    input: any,
    event: MatAutocompleteSelectedEvent,
    Trigger: MatAutocompleteTrigger,
  ): void {
    const index = list.findIndex((l) => l.value == event.option.value.value);
    if (index >= 0)
      this.toastrService.warning(event.option.value.text + ' را قبلاً انتخاب کرده اید.', 'تکراری!');
    else list.push(event.option.value);
    input.value = '';
    this.userForm.get(formControl)?.setValue(null);
    setTimeout(() => {
      Trigger.openPanel();
    }, 100);
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
      (error:any) => {
        this.loadingData = false;
      },
    );
  }

  getUnitsOfOrganization() {
    this.loadingUnit = true;
    this.dataService
      .get(ServerApis.getAllOrganizationalUnitByOrganId, {
        organId: this.userForm.get('organization')?.value.key,
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
        (error:any) => {
          this.loadingUnit = false;
        },
      );
  }
}
