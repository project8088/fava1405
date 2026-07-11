import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ENTER } from '@angular/cdk/keycodes';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-update-manzalat-base-form',
  templateUrl: './update-manzalat-base-form.component.html',
  styleUrls: ['./update-manzalat-base-form.component.scss'],
  standalone: false,
})
export class AdminUpdateManzalatBaseFormComponent extends AppBase implements OnInit, AfterViewInit {
  isUpdate = false;
  id: string = '';
  form: FormGroup;

  readonly separatorKeysCodes: number[] = [ENTER];
  seoTags: any[] = [];

  isSaving = false;
  loading?: boolean;
  siteName: string = '';
  constructor(private customValidator: CustomFormValidators) {
    super();
    this.route.params.subscribe((p) => {
      this.id = p['id'];
      this.getItem();
    });

    this.form = this.fb.group({
      id: [null],
      title: [null, [Validators.required, Validators.maxLength(500)]],
      minAge: [null, [Validators.min(0), Validators.max(99), Validators.pattern(/^[0-9]/)]],
      maxAge: [null, [Validators.min(0), Validators.max(99), Validators.pattern(/^[0-9]/)]],

      isActive: [false, []],
      gender: [null, []],
      description: ['', []],
      uploadDescription: ['', []],
      orderIndex: [null, []],
    });
  }

  ngOnInit(): void {}

  ngAfterViewInit() {}

  getItem() {
    this.loading = true;
    this.dataService.get(ServerApis.getManzalatBaseForm, { id: this.id }).subscribe(
      (response) => {
        this.loading = false;
        if (response.isSuccess && response.data) {
          this.form.setValue({
            id: response.data.id,
            title: response.data.title,
            description: response.data.description,
            minAge: response.data.minAge,
            maxAge: response.data.maxAge,
            isActive: response.data.isActive,
            uploadDescription: response.data.uploadDescription,
            orderIndex: response.data.orderIndex,
            gender: response.data.gender,
          });
        } else {
          var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      },
      (error: any) => {
        this.loading = false;
      },
    );
  }

  save() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
      return;
    }
    let form = this.form.value;
    let params: any = {
      id: this.id ? +this.id : '',
      title: form.title,
      description: form.body,
      minAge: form.minAge,
      maxAge: form.maxAge,
      isActive: form.isActive,
      gender: form.gender,
      orderIndex: form.orderIndex,
      uploadDescription: form.uploadDescription,
    };
    this.isSaving = true;
    this.dataService.post(ServerApis.updateManzalatBaseForm, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.router.navigate(['/admin/manzalat-form-list']);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error: any) => {
        this.isSaving = false;
      },
    );
  }
}
