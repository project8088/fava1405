import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'company-main-info',
  templateUrl: './main-info.component.html',
  styleUrls: ['./main-info.component.scss'],
  standalone: false,
})
export class CompanyMainInfoComponent extends AppBase implements OnInit, AfterViewInit {
  loading?: boolean;
  companyId: string = '';
  mainForm: FormGroup;
  isSaving = false;

  constructor(private customValidators: CustomFormValidators) {
    super();
    this.mainForm = this.fb.group({
      slagUrl: [
        null,
        [
          Validators.required,
          Validators.maxLength(40),
          this.customValidators.checkEnglishWithoutSpace,
        ],
      ],
      content: [null, []],
      insuranceNumber: [null, [Validators.required]],
      companyRepresentative: [null, [Validators.required, Validators.maxLength(100)]],
      numberOfEmployees: [null, [Validators.required]],
    });

    this.route.params.subscribe((p) => {
      if (p['id'] != '0' && p['id']) this.companyId = p['id'];
      this.getAddressInfo();
    });
  }

  ngAfterViewInit(): void {}

  ngOnInit(): void {}

  getAddressInfo() {
    this.loading = true;
    this.dataService
            .get(ServerApis.getCompanyMainInfo, {
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

                  this.mainForm.setValue({
                    slagUrl: response.data.slagUrl,
                    content: response.data.content,
                    insuranceNumber: response.data.insuranceNumber,
                    companyRepresentative: response.data.companyRepresentative,
                    numberOfEmployees: response.data.numberOfEmployees
                      ? response.data.numberOfEmployees
                      : 0,
                  });
                } else {
                  var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
                  this.toastrService.error(msg);
                }
              }, (error: any) => {
              });
  }

  save() {
    if (this.mainForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.mainForm.markAllAsTouched();
      return;
    }
    var form = this.mainForm.value;
    this.isSaving = true;

    let dataToPost = {
      companyId: +this.companyId,
      slagUrl: form.slagUrl,
      content: form.content,
      insuranceNumber: form.insuranceNumber,
      companyRepresentative: form.companyRepresentative,
      numberOfEmployees: form.numberOfEmployees,
    };
    this.dataService.post(ServerApis.updateCompanyMainInfo, dataToPost)
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
