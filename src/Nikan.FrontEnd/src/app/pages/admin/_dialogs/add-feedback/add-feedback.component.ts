import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-add-feedback-dialog',
  templateUrl: './add-feedback.component.html',
  styleUrls: ['./add-feedback.component.scss'],
  standalone: false,
})
export class AdminAddFeedBackDialogComponent extends AppBase implements OnInit {
  userCode: string = '';

  isSaving=false;
  groupList: any[] = [];
  form: FormGroup;

  constructor(
    private matDialogRef: MatDialogRef<AdminAddFeedBackDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) {
    super();
    this.form = this.fb.group({
      feedbackDescription: [null, [Validators.required]],
      feedbackId: [null, [Validators.required]],
    });
    this.userCode = _data.userCode;
  }

  ngOnInit() {
    this.getGroups();
  }

  getGroups() {
    this.dataService.get(ServerApis.getBaseListFeedbacke).subscribe((response) => {
      if (response.isSuccess) this.groupList = response.data ? response.data : [];
      else {
        let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
        this.toastrService.error(msg);
      }
    });
  }

  saveInfo() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
        return ;
    }

    var formValue = this.form.value;
    this.isSaving = true;
    this.dataService
      .post(ServerApis.addFeedbacke, {
        userCode: this.userCode,
        feedbackId: formValue.feedbackId,
        feedbackDescription: formValue.feedbackDescription ? formValue.feedbackDescription : '',
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

  /**
   * for bind object in select
   * @param item
   */
  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.id == c2.id : c1 == c2;
  }
}
