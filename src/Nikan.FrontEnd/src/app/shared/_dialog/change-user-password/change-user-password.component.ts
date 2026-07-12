import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  MatAutocompleteSelectedEvent,
  MatAutocompleteTrigger,
} from '@angular/material/autocomplete';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-adm-change-user-password-dialog',
  templateUrl: './change-user-password.component.html',
  styleUrls: ['./change-user-password.component.scss'],
  standalone: false,
})
export class AdminChangePasswordDialogComponent extends AppBase implements OnInit {
  isSaving = false;
  changePasswordForm: FormGroup;
  userId?: string;

  loading: boolean = true;

  constructor(
    private matDialogRef: MatDialogRef<AdminChangePasswordDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.changePasswordForm = this.fb.group(
      {
        userId: [null],
        displayName: [null],
        username: [null],
        password: [null, [Validators.required]],
        confirmPassword: [null, [Validators.required]],
      },
      { validator: this.checkPasswords },
    );

    if (_data) {
      this.userId = _data.userId;
      this.changePasswordForm.setValue({
        userId: this.userId,
        username: _data.userName,
        displayName: _data.displayName,
        password: '',
        confirmPassword: '',
      });
    }
  }

  /**
   * بررسی یکی بودن کلمه عبور و تائید آن
   */
  checkPasswords(group: FormGroup) {
    let pass = group.controls['password']?.value;
    let confirmPassword = group.controls['confirmPassword']?.value;

    return pass === confirmPassword ? null : { notSame: true };
  }

  ngOnInit() {}

  saveInfo() {
    if (this.changePasswordForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.changePasswordForm.markAllAsTouched();
      return;
    }

    var formValue = this.changePasswordForm.value;

    this.isSaving = true;
    this.dataService
      .post(ServerApis.changeUserPassword, {
        UserId: this.userId,
        NewPassword: formValue.password,
        ConfirmPassword: formValue.confirmPassword,
      })
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
      });
  }

  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.key === c2.key : c1 === c2;
  }

  /**
   * حذف از اتوکاملیت چیپ
   * @param fruit
   */
  removeAutoChip(list: any[], item: any): void {
    const index = list.indexOf(item);

    if (index >= 0) {
      list.splice(index, 1);
    }
  }

  /**
   * انتخاب اتوکاملیت و اضافه کردن به لیست چیپ
   * @param list
   * @param formControl
   * @param input
   * @param event
   * @param Trigger
   */
  selectedAutoChip(
    list: any[],
    formControl: string,
    input: any,
    event: MatAutocompleteSelectedEvent,
    Trigger: MatAutocompleteTrigger,
  ): void {
    const index = list.findIndex((l) => l.value == event.option.value.value);
    if (index >= 0)
      this.toastrService.warning(event.option.value.text + ' را قبلاً انتخاب کرده اید.', 'تکراری!');
    else list.push(event.option.value);
    input.value = '';
    this.changePasswordForm.get(formControl)?.setValue(null);
    setTimeout(() => {
      Trigger.openPanel();
    }, 100);
  }
}
