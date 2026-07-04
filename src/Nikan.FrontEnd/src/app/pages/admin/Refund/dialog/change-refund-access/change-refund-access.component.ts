import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '../../../../../core/custom-validator/form-validation';
import { DataService } from '../../../../../core/services/data-service.service';
import { ServerApis } from '../../../../../core/server-apis';

@Component({
  selector: 'adm-change-refund-access-dialog',
  templateUrl: './change-refund-access.component.html',
  styleUrls: ['./change-refund-access.component.scss'],
})
export class AdminChangeRefundAccessDialogComponent implements OnInit {
  isSaving: boolean;
  frm: FormGroup;
  loading: boolean = true;
  info: any;
  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<AdminChangeRefundAccessDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private customValidator: CustomFormValidators,
    private dataService: DataService,
  ) {
    this.info = _data.info;

    this.frm = this.fb.group({
      unitName: [null, [Validators.required]],
      className: ['', [Validators.required]],
      nationalCode: [null, this.customValidator.checkNationalCode],
      letterNumber: [''],
      description: [null],
      citizenAccess: [true],
      isClosed: [false],
    });
    this.frm.patchValue(_data.info);
  }

  ngOnInit() {}

  saveInfo() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return false;
    }

    this.isSaving = true;
    var url = ServerApis.updateRefundAccess;
    var params = this.frm.value;
    params.id = this.info.id;
    params.unitName = params.unitName;
    params.className = params.className;
    params.nationalCode = params.nationalCode;
    params.description = params.description;
    params.citizenAccess = +params.citizenAccess;
    params.isClosed = +params.isClosed;
    this.dataService.post(url, params).subscribe(
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
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }
}
