import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'company-additional-info',
  templateUrl: './additional-info.component.html',
  styleUrls: ['./additional-info.component.scss'],
  standalone: false,
})
export class CompanyAdditionalInfoComponent extends AppBase implements OnInit, AfterViewInit {
  loading?: boolean;
  companyId: string = '';
  form: FormGroup;
  isSaving = false;

  constructor(private customValidators: CustomFormValidators) {
    super();
    this.form = this.fb.group({
      companyId: [null],
      contractCode: [null],
      contractOnDate: [null],
      waterContractOnDate: [null],
      fileCode: [null],
      waterDepositId: [null],
      waterCode: [null],
      chargeDepositId: [null],
      chargeCode: [null],
      chargeMoeinCode: [null],
      volumeAirTanks: [0],
      isBusinessUnit: [false],
      isBuildingCompany: [false],
      issueWaterBill: [false],
      issueChargeBill: [false],
    });

    this.route.params.subscribe((p) => {
      if (p['id'] != '0' && p['id']) this.companyId = p['id'];
      this.getAdditionalInfo();
    });
  }

  ngAfterViewInit(): void {}

  ngOnInit(): void {}

  getAdditionalInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getAdditionalInfo, {
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

            this.form.patchValue(response.data);
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
    if (form.contractOnDate) form.contractOnDate = this.dataService.formatDate(form.contractOnDate);
    if (form.waterContractOnDate)
      form.waterContractOnDate = this.dataService.formatDate(form.waterContractOnDate);

    if (this.companyId) form.CompanyId = +this.companyId;

    this.isSaving = true;

    let dataToPost = form;
    this.dataService
      .post(ServerApis.updateAdditionalInfo, dataToPost)
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
