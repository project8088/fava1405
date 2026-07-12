import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'company-base-info',
  templateUrl: './base-info.component.html',
  styleUrls: ['./base-info.component.scss'],
  standalone: false,
})
export class CompanyBaseInfoComponent extends AppBase implements OnInit {
  loading?: boolean;
  provinceList: any[] = [];
  companyId: string = '';
  form: FormGroup;
  isSaving = false;
  baseInfo: any = {};

  constructor(private customValidators: CustomFormValidators) {
    super();
    this.form = this.fb.group({
      companyName: [null, [Validators.required, Validators.maxLength(500)]],
      englishName: [
        null,
        [
          Validators.required,
          Validators.maxLength(40),
          this.customValidators.checkEnglishCharacters,
        ],
      ],
      establishedYear: [
        null,
        [Validators.required, Validators.maxLength(4), Validators.minLength(4)],
      ],
      txtTinNo: [null, [Validators.required, Validators.maxLength(20)]],
      txtRegNO: [null, [Validators.required, Validators.maxLength(20)]],
      companyOwnerType: [null, [Validators.required]],
      activityLicenseType: [null, [Validators.required]],
      activityLicense: [null, [Validators.required]],
      activityLicenseDate: [null, [Validators.required]],
      earthCondition: [null, [Validators.required]],
      fieldOfActivity: [null, [Validators.required]],
      managerNationCode: [null, [Validators.required, this.customValidators.checkNationalCode]],
      managerName: [null, [Validators.required, Validators.maxLength(100)]],
      unitArea: [null, [Validators.required, Validators.maxLength(20)]],
      areaOfGreenSpace: [null, [Validators.required, Validators.maxLength(20)]],
      buildingArea: [null, [Validators.required, Validators.maxLength(20)]],
      buildingLicenseArea: [null, [Validators.required, Validators.maxLength(20)]],
    });

    this.route.params.subscribe((p) => {
      if (p['id'] != '0' && p['id']) this.companyId = p['id'];
      this.getAddressInfo();
    });
  }

  ngOnInit(): void {
    this.dataService.getEnums().subscribe((response) => {
      this.baseInfo = {
        activityLicense: response.activityLicense,
        companyOwnerType: response.companyOwnerType,
        activityLicenseType: response.activityLicenseType,
        earthCondition: response.earthCondition,
        companyFieldOfActivity: response.companyFieldOfActivity,
      };
    });
  }

  getAddressInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getCompanyBaseInfo, {
        companyId: this.companyId,
      })
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response.isSuccess && response.data) {
            this.companyId = response.data.companyId;
            this.form.setValue({
              companyName: response.data.companyName,
              englishName: response.data.englishName,
              establishedYear: response.data.establishedYear,
              txtTinNo: response.data.txtTinNo,
              txtRegNO: response.data.txtRegNO,
              companyOwnerType: response.data.companyOwnerType,
              activityLicenseType: response.data.activityLicenseType,
              activityLicense: response.data.activityLicense,
              activityLicenseDate: response.data.activityLicenseDate,
              earthCondition: response.data.earthCondition,
              fieldOfActivity: response.data.fieldOfActivity,
              managerNationCode: response.data.managerNationCode,
              managerName: response.data.managerName,
              unitArea: response.data.unitArea,
              areaOfGreenSpace: response.data.areaOfGreenSpace,
              buildingArea: response.data.buildingArea,
              buildingLicenseArea: response.data.buildingLicenseArea,
            });
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
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
      CompanyId: this.companyId ? +this.companyId : null,
      companyName: form.companyName,
      englishName: form.englishName,
      establishedYear: form.establishedYear,
      txtTinNo: form.txtTinNo,
      txtRegNO: form.txtRegNO,
      companyOwnerType: form.companyOwnerType,
      activityLicenseType: form.activityLicenseType,
      activityLicense: form.activityLicense,
      activityLicenseDate: form.activityLicenseDate
        ? this.dataService.formatDate(form.activityLicenseDate)
        : null,
      earthCondition: form.earthCondition,
      fieldOfActivity: form.fieldOfActivity,
      managerNationCode: form.managerNationCode,
      managerName: form.managerName,
      unitArea: +form.unitArea,
      areaOfGreenSpace: +form.areaOfGreenSpace,
      buildingArea: +form.buildingArea,
      buildingLicenseArea: +form.buildingLicenseArea,
    };
    this.dataService
      .post(ServerApis.updateCompnayBaseInfo, dataToPost)
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
