import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '../../../../core/server-apis';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-company-change-status-dialog',
  templateUrl: './company-change-status.component.html',
  styleUrls: ['./company-change-status.component.scss'],
    standalone: false
})
export class AdminCompanyChangeStatusDialogComponent extends AppBase implements OnInit {
  companyInfo: any = {};
  isSaving: boolean;

  form: FormGroup;
  userCompanyAccountStatus: any[] = [];

  constructor(
    private matDialogRef: MatDialogRef<AdminCompanyChangeStatusDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any
  ) {
      super();
    this.form = this.fb.group({
      rejectDesription: [''],
      userCompanyAccountStatus: [null, [Validators.required]],
      sendSms: [false],
    });
    this.companyInfo = _data.company;
  }

  ngOnInit() {
    this.dataService.getEnums().subscribe((response) => {
      this.userCompanyAccountStatus = response.userCompanyAccountStatus;
    });
  }

  changeStatus() {
    if (this.form.get('userCompanyAccountStatus').value == 2) {
      this.form.get('rejectDesription').setValidators([Validators.required]);
      this.form.get('rejectDesription').updateValueAndValidity();
    } else {
      this.form.get('rejectDesription').setValue('');
      this.form.get('rejectDesription').clearValidators();
      this.form.get('rejectDesription').updateValueAndValidity();
    }
  }

  saveInfo() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
      return false;
    }

    var formValue = this.form.value;
    this.isSaving = true;
    this.dataService
      .post(ServerApis.changeCompanyAccount, {
        CompanyId: this.companyInfo.companyId,
        RejectDesription: formValue.rejectDesription ? formValue.rejectDesription : '',
        UserCompanyAccountStatus: formValue.userCompanyAccountStatus,
        SendSms: formValue.sendSms ? formValue.sendSms : false,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
            this.matDialogRef.close(true);
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
