import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-add-sabtAhval-dialog',
  templateUrl: './add-sabtAhval.component.html',
  styleUrls: ['./add-sabtAhval.component.scss'],
  standalone: false,
})
export class AdminAddSabtAhvalDialogComponent extends AppBase implements OnInit {
  isSaving = false;
  frm: FormGroup;
  baseEnums: any = {};
  isUpdate = false;
  calcChargeTypeList: any = ([] = []);
  loadingData?: boolean;
  id: string = '';
  constructor(
    private matDialogRef: MatDialogRef<AdminAddSabtAhvalDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.frm = this.fb.group({
      fromDate: [null, [Validators.required]],
      exportType: [null, [Validators.required]],
      groupId: [null],
      toDate: [null, [Validators.required]],
    });
  }

  ngOnInit() {
    this.getGroups();
  }

  getGroups() {
    this.dataService.get(ServerApis.getGroups).subscribe(
      (response) => {
        this.baseEnums.citizenGroups = response.data;
      },
      (error: any) => {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  saveInfo() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return;
    }

    this.isSaving = true;

    var url = ServerApis.exportCitizenForSabtAhval;

    var params = this.frm.value;
    if (params.fromDate) params.fromDate = this.dataService.formatDate(params.fromDate);

    if (params.toDate) params.toDate = this.dataService.formatDate(params.toDate);

    this.dataService.post(url, this.frm.value).subscribe(
      (response) => {
        this.isSaving = false;
        if (response && response.isSuccess) {
          this.toastrService.success('با موفقیت ایجاد شد.');
          this.matDialogRef.close(true);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error: any) => {
        this.isSaving = false;
      },
    );
  }
}
