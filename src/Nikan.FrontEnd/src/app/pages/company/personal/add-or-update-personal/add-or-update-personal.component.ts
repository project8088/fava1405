import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { forkJoin, finalize } from 'rxjs';
import { AuthUser } from '@core/authentication/user.model';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'company-add-or-update-personal',
  templateUrl: './add-or-update-personal.component.html',
  styleUrls: ['./add-or-update-personal.component.scss'],
  standalone: false,
})
export class CompanyAddOrUpdatePersonalComponent extends AppBase implements OnInit, AfterViewInit {
  isUpdate = false;
  id: string = '';
  companyId: string = '';
  isSaving = false;
  userForm: FormGroup;
  loading: boolean = true;
  imageUrl: string = '';
  namePrefixList: any = ([] = []);
  provinceList: any = ([] = []);
  organizationalPositionList: any = ([] = []);
  loadingData: boolean = true;
  baseUrl: string = ServerApis.baseUrl;
  user?: AuthUser | null;
  constructor(private customValidator: CustomFormValidators) {
    super();
    this.user = this.authService.currentUserValue;

    this.route.params.subscribe((p) => {
      this.companyId = p['companyId'];
      if (p['id'] && p['id'] != '0') {
        this.isUpdate = true;
        this.id = p['id'];
        this.getUserInfo();
      } else {
        this.id = '';
        this.isUpdate = false;
      }
    });

    this.userForm = this.fb.group({
      id: [null],
      personelCode: [null, [Validators.required]],
      organizationalPositionId: [null, [Validators.required]],
      namePrefix: [null, [Validators.required]],
      firstName: [
        null,
        [
          Validators.required,
          Validators.maxLength(50),
          this.customValidator.checkPersianCharacters,
        ],
      ],
      lastName: [
        null,
        [
          Validators.required,
          Validators.maxLength(50),
          this.customValidator.checkPersianCharacters,
        ],
      ],
      fatherName: [
        null,
        [
          Validators.required,
          Validators.maxLength(50),
          this.customValidator.checkPersianCharacters,
        ],
      ],
      nationCode: [null, [Validators.required, this.customValidator.checkNationalCode]],
      mobileNumber: [null, [Validators.required, this.customValidator.checkMobileNumber]],
      cellNumber: [null, [Validators.required, this.customValidator.checkPhoneNumber]],
      email: [null, [Validators.required, this.customValidator.checkEmail]],
      province: [null],
      city: [null],
      imageUrl: [null],
      zipCode: [null, [Validators.maxLength(10), Validators.minLength(10)]],
      street: [null, [Validators.maxLength(100)]],
      fullAddress: [null, [Validators.maxLength(1000)]],
      pelak: [null, [Validators.maxLength(100)]],
      office: [null],
      officePhoneNumber: [null, [this.customValidator.checkPhoneNumber]],
      isManagementMembers: [false],
      biography: [''],
      hasSpecificDisease: [false],
      descriptionDisease: [''],
    });
  }

  ngOnInit() {
    this.getBaseData();
  }

  getBaseData() {
    this.loadingData = true;
    forkJoin(
            this.dataService.get(ServerApis.getProvinces),
            this.dataService.getEnums(),
            this.dataService.get(ServerApis.getPositionList),
          )
      .pipe(
        finalize(() => {
          this.loadingData = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(([provinces, enums, positions]) => {
            this.provinceList = provinces.data ? provinces.data : [];
            this.namePrefixList = enums.namePrefix ? enums.namePrefix : [];
            this.organizationalPositionList = positions.data ? positions.data : [];
          });
  }

  ngAfterViewInit() {}

  getUserInfo() {
    this.loading = true;
    this.dataService.get(ServerApis.getPersonelInfo, { id: this.id })
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response && response.isSuccess) {
                this.userForm.patchValue(response.data);
                this.imageUrl = response.data.imageUrl;
              } else {
                let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
                if (this.user?.isAdmin) this.router.navigate(['/company/personal/' + this.companyId]);
                else this.router.navigate(['/company/personal/0']);
              }
            }, (error: any) => {
            });
  }

  getAttachmentId(ev: { uploadUrl: string }) {
    this.imageUrl = ev.uploadUrl;
  }

  saveInfo() {
    if (this.userForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.userForm.markAllAsTouched();
      return;
    }
    var formValue = this.userForm.value;

    this.isSaving = true;

    formValue.cityId = formValue.city.key;
    var params = {
      Id: formValue.id ? formValue.id : '',
      PersonelCode: formValue.personelCode,
      UserCompanyId: this.companyId && this.companyId != '0' ? this.companyId : null,
      OrganizationalPositionId: +formValue.organizationalPositionId,
      NamePrefix: formValue.namePrefix,
      FirstName: formValue.firstName,
      LastName: formValue.lastName,
      FatherName: formValue.fatherName,
      NationCode: formValue.nationCode,
      MobileNumber: formValue.mobileNumber,
      CellNumber: formValue.cellNumber,
      Email: formValue.email,
      CityId: +formValue.city.key,
      ImageUrl: this.imageUrl,
      ZipCode: formValue.zipCode,
      Street: formValue.street,
      FullAddress: formValue.fullAddress,
      Pelak: formValue.pelak,
      Office: formValue.office,
      OfficePhoneNumber: formValue.officePhoneNumber,
      EmployeementOnDate: null,
      IsManagementMembers: formValue.isManagementMembers ? formValue.isManagementMembers : false,
      Biography: formValue.biography ? formValue.biography : '',
      HasSpecificDisease: formValue.hasSpecificDisease ? formValue.hasSpecificDisease : false,
      DescriptionDisease: formValue.descriptionDisease ? formValue.descriptionDisease : '',
    };

    this.dataService.post(ServerApis.addOrUpdatePersonel, params)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response && response.isSuccess) {
                this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
                if (this.user?.isAdmin) this.router.navigate(['/company/personal/' + this.companyId]);
                else this.router.navigate(['/company/personal/0']);
              } else {
                let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            }, (error: any) => {
              this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
            });
  }

  changeHasSpecificDisease() {
    if (this.userForm.get('hasSpecificDisease')?.value == true) {
      this.userForm.get('descriptionDisease')?.setValidators([Validators.required]);
      this.userForm.get('descriptionDisease')?.updateValueAndValidity();
    } else {
      this.userForm.get('descriptionDisease')?.clearValidators();
      this.userForm.get('descriptionDisease')?.updateValueAndValidity();
      this.userForm.get('descriptionDisease')?.setValue('');
    }
  }
}
