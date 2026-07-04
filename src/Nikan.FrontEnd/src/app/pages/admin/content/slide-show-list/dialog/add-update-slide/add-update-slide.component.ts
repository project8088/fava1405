import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DataService } from '@core/services/data-service.service';
import { ServerApis } from '@core/server-apis';

@Component({
  selector: 'adm-add-update-slide-dialog',
  templateUrl: './add-update-slide.component.html',
  styleUrls: ['./add-update-slide.component.scss'],
})
export class AdminAddOrUpdateSlideShowDialogComponent implements OnInit {
  isUpdate: boolean;
  isSaving: boolean;
  form: FormGroup;
  id: number;
  loading = true;
  imageUrl: string = '';
  baseUrl = ServerApis.baseUrl;

  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<AdminAddOrUpdateSlideShowDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private dataService: DataService,
  ) {
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
    this.dataService.get(ServerApis.getSlideShow, { id: this.id }).subscribe(
      (response) => {
        this.loading = false;
        if (response.isSuccess) {
          this.form.patchValue(response.data);
          this.imageUrl = response.data.imageUrl;
        } else {
          var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loading = false;
      },
    );
  }

  getAttachmentId(ev) {
    this.imageUrl = ev.uploadUrl;
  }

  saveInfo() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
      return false;
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

    this.dataService.post(ServerApis.addOrUpdateSlideShow, params).subscribe(
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
