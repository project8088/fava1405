import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { BaseDataModel } from '@core/models/base-data-model';
import { CitizenProfileComponent } from '../profile.component';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { KarjoGlobalInformationDto } from '@core/models/citizen/global-information';
import { Observable } from 'rxjs';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.scss'],
  standalone: false,
})
export class CitizenEditProfileComponent extends AppBase implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  userCode: string='';
  personalForm: FormGroup;
  baseEnums: any = {};
  loadingEnums: boolean = true;

  loadingState: boolean=false;
  stateList: BaseDataModel[] = [];
  filteredState=new Observable<any[]>();

  lastModifiedOnDate?: string;
  citizenInfo: KarjoGlobalInformationDto;
  constructor(
    private customValidator: CustomFormValidators,
    private profileComponent: CitizenProfileComponent,
  ) {
    super();
    this.route.parent.params.subscribe((p) => {
      this.userCode = p['id'] && p['id'] != '0' ? p['id'] : '';
      this.getPersonalInfo();
    });

    this.personalForm = this.fb.group({
      gender: [null, [Validators.required]],
      mobile: [null, [Validators.required, this.customValidator.checkMobileNumber]],
      email: [null, [this.customValidator.checkEmail]],
      nationalCode: [{ value: '', disabled: true }, [this.customValidator.checkNationalCode]],

      firstName: [null, [Validators.required, this.customValidator.checkPersianCharacters]],
      lastName: [null, [Validators.required, this.customValidator.checkPersianCharacters]],
      fatherName: [null, [Validators.required, this.customValidator.checkPersianCharacters]],

      dateOfBirth: [null, [Validators.required]],

      marital: [null, [Validators.required]],
      phoneNumber: [null, [this.customValidator.checkPhoneNumber]],
      fullAddress: [null, []],
      state: [null, []],
    });
  }

  ngOnInit() {
    this.getBaseEnums();

    this.filteredState = this.personalForm.get('state')!.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((value) => {
        return this._getStates(value);
      }),
    );
  }

  private _getStates(value: string) {
    if (!value || typeof value !== 'string') return this.filteredState;

    const filterValue = value.toLowerCase();

    this.loadingState = true;
    return this.dataService
      .get(ServerApis.citySearch, {
        query: filterValue,
        offset: 0,
        count: 20,
      })
      .pipe(
        map(
          (response) => {
            if (response) {
              this.loadingState = false;
              if (response.isSuccess) return response.data;
              else {
                let msg = response.messages
                  ? response.messages
                  : 'در یافت اطلاعات از سرور با خطا مواجه شده است.';
                this.toastrService.error(msg);
              }
            }
          },
          (error:any) => {
            this.toastrService.error('خطا در ارتباط با سرور!');
            this.loadingState = false;
          },
        ),
      );
  }

  /**
   * دریافت اطلاعات  پایه
   *
   * */
  getBaseEnums() {
    this.loadingEnums = true;
    this.dataService.getEnums().subscribe(
      (response) => {
        this.loadingEnums = false;
        if (response) {
          this.baseEnums.military = response.military;
          this.baseEnums.maritalStatus = response.maritalStatus;
        }
      },
      (error:any) => {
        this.toastrService.error('خطا در ارتباط با سرور!');
        this.loadingEnums = false;
      },
    );
  }

  getPersonalInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getCitizenBaseInfoByAdmin, { userCode: this.userCode })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response && response.isSuccess) {
            this.lastModifiedOnDate = response.data.lastModifiedOnDate;
            (this, (this.citizenInfo = response.data));

            this.personalForm.setValue({
              gender: response.data.gender,
              mobile: response.data.mobile,
              email: response.data.eMail || null,
              nationalCode: response.data.nationCode,
              firstName: response.data.firstName,
              lastName: response.data.lastName,
              fatherName: response.data.fatherName,
              creationDate: response.data.creationDate ? new Date(response.data.creationDate) : '',
              date_SabtConfirm: response.data.date_SabtConfirm
                ? new Date(response.data.date_SabtConfirm)
                : '',
              birthDate: response.data.date_SabtConfirm ? new Date(response.data.birthDate) : '',

              educationField: response.data.educationField,
              educationGroup: response.data.educationGroup,
              educationGroupId: response.data.educationGroupId,

              fullAddress: response.data.fullAddress,
              dateOfBirth: '',
              marital: 1,
              phoneNumber: '09139879696',
              state: {
                value: response.data.cityId,
                text: response.data.city,
              },
            });
            this.changeGender();
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error:any) => {
          this.loading = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
  }

  savePersonalInfo() {
    if (this.personalForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.personalForm.markAllAsTouched();
        return ;
    }
    var formValue = this.personalForm.value;

    this.isSaving = true;

    this.dataService
      .post(ServerApis.updateGlobalInformation, {
        UserId: 0, // this.userId,
        UserCode: '',
        LastModifiedOnDate: '2020-10-08',
        Gender: formValue.gender,
        FirstName: formValue.firstName,
        LastName: formValue.lastName,
        FatherName: formValue.fatherName,
        NationalCode: this.citizenInfo.nationalCode,
        MobileNumber: formValue.mobile,
        Email: formValue.email,

        DateOfBirth: formValue.dateOfBirth
          ? this.dataService.formatDate(formValue.dateOfBirth)
          : '',
        Marital: formValue.marital,

        Address: formValue.fullAddress,
        City: formValue.state.text,
        CityId: +formValue.state.key,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response && response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
            this.profileComponent.getPersonalInfo();
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error:any) => {
          this.isSaving = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
  }

  /**
   * for bind object in autocomplete
   * @param item
   */
  displayFn(item:any): string {
    return item && item.text ? item.text : '';
  }
  /**
   * for bind object in select
   * @param item
   */
  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.key === c2.key : c1 === c2;
  }

  changeGender() {
    if (this.personalForm.get('gender')?.value == false) {
      this.personalForm.get('soldierState')?.setValue(null);
      this.personalForm.get('soldierState')?.clearValidators();
      this.personalForm.get('soldierState')?.updateValueAndValidity();
    } else {
      this.personalForm.get('soldierState')?.setValidators([Validators.required]);
      this.personalForm.get('soldierState')?.updateValueAndValidity();
    }
  }
}
