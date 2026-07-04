import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { ServerApis } from '../../../../core/server-apis';
import { DataService } from '../../../../core/services/data-service.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import * as CkEditor from '../../../../../assets/ckeditor';

@Component({
  selector: 'company-additional-info',
  templateUrl: './additional-info.component.html',
  styleUrls: ['./additional-info.component.scss'],
})
export class CompanyAdditionalInfoComponent implements OnInit, AfterViewInit {
  loading: boolean;
  companyId: string = '';
  form: FormGroup;
  isSaving: boolean;

  constructor(
    private fb: FormBuilder,
    private customValidators: CustomFormValidators,
    private dataService: DataService,
    private toastrService: ToastrService,
    private route: ActivatedRoute,
  ) {
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
      if (p.id != '0' && p.id) this.companyId = p.id;
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
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.companyId = response.data.companyId;

            this.form.patchValue(response.data);
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loading = false;
        },
      );
  }

  save() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
      return false;
    }
    var form = this.form.value;
    if (form.contractOnDate) form.contractOnDate = this.dataService.formatDate(form.contractOnDate);
    if (form.waterContractOnDate)
      form.waterContractOnDate = this.dataService.formatDate(form.waterContractOnDate);

    if (this.companyId) form.CompanyId = +this.companyId;

    this.isSaving = true;

    let dataToPost = form;
    this.dataService.post(ServerApis.updateAdditionalInfo, dataToPost).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
        } else {
          var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.isSaving = false;
      },
    );
  }
}
