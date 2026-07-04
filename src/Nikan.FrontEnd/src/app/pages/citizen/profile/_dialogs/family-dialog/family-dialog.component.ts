import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { startWith, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '../../../../../core/server-apis';
import { MatStepper } from '@angular/material/stepper';
import { HelperService } from '@core/services/helper.service';
import { citizenFamilyModel } from '@core/models/citizen/family.model';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-family-dialog',
  templateUrl: './family-dialog.component.html',
  styleUrls: ['./family-dialog.component.scss'],
  standalone: false,
})
export class CitizenFamilyDialogComponent extends AppBase implements OnInit {
  isSaving: boolean;

  id: string;
  userId: string;
  loading: boolean = true;

  loadingEnums: boolean = true;
  baseEnums: any = {};

  family: citizenFamilyModel;

  loadingFieldStudies: boolean;
  filteredFieldStudies: Observable<any[]>;
  selectedFieldStudies: any[] = [];

  firstFormGroup: FormGroup;
  registerForm: FormGroup;
  secoundFormGroup: FormGroup;
  familyForm: FormGroup;

  familyIsRegister: boolean = true;

  states: [] = [];
  isfahanCities;

  constructor(
    private matDialogRef: MatDialogRef<CitizenFamilyDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
    private helperService: HelperService,
  ) {
    super();
    this.dataService.getEnums().subscribe((data) => {
      this.baseEnums.maritalStatus = data['maritalStatus'];
      this.baseEnums.educationStatues = data['educationStatues'];
      this.baseEnums.educationLevel = data['educationLevel'];
      this.baseEnums.jobGroup = data['jobGroup'];
      this.baseEnums.passwordQuestion = data['passwordQuestion'];
      this.baseEnums.familyRelationships = data['familyRelationships'];
    });

    this.familyForm = this.fb.group({
      nationCode: [null, [Validators.required]],
      familyRelation: [null, [Validators.required]],
      birthDate: [null, [Validators.required]],
    });

    this.helperService.getProvinces().subscribe((data) => {
      this.states = data as [];
    });
  }

  ngOnInit() {
    this.getBaseEnums();
    this.getEducationGroups();
    this.firstFormGroup = this.fb.group({
      nationCode: [null, [Validators.required, this.customValidator.checkNationalCode]],
    });

    this.registerForm = this.fb.group({
      familyRelation: [null, [Validators.required]],
      nationCode: [{ value: null, disabled: true }, [Validators.required]],

      firstName: [null, [Validators.required]],
      lastName: [null, [Validators.required]],
      mariageStatus: [null, [Validators.required]],
      fatherName: [null, [Validators.required]],
      birthDate: [null, [Validators.required]],
      gender: [null, [Validators.required]],

      phone: [null, [Validators.required]],
      mobile: [null],
      region: [null],
      cityId: [null, [Validators.required]],
      street: [null, [Validators.required]],
      alley: [null],
      plaque: [null],

      postalCode: [null],
      eMail: [null],

      educationStatues: [null, [Validators.required]],
      educationGroup: [null],
      educationTitle: [null],
      educationLevel: [null],

      jobGroup: [null, [Validators.required]],
      jobTitle: [null],
    });

    this.registerForm.get('familyRelation').valueChanges.subscribe((value) => {
      this.registerForm.get('gender').enable();
      this.registerForm.get('mariageStatus').enable();

      switch (value) {
        case 0: // father
          this.registerForm.get('gender').setValue(true);
          this.registerForm.get('gender').disable();
          break;

        case 1: // mother
          this.registerForm.get('gender').setValue(false);
          this.registerForm.get('gender').disable();
          break;

        case 2: // wife/husband
          this.registerForm.get('mariageStatus').setValue(1);
          this.registerForm.get('mariageStatus').disable();
          break;

        case 4: // brother
          this.registerForm.get('gender').setValue(true);
          this.registerForm.get('gender').disable();
          break;

        case 5: // sister
          this.registerForm.get('gender').setValue(false);
          this.registerForm.get('gender').disable();
          break;
      }
    });

    this.registerForm.get('educationStatues').valueChanges.subscribe((value) => {
      if (+value !== 3) {
        this.registerForm.get('educationLevel').setValidators(Validators.required);
        this.registerForm.get('educationGroup').setValidators(Validators.required);
      } else {
        this.registerForm.get('educationLevel').clearValidators();
        this.registerForm.get('educationGroup').clearValidators();
      }

      this.registerForm.get('educationLevel').updateValueAndValidity();
      this.registerForm.get('educationGroup').updateValueAndValidity();
    });

    this.registerForm.get('familyRelation').valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((value) => {
        debugger;
        return value;
      }),
    );

    if (this._data) {
      this.family = this._data;
      this.loading = true;
      this.dataService
        .get(ServerApis.getMyFamilyBaseInfo + '?familyId=' + this.family.familyCitizenId)
        .subscribe((data) => {
          const family = data.data;
          this.loading = false;
          this.registerForm.patchValue({
            familyRelation: this.family.familyRelation,
            nationCode: family.nationCode,

            firstName: family.firstName,
            lastName: family.lastName,
            mariageStatus: family.mariageStatus,
            fatherName: family.fatherName,
            birthDate: family.birthDate,
            gender: family.gender,

            phone: family.phone,
            mobile: family.mobile,
            region: family.region,
            stateId: family.stateId,
            cityId: family.cityId,
            street: family.street,
            alley: family.alley,
            plaque: family.plaque,

            postalCode: family.postalCode,
            eMail: family.eMail,

            educationStatues: family.educationStatues,
            educationTitle: family.educationTitle,
            educationGroup: String(family.educationGroupId),
            educationLevel: family.educationLevel,
            jobGroup: family.jobGroupId,
            jobTitle: family.jobTitle,
          });
        });
    }
  }

  setNationCodeInfamilyForm() {
    this.familyForm.patchValue({
      nationCode: this.firstFormGroup.value.nationCode,
    });
    this.familyForm.get('nationCode').disable();
  }
  checkNationCode(stepper: MatStepper) {
    if (this.firstFormGroup.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.firstFormGroup.markAllAsTouched();
      return false;
    }

    var formValue = this.firstFormGroup.value;

    this.isSaving = true;
    this.dataService
      .post(ServerApis.checkRegisterCitizenByNtionCode, {
        formValue,
        nationCode: this.firstFormGroup.value.nationCode,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response && response.isSuccess) {
            this.familyIsRegister = response.data.isRegister;
            stepper.next();
            this.setNationCodeInfamilyForm();
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

  saveFamilyDetails(stepper: MatStepper) {
    if (this._data) this.updateFamilyDetails();
    else this.registerFamilyAsNewUser(stepper);
  }
  registerFamilyAsNewUser(stepper: MatStepper) {
    if (this.registerForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.registerForm.markAllAsTouched();
      return false;
    }

    var formValue = this.registerForm.value;

    this.isSaving = true;
    this.dataService
      .post(ServerApis.addFamilyMemberIfNotAny, {
        familyRelation: formValue.familyRelation,
        nationCode: this.firstFormGroup.value.nationCode,
        gender: formValue.gender,
        firstName: formValue.firstName,
        lastName: formValue.lastName,
        fatherName: formValue.fatherName,
        birthDate: formValue.birthDate,
        mobile: formValue.mobile,
        mariageStatus: formValue.mariageStatus,
        educationStatues: formValue.educationStatues,
        educationGroup: formValue.educationGroup,
        educationLevel: formValue.educationLevel,
        jobTitle: formValue.jobTitle,
        jobGroup: formValue.jobGroup,
        educationTitle: formValue.educationTitle,
        phone: formValue.phone,
        cityId: Number(formValue.cityId),
        region: Number(formValue.region),
        plaque: formValue.plaque,
        alley: formValue.alley,
        postalCode: formValue.postalCode,
        eMail: formValue.eMail,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response && response.isSuccess) {
            this.toastrService.success(response.messages);
            this.familyForm.patchValue({
              familyRelation: formValue.familyRelation,
            });

            // stepper.next();
            // this.matDialogRef.close(response.data);
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

  registerFamily(stepper: MatStepper) {
    if (this.familyForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.familyForm.markAllAsTouched();
      return false;
    }

    var formValue = this.familyForm.value;
    if (formValue.birthDate) formValue.birthDate = this.dataService.formatDate(formValue.birthDate);

    this.isSaving = true;
    this.dataService
      .post(ServerApis.addFamilyMemberIfAny, {
        familyRelation: formValue.familyRelation,
        nationCode: this.firstFormGroup.value.nationCode,
        birthDate: formValue.birthDate,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response && response.isSuccess) {
            this.toastrService.success(response.messages);
            this.familyForm.patchValue({
              familyRelation: formValue.familyRelation,
            });

            // stepper.next();
            this.matDialogRef.close(response.data);
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

  updateFamilyDetails() {
    if (this.registerForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.registerForm.markAllAsTouched();
      return false;
    }

    var formValue = this.registerForm.getRawValue();
    this.isSaving = true;
    this.dataService
      .post(ServerApis.updateFamilyMemberByCitizen, {
        familyCitizenId: this.family.familyCitizenId,
        familyRelation: formValue.familyRelation,
        nationCode: formValue.nationCode,
        gender: formValue.gender,
        firstName: formValue.firstName,
        lastName: formValue.lastName,
        fatherName: formValue.fatherName,
        birthDate: formValue.birthDate,
        mobile: formValue.mobile,
        mariageStatus: formValue.mariageStatus,
        educationStatues: formValue.educationStatues,
        educationGroup: formValue.educationGroup,
        educationLevel: formValue.educationLevel,
        jobTitle: formValue.jobTitle,
        jobGroup: formValue.jobGroup,
        educationTitle: formValue.educationTitle,
        phone: formValue.phone,
        cityId: Number(formValue.cityId),
        region: Number(formValue.region),
        street: formValue.street,
        alley: formValue.alley,
        plaque: formValue.plaque,
        postalCode: formValue.postalCode,
        eMail: formValue.eMail,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response && response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
            this.matDialogRef.close(response.data);
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

    if (this.registerForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.registerForm.markAllAsTouched();
      return false;
    }
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
          this.baseEnums.educationLevel = response.educationLevel ? response.educationLevel : [];

          var grade = [];
          this.baseEnums.educationLevel.forEach((item, index) => {
            if (+item.key > 19) grade.push(item);
          });
          this.baseEnums.educationLevel = grade;
        } else {
          let msg = response.messages
            ? response.messages
            : 'در یافت اطلاعات از سرور با خطا مواجه شده است.';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.toastrService.error('خطا در ارتباط با سرور!');
        this.loadingEnums = false;
      },
    );

    this.helperService.getIsfahanCities().subscribe((data) => {
      this.isfahanCities = data;
    });
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

  displayFn(item): string {
    return item && item.text ? item.text : '';
  }

  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.key === c2.key : c1 === c2;
  }
}
