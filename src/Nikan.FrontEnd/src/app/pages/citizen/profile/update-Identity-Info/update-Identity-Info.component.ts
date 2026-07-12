import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CitizenProfileComponent } from '../profile.component';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { HelperService } from '@core/services/helper.service';
import { KarjoGlobalInformationDto } from '@core/models/citizen/global-information';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'app-update-Identity-Info',
  templateUrl: './update-Identity-Info.component.html',
  styleUrls: ['./update-Identity-Info.component.scss'],
  standalone: false,
})
export class CitizenUpdateIdentityInfoComponent extends AppBase implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  userId?: string;
  personalForm: FormGroup;
  baseEnums: any = {};
  loadingEnums: boolean = true;
  info: any;

  lastModifiedOnDate?: string;
  citizenInfo?: KarjoGlobalInformationDto;

  userStatus?: number;

  constructor(
    private helperService: HelperService,
    private customValidator: CustomFormValidators,
    private profileComponent: CitizenProfileComponent,
  ) {
    super();
    this.getIdentityInfo();
    this.personalForm = this.fb.group({
      gender: [null, [Validators.required]],
      nationalCode: [{ value: '' }],
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
      birthDate: [{ value: '', disabled: false }, [Validators.required]],
      identityId: [{ value: '' }],
    });
  }

  ngOnInit() {}

  getIdentityInfo() {
    this.loading = true;
    this.dataService.get(ServerApis.getIdentityInformationByCitizen)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response && response.isSuccess) {
                this.lastModifiedOnDate = response.data.lastModifiedOnDate;
                (this, (this.citizenInfo = response.data));
                this.info = response.data;
                this.personalForm.patchValue({
                  gender: response.data.gender,
                  nationalCode: response.data.nationCode,
                  firstName: response.data.firstName,
                  lastName: response.data.lastName,
                  fatherName: response.data.fatherName,

                  birthDate: response.data.birthDate ? new Date(response.data.birthDate) : '',

                  identityId: response.data.identityId,
                });

                this.setDisabledFields();
              } else {
                let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            }, (error: any) => {
              this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
            });
  }

  savePersonalInfo() {
    if (this.personalForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.personalForm.markAllAsTouched();
      return;
    }
    var formValue = this.personalForm.value;

    this.isSaving = true;
    this.dataService
            .post(ServerApis.updteIdentityInformationByCitizen, {
              gender: formValue.gender,
              firstName: formValue.firstName,
              identityId: formValue.identityId,
              lastName: formValue.lastName,
              fatherName: formValue.fatherName,
              nationalCode: this.citizenInfo?.nationalCode,
              birthDate: formValue.birthDate ? this.dataService.formatDate(formValue.birthDate) : null,
            })
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
                if (response && response.isSuccess) {
                  this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
                  this.getIdentityInfo();
                } else {
                  let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                  this.toastrService.error(msg);
                }
              }, (error: any) => {
                this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
              });
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
    return options.map((el: { key: number; text: string }) => {
      return { value: String(el.key), text: el.text };
    });
  }

  setDisabledFields() {
    if (this.noEditStatus()) {
      //this.personalForm.get('firstName')?.disable();
      //this.personalForm.get('lastName')?.disable();
      //this.personalForm.get('fatherName')?.disable();
      //this.personalForm.get('birthDate')?.disable();
      //this.personalForm.get('gender')?.disable();
    }
  }
  noEditStatus() {
    return this.userStatus === 3 || this.userStatus === 1;
  }
}
