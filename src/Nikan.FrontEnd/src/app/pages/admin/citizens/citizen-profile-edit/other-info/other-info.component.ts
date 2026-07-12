import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { BaseDataModel } from '@core/models/base-data-model';
import { Observable, finalize } from 'rxjs';
import { HelperService } from '@core/services/helper.service';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'citizen-other-info',
  templateUrl: './other-info.component.html',
  styleUrls: ['./other-info.component.scss'],
  standalone: false,
})
export class AdminCitizenOtherInfoComponent extends AppBase implements OnInit {
  loading?: boolean;
  provinceList: any[] = [];
  userCode: string = '';
  form: FormGroup;
  isSaving = false;
  baseInfo: any = {};
  loadingEnums: boolean = true;
  baseEnums: any = {};
  stateList: BaseDataModel[] = [];
  filteredState = new Observable<any[]>();
  states: any[] = [];
  birthCities = new Observable<any>();
  shCities = new Observable<any>();
  loadingState: boolean = false;

  constructor(
    private helperService: HelperService,
    private customValidators: CustomFormValidators,
  ) {
    super();
    this.form = this.fb.group({
      provinceShCity: [null],
      shCity: [null, []],

      provinceCityOfBirth: [null],
      cityOfBirth: [null, []],

      villageOfBirth: [null, []],
      birthCitySection: [null, []],

      dateOfMarriage: [null, []],
      insuranceNumber: [null, []],
      dateOfEmployeement: [null, []],

      personnelCode: [null, []],
      shCode: [null, []],
      shSerial: [null, []],
      shDate: [null, []],

      shCitySection: [null, []],
      shNote: [null, []],

      militaryStatus: [null, []],
      endOfMilitary: [null, []],
      religion: [null, []],

      baseEducation: [null, []],
      universityName: [null, []],
      academicGrade: [null, []],
      academicNote: [null, []],
      endOfEducation: [null, []],

      educationStatues: [null],
    });

    this.route.params.subscribe((p) => {
      if (p['id'] != '0' && p['id']) this.userCode = p['id'];
      this.getPersonalInfo();
    });
  }

  ngOnInit(): void {
    this.getBaseEnums();
    this.getProvinces();
  }

  getBaseEnums() {
    this.loadingEnums = true;
    this.dataService.getEnums().subscribe(
      (response) => {
        this.loadingEnums = false;
        if (response) {
          this.baseEnums.soldierState = response.soldierState;
          this.baseEnums.religion = response.religion;
        }
      },
      (error: any) => {
        this.toastrService.error('خطا در ارتباط با سرور!');
        this.loadingEnums = false;
      },
    );
  }

  getProvinces() {
    this.dataService.get(ServerApis.getProvinces).subscribe(
      (response) => {
        this.provinceList = response.data ? response.data : [];
      },
      (error: any) => {},
    );
  }

  getPersonalInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.geCitizenProfileByAdmin, {
        userCode: this.userCode,
      })
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        this.loading = false;
        if (response.isSuccess && response.data) {
          this.userCode = response.data.userCode;
          this.form.setValue({
            provinceCityOfBirth: response.data.provinceCityOfBirth
              ? response.data.provinceCityOfBirth
              : '',
            cityOfBirth: {
              key: response.data.cityOfBirthId,
              text: response.data.cityOfBirth,
            },
            provinceShCity: response.data.provinceShCity ? response.data.provinceShCity : '',
            shCity: {
              key: response.data.shCityId,
              text: response.data.shCity,
            },
            villageOfBirth: response.data.villageOfBirth,
            birthCitySection: response.data.birthCitySection,
            dateOfMarriage: response.data.dateOfMarriage,
            insuranceNumber: response.data.insuranceNumber,
            dateOfEmployeement: response.data.dateOfEmployeement,
            personnelCode: response.data.personnelCode,
            shCode: response.data.shCode,
            shSerial: response.data.shSerial,
            shDate: response.data.shDate,
            shCitySection: response.data.shCitySection,
            shNote: response.data.shNote,
            militaryStatus: response.data.militaryStatus,
            endOfMilitary: response.data.endOfMilitary,
            religion: response.data.religion,

            educationStatues: response.data.educationStatues,
            baseEducation: response.data.baseEducation,
            universityName: response.data.universityName,
            academicGrade: response.data.academicGrade,
            academicNote: response.data.academicNote,
            endOfEducation: response.data.endOfEducation,
          });
        } else {
          var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      });
  }

  save() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
      return;
    }
    var form = this.form.value;
    this.isSaving = true;
    let dataToPost = {
      userCode: this.userCode,
      cityOfBirthId: +form.cityOfBirth.key,
      shCityId: +form.shCity.key,
      birthCitySection: form.birthCitySection,
      academicNote: form.academicNote,
      religion: form.religion,
      baseEducation: form.baseEducation,
      universityName: form.universityName,
      academicGrade: form.academicGrade,
      dateOfMarriage: form.dateOfMarriage ? this.dataService.formatDate(form.dateOfMarriage) : null,
      endOfMilitary: form.endOfMilitary ? this.dataService.formatDate(form.endOfMilitary) : null,
      endOfEducation: form.endOfEducation ? this.dataService.formatDate(form.endOfEducation) : null,
      dateOfEmployeement: form.dateOfEmployeement
        ? this.dataService.formatDate(form.dateOfEmployeement)
        : null,
      shDate: form.shDate ? this.dataService.formatDate(form.shDate) : null,
      insuranceNumber: form.insuranceNumber,
      personnelCode: form.personnelCode,
      shCode: form.shCode,
      shSerial: form.shSerial,
      jobTitle: form.jobTitle,
      militaryStatus: form.militaryStatus,

      shCitySection: form.shCitySection,
      shNote: form.shNote,
      educationStatues: form.educationStatues,
      villageOfBirth: form.villageOfBirth,
    };
    this.dataService
      .post(ServerApis.addOrUpdateCitizenProfileByAdmin, dataToPost)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
  }
}
