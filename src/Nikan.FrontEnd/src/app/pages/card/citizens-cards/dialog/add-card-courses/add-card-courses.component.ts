import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'card-add-card-courses-dialog',
  templateUrl: './add-card-courses.component.html',
  styleUrls: ['./add-card-courses.component.scss'],
  standalone: false,
})
export class CardAddCardCoursesDialogComponent extends AppBase implements OnInit {
  isSaving = false;
  frm: FormGroup;
  centerList: any[] = [];
  loading: boolean = true;
  info: any;
  constructor(
    private matDialogRef: MatDialogRef<CardAddCardCoursesDialogComponent>,
    @Inject(MAT_DIALOG_DATA) _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.frm = this.fb.group({
      description: ['', [Validators.required]],
    });
  }

  ngOnInit() {}

  saveInfo() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    var url = ServerApis.addCardCourses;
    var params = this.frm.value;
    params.description = params.description;

    this.dataService.post(url, params)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response && response.isSuccess) {
                this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
                this.matDialogRef.close(true);
              } else {
                let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            }, (error: any) => {
              
            });
  }
}
