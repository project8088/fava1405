import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '../../../../../core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'company-add-update-product-group-dialog',
  templateUrl: './add-update-product-group.component.html',
  styleUrls: ['./add-update-product-group.component.scss'],
  standalone: false,
})
export class CompanyAddUpdateProductGroupDialogComponent extends AppBase implements OnInit {
  isSaving: boolean;
  isUpdate: boolean;
  form: FormGroup;
  parentProductList: any[] = [];

  constructor(
    private matDialogRef: MatDialogRef<CompanyAddUpdateProductGroupDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) {
    super();
    this.form = this.fb.group({
      id: [null, []],
      name: [null, [Validators.required]],
      parentId: [null, [Validators.required]],
      isActive: [true],
    });
    if (_data.item) {
      this.isUpdate = true;
      if (_data.item.parentId) _data.item.parentId = _data.item.parentId.toString();
      this.form.patchValue(_data.item);
    } else {
      this.isUpdate = false;
    }
  }

  ngOnInit() {
    this.dataService.get(ServerApis.getProductParentGroups).subscribe((response) => {
      if (response.isSuccess) this.parentProductList = response.data;
      else {
        var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
        this.toastrService.error(msg);
      }
    });
  }

  saveInfo() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
      return false;
    }

    var formValue = this.form.value;
    this.isSaving = true;
    this.dataService.post(ServerApis.addOrUpdateProductGroup, formValue).subscribe(
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
