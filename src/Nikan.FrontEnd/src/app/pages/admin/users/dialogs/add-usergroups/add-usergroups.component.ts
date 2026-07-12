import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { userGroupsDto } from '@core/models/users/userGroups';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-adm-add-usergroups-dialog',
  templateUrl: './add-usergroups.component.html',
  styleUrls: ['./add-usergroups.component.scss'],
  standalone: false,
})
export class AdminAddUserGrousDialogComponent extends AppBase implements OnInit {
  isSaving = false;
  isUpdate = false;
  userGroupsForm: FormGroup;
  id: string = '';
  loading: boolean = true;
  userGroups?: userGroupsDto;

  constructor(
    private matDialogRef: MatDialogRef<AdminAddUserGrousDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    if (_data) {
      this.userGroups = _data.userGroups ? _data.userGroups : '';
      this.isUpdate = true;
    }

    this.userGroupsForm = this.fb.group({
      id: [null],
      name: [null, [Validators.required]],
    });
  }

  ngOnInit() {
    if (this.userGroups) {
      console.log(this.userGroups.name);
      this.userGroupsForm.setValue({
        name: this.userGroups.name,
        id: this.userGroups.id,
      });
    }
  }

  saveInfo() {
    if (this.userGroupsForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.userGroupsForm.markAllAsTouched();
      return;
    }

    var formValue = this.userGroupsForm.value;

    this.isSaving = true;
    this.dataService
      .post(ServerApis.addOrUpdateUserGroups, formValue)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response && response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
            this.matDialogRef.close(response.data);
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
  }

  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.key === c2.key : c1 === c2;
  }
}
