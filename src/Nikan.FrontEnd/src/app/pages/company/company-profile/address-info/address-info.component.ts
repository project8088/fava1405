import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'company-address-info',
  templateUrl: './address-info.component.html',
  styleUrls: ['./address-info.component.scss'],
  standalone: false,
})
export class CompanyAddressInfoComponent extends AppBase implements OnInit {
  loading?: boolean;
  provinceList: any[] = [];
  companyId: string = '';
  addressForm: FormGroup;
  isSaving = false;

  constructor(private customValidators: CustomFormValidators) {
    super();
    this.addressForm = this.fb.group({
      mobileNumber: [null, [Validators.required, this.customValidators.checkMobileNumber]],
      mobileNumber2: [null, [this.customValidators.checkMobileNumber]],
      mobileNumber3: [null, [this.customValidators.checkMobileNumber]],
      cellNumber: [null, [Validators.required, this.customValidators.checkPhoneNumber]],
      cellNumber2: [null, [this.customValidators.checkPhoneNumber]],
      cellNumber3: [null, [this.customValidators.checkPhoneNumber]],
      smsNumber: [null, [Validators.required]],
      fax: [null, [Validators.required, Validators.maxLength(50)]],
      website: [
        null,
        [Validators.maxLength(100), this.customValidators.checkEnglishAndNumberCharacters],
      ],
      email: [
        null,
        [Validators.required, Validators.maxLength(100), this.customValidators.checkEmail],
      ],
      telegram: [null, [Validators.maxLength(100)]],
      province: [null, [Validators.required]],
      city: [null, [Validators.required]],
      zipCode: [null, [Validators.required, Validators.maxLength(10), Validators.minLength(10)]],
      street: [null, [Validators.required, Validators.maxLength(100)]],
      fullAddress: [null, [Validators.required, Validators.maxLength(500)]],
      pelak: [null, [Validators.required, Validators.maxLength(10)]],
    });

    this.route.params.subscribe((p) => {
      if (p['id'] != '0' && p['id']) this.companyId = p['id'];
      this.getAddressInfo();
    });
  }

  ngOnInit(): void {
    this.getProvinces();
  }

  /**
   *دریافت لیست استان ها
   * */
  getProvinces() {
    this.dataService.get(ServerApis.getProvinces).subscribe(
      (response) => {
        this.provinceList = response.data ? response.data : [];
      },
      (error: any) => {},
    );
  }

  getAddressInfo() {
    this.loading = true;
    this.dataService
            .get(ServerApis.getCompanyAddressInfo, {
              companyId: this.companyId,
            })
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
                if (response.isSuccess && response.data) {
                  this.companyId = response.data.companyId;
                  this.addressForm.setValue({
                    mobileNumber: response.data.mobileNumber,
                    mobileNumber2: response.data.mobileNumber2,
                    mobileNumber3: response.data.mobileNumber3,
                    cellNumber: response.data.cellNumber,
                    cellNumber2: response.data.cellNumber2,
                    cellNumber3: response.data.cellNumber3,
                    smsNumber: response.data.smsNumber,
                    fax: response.data.fax,
                    website: response.data.website,
                    email: response.data.email,
                    telegram: response.data.telegram,
                    province: response.data.province ? response.data.province : '',
                    city: {
                      key: response.data.cityId,
                      text: response.data.city,
                    },
                    zipCode: response.data.zipCode,
                    street: response.data.street,
                    fullAddress: response.data.fullAddress,
                    pelak: response.data.pelak,
                  });
                } else {
                  var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
                  this.toastrService.error(msg);
                }
              }, (error: any) => {
              });
  }

  save() {
    if (this.addressForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.addressForm.markAllAsTouched();
      return;
    }
    var form = this.addressForm.value;
    this.isSaving = true;
    let dataToPost = {
      companyId: +this.companyId,
      mobileNumber: form.mobileNumber,
      mobileNumber2: form.mobileNumber2,
      mobileNumber3: form.mobileNumber3,
      cellNumber: form.cellNumber,
      cellNumber2: form.cellNumber2,
      cellNumber3: form.cellNumber3,
      smsNumber: form.smsNumber,
      fax: form.fax,
      website: form.website,
      email: form.email,
      telegram: form.telegram,
      province: form.province ? form.province : '',
      city: form.city.text,
      cityId: +form.city.key,
      zipCode: form.zipCode,
      street: form.street,
      fullAddress: form.fullAddress,
      pelak: form.pelak,
    };
    this.dataService.post(ServerApis.updateCompanyAddressInfo, dataToPost)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response.isSuccess) {
                this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
              } else {
                var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
                this.toastrService.error(msg);
              }
            }, (error: any) => {
            });
  }
}
