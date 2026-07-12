import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'citizen-base-info',
  templateUrl: './base-info.component.html',
  styleUrls: ['./base-info.component.scss'],
  standalone: false,
})
export class AdminCitizenBaseInfoComponent extends AppBase implements OnInit {
  loading?: boolean;
  provinceList: any[] = [];
  userCode: string = '';
  form: FormGroup;
  isSaving = false;
  baseInfo: any = {};
  loadingEnums: boolean = true;
  baseEnums: any = {};
  userStatus?: number;

  constructor(private customValidators: CustomFormValidators) {
    super();
    this.form = this.fb.group({
      firstName: [null, [Validators.required, Validators.maxLength(40)]],
      lastName: [null, [Validators.maxLength(40), Validators.maxLength(500)]],
      fatherName: [null, [Validators.maxLength(40), Validators.maxLength(500)]],
      birthDate: [{ value: '', disabled: false }, [Validators.required]],
      nationalCode: [{ value: '', disabled: true }, [this.customValidators.checkNationalCode]],
      educationStatues: [null],
      educationLevel: [null],
      educationTitle: [null],
      educationGroup: [null],
      jobGroup: [null],
      jobTitle: [null],
      mariageStatus: [null, [Validators.required]],
      gender: [false, [Validators.required]],
      mobileNumber: [null, [Validators.required]],
    });

    this.route.params.subscribe((p) => {
      if (p['id'] != '0' && p['id']) this.userCode = p['id'];
      this.getPersonalInfo();
    });
  }

  ngOnInit(): void {
    this.getBaseEnums();
    this.getEducationGroups();
  }

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
      (error: any) => {
        this.toastrService.error('خطا در ارتباط با سرور!');
        this.loadingEnums = false;
      },
    );
  }

  getEducationGroups() {
    this.loadingEnums = true;
    this.dataService
      .get(ServerApis.getEducationGroups)
      .pipe(
        finalize(() => {
          this.loadingEnums = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response) {
            this.baseEnums.educationGroups = response;
          }
        },
        (error: any) => {
          this.toastrService.error('خطا در ارتباط با سرور!');
        },
      );
  }

  getPersonalInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getCitizenBaseInfoByAdmin, { userCode: this.userCode })
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        this.loading = false;
        if (response && response.isSuccess) {
          this.form.patchValue({
            gender: response.data.gender,
            nationalCode: response.data.nationCode,
            firstName: response.data.firstName,
            lastName: response.data.lastName,
            fatherName: response.data.fatherName,
            educationTitle: response.data.educationField,
            educationGroup: String(response.data.educationGroupId),
            educationGroupId: response.data.educationGroupId,

            birthDate: response.data.birthDate,
            mariageStatus: response.data.mariageStatus,
            educationStatues: response.data.educationStatues,
            educationLevel: response.data.educationLevel,
            jobGroup: response.data.jobGroupId,
            jobTitle: response.data.jobTitle,
            mobileNumber: response.data.mobileNumber,
          });
          this.userStatus = response.data.sabtStatus;
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
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
      citizenId: 0,
      userCode: this.userCode,
      gender: form.gender,
      nationalCode: form.nationalCode,
      firstName: form.firstName,
      lastName: form.lastName,
      fatherName: form.fatherName,
      educationTitle: form.educationTitle,
      educationGroup: form.educationGroup,
      educationGroupId: form.educationGroupId,
      activityLicense: form.activityLicense,
      birthDate: form.birthDate ? this.dataService.formatDate(form.birthDate) : null,
      mariageStatus: form.mariageStatus,
      educationStatues: form.educationStatues,
      educationLevel: form.educationLevel,
      mobileNumber: form.mobileNumber,
      jobGroup: form.jobGroup,
      jobTitle: form.jobTitle,
    };
    this.dataService
      .post(ServerApis.updteCitizenByAdmin, dataToPost)
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
