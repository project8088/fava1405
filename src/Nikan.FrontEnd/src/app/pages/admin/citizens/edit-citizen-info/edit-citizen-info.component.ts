import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { BaseDataModel } from '@core/models/base-data-model';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { HelperService } from '@core/services/helper.service';
import { KarjoGlobalInformationDto } from '@core/models/citizen/global-information';
import { Observable } from 'rxjs';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-edit-citizen-info',
  templateUrl: './edit-citizen-info.component.html',
  styleUrls: ['./edit-citizen-info.component.scss'],
  standalone: false,
})
export class AdminEditCitizenInfoComponent extends AppBase implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  userCode: string;
  form: FormGroup;
  baseEnums: any = {};
  loadingEnums: boolean = true;

  loadingState: boolean;
  stateList: BaseDataModel[] = [];
  filteredState: Observable<any[]>;
  states: any[] = [];
  birthC: ities = new Observable<any>();
  shC: ities = new Observable<any>();

  lastModifiedOnDate?: string;
  citizenInfo: KarjoGlobalInformationDto;
  constructor(
    private helperService: HelperService,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.form = this.fb.group({
      state: [null, []],
      stateId: [null, []],

      birthStateId: [null, []],
      cityOfBirthId: [null, []],
      villageOfBirth: [null, []],
      birthCitySection: [null, []],
      shStateId: [null, []],

      dateOfMarriage: [null, []],
      insuranceNumber: [null, []],
      dateOfEmployeement: [null, []],

      personnelCode: [null, []],
      shCode: [null, []],
      shSerial: [null, []],
      shDate: [null, []],
      shCityId: [null, []],
      shCitySection: [null, []],
      shNote: [null, []],

      soldierState: [null, []],
      endOfMilitary: [null, []],
      religion: [null, []],

      baseEducation: [null, []],
      universityName: [null, []],
      academicGrade: [null, []],
      academicNote: [null, []],
      endOfEducation: [null, []],

      educationStatues: [null],
    });

    this.helperService.getProvinces().subscribe((data) => {
      this.states = data as [];
    });

    this.personalForm = this.fb.group({
      gender: [null, [Validators.required]],
      mobile: [null, [Validators.required, this.customValidator.checkMobileNumber]],
      email: [null, [this.customValidator.checkEmail]],
      nationalCode: [{ value: '', disabled: true }, [this.customValidator.checkNationalCode]],

      firstName: [
        { value: '', disabled: false },
        [Validators.required, this.customValidator.checkPersianCharacters],
      ],
      lastName: [
        { value: '', disabled: false },
        [Validators.required, this.customValidator.checkPersianCharacters],
      ],
      fatherName: [
        { value: '', disabled: false },
        [Validators.required, this.customValidator.checkPersianCharacters],
      ],

      dateOfBirth: [{ value: '', disabled: false }, [Validators.required]],
      educationStatues: [null, [Validators.required]],
      educationLevel: [null, [Validators.required]],
      educationTitle: [null],
      educationGroup: [null, [Validators.required]],
      jobGroup: [null, [Validators.required]],
      jobTitle: [null],
      region: [null],
      postalCode: [null, [Validators.required]],
      marital: [null, [Validators.required]],
      phoneNumber: [null],

      street: [null],
      alley: [null],
      plaque: [null],

      state: [null, []],
      stateId: [null, []],
      cityId: [null, []],
    });

    this.personalForm.get('educationStatues')?.valueChanges.subscribe((value) => {
      if (+value !== 3) {
        this.personalForm.get('educationLevel')?.setValidators(Validators.required);
        this.personalForm.get('educationGroup')?.setValidators(Validators.required);
      } else {
        this.personalForm.get('educationLevel').clearValidators();
        this.personalForm.get('educationGroup').clearValidators();
      }

      this.personalForm.get('educationLevel').updateValueAndValidity();
      this.personalForm.get('educationGroup').updateValueAndValidity();
    });

    this.helperService.getProvinces().subscribe((data) => {
      this.states = this.getListOptions(data);
    });

    this.helperService.getIsfahanCities().subscribe((data) => {
      this.isfahanCities = data;
    });
  }

  ngOnInit() {
    this.getBaseEnums();
    this.getEducationGroups();
    this.getPersonalInfo();

    this.filteredState = this.form.get('state')?.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((value) => {
        return this._getStates(value);
      }),
    );

    this.getBaseEnums();
    this.getEducationGroups();
    this.cities = this.personalForm.get('stateId')?.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((value) => {
        return this.helperService
          .getCitesByParent(value)
          .pipe(map((data) => this.getListOptions(data)));
      }),
    );

    this.filteredState = this.personalForm.get('state')?.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((value) => {
        return this._getStates(value);
      }),
    );
  }

  saveForm() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
        return ;
    }
    var formValue = this.form.value;

    this.isSaving = true;
    this.dataService
      .post(ServerApis.addOrUpdateCitizenProfile, {
        ...formValue,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response && response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
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

  getListOptions(options:{key:number,text:string}[]){
    return options.map((el:{key:number,text:string}) => {
      return { value: +el.key, text: el.text };
    });
  }

  personalForm: FormGroup;
  c: ities = new Observable<any>();
  isfahanCities: any[]=[];

  userStatus?: number;

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
          (error) => {
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
          this.baseEnums.educationLevel = response.educationLevel;
          this.baseEnums.educationStatues = response.educationStatues;
          this.baseEnums.jobGroup = response.jobGroup;
        }
      },
      (error) => {
        this.toastrService.error('خطا در ارتباط با سرور!');
        this.loadingEnums = false;
      },
    );
  }
  getEducationGroups() {
    this.loadingEnums = true;
    this.dataService.get(ServerApis.getEducationGroups).subscribe(
      (response) => {
        this.loadingEnums = false;
        if (response) {
          this.baseEnums.educationGroups = response;
        }
      },
      (error) => {
        this.toastrService.error('خطا در ارتباط با سرور!');
        this.loadingEnums = false;
      },
    );
  }
  getPersonalInfo() {
    this.loading = true;
    this.dataService.get(ServerApis.getCitizenBaseInfoByAdmin).subscribe(
      (response) => {
        this.loading = false;
        if (response && response.isSuccess) {
          this.lastModifiedOnDate = response.data.lastModifiedOnDate;
          (this, (this.citizenInfo = response.data));

          this.personalForm.patchValue({
            gender: response.data.gender,
            mobile: response.data.mobile,
            email: response.data.eMail || null,
            nationalCode: response.data.nationCode,
            firstName: response.data.firstName,
            lastName: response.data.lastName,
            fatherName: response.data.fatherName,
            date_SabtConfirm: response.data.date_SabtConfirm
              ? new Date(response.data.date_SabtConfirm)
              : '',
            birthDate: response.data.date_SabtConfirm ? new Date(response.data.birthDate) : '',

            educationTitle: response.data.educationField,
            educationGroup: String(response.data.educationGroupId),
            educationGroupId: response.data.educationGroupId,

            alley: response.data.alley,
            region: response.data.region,
            postalCode: response.data.postalCode,
            dateOfBirth: response.data.birthDate,
            marital: response.data.mariageStatus,
            educationStatues: response.data.educationStatues,
            educationLevel: response.data.educationLevel,
            jobGroup: response.data.jobGroupId,
            jobTitle: response.data.jobTitle,
            stateId: response.data.city ? response.data.city.parentValue : null,
            cityId: String(response.data.cityId),
            plaque: response.data.plaque,

            phoneNumber: response.data.phone,
            state: {
              value: response.data.cityId,
              text: response.data.city,
            },
          });

          this.userStatus = response.data.sabtStatus;
          this.setDisabledFields();
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
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
      .post(ServerApis.addOrUpdateCitizenProfileByAdmin, {
        gender: formValue.gender,
        firstName: formValue.firstName,
        lastName: formValue.lastName,
        fatherName: formValue.fatherName,
        nationalCode: this.citizenInfo.nationalCode,
        mobile: formValue.mobile,
        eMail: formValue.email || '',
        birthDate: formValue.dateOfBirth ? formValue.dateOfBirth : '',
        mariageStatus: formValue.marital,

        educationStatues: formValue.educationStatues,
        educationGroup: formValue.educationGroup,
        educationTitle: formValue.educationTitle,

        jobTitle: formValue.jobTitle,
        jobGroup: formValue.jobGroup,

        phoneNumber: formValue.phoneNumber,
        postalCode: String(formValue.postalCode),
        alley: formValue.alley || '',
        plaque: formValue.plaque || '',
        street: formValue.street || '',
        region: formValue.region || '',
        cityId: Number(formValue.cityId) || 0,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response && response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.isSaving = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
  }

  setDisabledFields() {
    if (this.noEditStatus()) {
      this.personalForm.get('firstName').disable();
      this.personalForm.get('lastName').disable();
      this.personalForm.get('fatherName').disable();
      this.personalForm.get('dateOfBirth').disable();
      this.personalForm.get('gender').disable();
    }
  }
  noEditStatus() {
    return this.userStatus === 3 || this.userStatus === 1;
  }
}
