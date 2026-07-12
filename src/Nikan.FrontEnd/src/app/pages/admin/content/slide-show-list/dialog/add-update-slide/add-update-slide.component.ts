import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'adm-add-update-slide-dialog',
  templateUrl: './add-update-slide.component.html',
  styleUrls: ['./add-update-slide.component.scss'],
  standalone: false,
})
export class AdminAddOrUpdateSlideShowDialogComponent extends AppBase implements OnInit {
  isUpdate = false;
  isSaving = false;
  form: FormGroup;
  id?: number;
  loading = true;
  imageUrl: string = '';
  baseUrl = ServerApis.baseUrl;

  constructor(
    private matDialogRef: MatDialogRef<AdminAddOrUpdateSlideShowDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) {
    super();
    this.form = this.fb.group({
      id: [null],
      caption: ['', [Validators.required]],
      description: ['', []],
      url: ['', []],
      indexOrder: [0, [Validators.required]],
      isActive: [true, []],
    });
    if (_data.id) {
      this.isUpdate = true;
      this.id = _data.id;
      this.getInfo();
    } else {
      this.isUpdate = false;
    }
  }

  ngOnInit() {}

  getInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getSlideShow, { id: this.id })
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response.isSuccess) {
            this.form.patchValue(response.data);
            this.imageUrl = response.data.imageUrl;
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
  }

  getAttachmentId(ev: { uploadUrl: string }) {
    this.imageUrl = ev.uploadUrl;
  }

  saveInfo() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
      return;
    }

    var formValue = this.form.value;

    this.isSaving = true;
    var params = {
      id: formValue.id ? formValue.id : '',
      caption: formValue.caption,
      description: formValue.description,
      imageUrl: this.imageUrl,
      url: formValue.url,
      indexOrder: formValue.indexOrder,
      isActive: formValue.isActive,
    };

    this.dataService
      .post(ServerApis.addOrUpdateSlideShow, params)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
            this.matDialogRef.close(true);
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
  }
}
