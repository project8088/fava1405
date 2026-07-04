import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '../../../../../core/custom-validator/form-validation';
import { DataService } from '../../../../../core/services/data-service.service';
import { ServerApis } from '../../../../../core/server-apis';

@Component({
  selector: 'card-add-card-courses-dialog',
  templateUrl: './add-card-courses.component.html',
  styleUrls: ['./add-card-courses.component.scss'],
})
export class CardAddCardCoursesDialogComponent implements OnInit {
  isSaving: boolean;
  frm: FormGroup;
  centerList: any[] = [];
  loading: boolean = true;
  info: any;
  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<CardAddCardCoursesDialogComponent>,
    @Inject(MAT_DIALOG_DATA) _data: any,
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private customValidator: CustomFormValidators,
    private dataService: DataService,
  ) {
    this.frm = this.fb.group({
      description: ['', [Validators.required]],
    });
  }

  ngOnInit() {}

  saveInfo() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return false;
    }

    this.isSaving = true;
    var url = ServerApis.addCardCourses;
    var params = this.frm.value;
    params.description = params.description;

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
