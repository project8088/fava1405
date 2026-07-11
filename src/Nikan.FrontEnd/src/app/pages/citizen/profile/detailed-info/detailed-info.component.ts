import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { BaseDataModel } from '@core/models/base-data-model';
import { CitizenProfileComponent } from '../profile.component';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { HelperService } from '@core/services/helper.service';
import { KarjoGlobalInformationDto } from '@core/models/citizen/global-information';
import { Observable } from 'rxjs';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-personal-info',
  templateUrl: './detailed-info.component.html',
  styleUrls: ['./detailed-info.component.scss'],
  standalone: false,
})
export class CitizenDetailedInfoComponent extends AppBase implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  userId?: string;
  form: FormGroup;
  baseEnums: any = {};
  loadingEnums: boolean = true;

  loadingState: boolean = false;
  stateList: BaseDataModel[] = [];
  filteredState = new Observable<any[]>();
  states: any[] = [];
  provinceList: any[] = [];

  birthCities = new Observable<any>();
  shCities = new Observable<any>();

  lastModifiedOnDate?: string;
  citizenInfo?: KarjoGlobalInformationDto;
  constructor(
    private helperService: HelperService,
    private customValidator: CustomFormValidators,
    private profileComponent: CitizenProfileComponent,
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
  }

  ngOnInit() {
    this.getProvinces();
    this.getBaseEnums();
    this.getEducationGroups();

    this.getPersonalInfo();

    this.birthCities = this.form.get('birthStateId')!.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((value) => {
        return this.helperService.getCitesByParent(value).pipe(map((data) => data));
      }),
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
          this.baseEnums.soldierState = response.soldierState;
          this.baseEnums.educationStatues = response.educationStatues;
          this.baseEnums.religion = response.religion;
        }
      },
      (error: any) => {
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
      (error: any) => {
        this.toastrService.error('خطا در ارتباط با سرور!');
        this.loadingEnums = false;
      },
    );
  }
  getPersonalInfo() {
    this.loading = true;
    this.dataService.get(ServerApis.geCitizenProfileByCitizen).subscribe(
      (response) => {
        this.loading = false;
        if (response && response.isSuccess) {
          this.lastModifiedOnDate = response.data.lastModifiedOnDate;
          (this, (this.citizenInfo = response.data));

          this.form.patchValue({
            birthDate: response.data.date_SabtConfirm ? new Date(response.data.birthDate) : '',

            birthStateId: response.data.cityOfBirth?.parentValue,
            cityOfBirthId: String(response.data.cityOfBirthId),
            villageOfBirth: response.data.villageOfBirth,
            birthCitySection: response.data.birthCitySection,

            dateOfMarriage: response.data.dateOfMarriage,
            insuranceNumber: response.data.insuranceNumber,
            dateOfEmployeement: response.data.dateOfEmployeement,

            personnelCode: response.data.personnelCode,
            shCode: response.data.shCode,
            shSerial: response.data.shSerial,
            shDate: response.data.shDate,
            shCityId: String(response.data.shCityId),
            shStateId: response.data.shCity?.parentValue,
            shCitySection: response.data.shCitySection,
            shNote: response.data.shNote,

            soldierState: response.data.soldierState,
            endOfMilitary: response.data.endOfMilitary,
            religion: response.data.religion,

            educationStatues: response.data.educationStatues,
            baseEducation: response.data.baseEducation,
            universityName: response.data.universityName,
            academicGrade: response.data.academicGrade,
            academicNote: response.data.academicNote,
            endOfEducation: response.data.endOfEducation,
          });

          this.shCities = this.form.get('shStateId')!.valueChanges.pipe(
            startWith(response.data.shCity?.parentValue),
            debounceTime(400),
            distinctUntilChanged(),
            switchMap((value) => {
              return this.helperService.getCitesByParent(value);
            }),
          );

          this.birthCities = this.form.get('birthStateId')!.valueChanges.pipe(
            startWith(response.data.cityOfBirth?.parentValue),
            debounceTime(400),
            distinctUntilChanged(),
            switchMap((value) => {
              return this.helperService.getCitesByParent(value);
            }),
          );
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error: any) => {
        this.loading = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  saveForm() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
      return;
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
            this.profileComponent.getPersonalInfo();
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {
          this.isSaving = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
  }

  /**
   * for bind object in autocomplete
   * @param item
   */
  displayFn(item: any): string {
    return item && item.text ? item.text : '';
  }
  /**
   * for bind object in select
   * @param item
   */
  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.key === c2.key : c1 === c2;
  }

  getListOptions(options: { key: number; text: string }[]) {
    debugger;
    return options.map((el: { key: number; text: string }) => {
      return { value: +el.key, text: el.text };
    });
  }
}
