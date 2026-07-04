import { Component, OnInit, Inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  MatAutocompleteSelectedEvent,
  MatAutocompleteTrigger,
} from '@angular/material/autocomplete';
import { BaseDataModel } from '@core/models/base-data-model';
import { Observable } from 'rxjs';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';

@Component({
  selector: 'app-adm-update-citizen-mobile-number-dialog',
  templateUrl: './update-citizen-mobile-number.component.html',
  styleUrls: ['./update-citizen-mobile-number.component.scss'],
})
export class CardUpdateCitizenMobileNumberDialogComponent implements OnInit {
  isSaving: boolean;
  userForm: FormGroup;
  userCode: string;
  loading: boolean = true;
  info: any;

  loadingData: boolean = true;
  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<CardUpdateCitizenMobileNumberDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private customValidator: CustomFormValidators,
    private dataService: DataService,
  ) {
    if (_data) {
      this.userCode = _data.userCode;
      this.getUserInfo();
    }

    this.userForm = this.fb.group({
      mobileNumber: [null, [Validators.required, this.customValidator.checkMobileNumber]],
    });
  }

  ngOnInit() {}

  getUserInfo() {
    this.loading = true;
    //todo
    this.dataService
      .get(ServerApis.getShortCitizenInfoByCard, { userCode: this.userCode })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess) {
            this.info = response.data;
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
            this.matDialogRef.close(false);
          }
        },
        (error) => {
          this.loading = false;
          this.matDialogRef.close(false);
        },
      );
  }

  displayFn(item): string {
    return item && item.text ? item.text : '';
  }

  saveInfo() {
    if (this.userForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.userForm.markAllAsTouched();
      return false;
    }

    var formValue = this.userForm.value;

    this.isSaving = true;
    this.dataService
      .post(ServerApis.updateCitizenMobileNumberByCard, {
        userCode: this.userCode,
        MobileNumber: formValue.mobileNumber,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response && response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
            this.matDialogRef.close(true);
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.isSaving = false;
        },
      );
  }
}
