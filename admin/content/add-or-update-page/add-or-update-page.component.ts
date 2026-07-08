import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { ENTER } from '@angular/cdk/keycodes';
import { ServerApis } from '@core/server-apis';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-add-or-update-page',
  templateUrl: './add-or-update-page.component.html',
  styleUrls: ['./add-or-update-page.component.scss'],
  standalone: false,
})
export class AdminAddOrUpdatePageComponent extends AppBase implements OnInit, AfterViewInit {
  isUpdate=false;
  id: string ='';
  form: FormGroup;

  readonly separatorKeysCodes: number[] = [ENTER];
  seoTags: any[] = [];

  isSaving=false;
    loading?: boolean;
  siteName: string;
  constructor(private customValidator: CustomFormValidators) {
    super();
    this.route.params.subscribe((p) => {
      if (p['id'] && p['id'] != '0') {
        this.isUpdate = true;
        this.id = p['id'];
        this.getWebPage();
      } else {
        this.id = '';
        this.isUpdate = false;
      }
    });

    this.form = this.fb.group({
      id: [null],
      title: [null, [Validators.required, Validators.maxLength(500)]],
      description: [null, [Validators.required, Validators.maxLength(2000)]],
      body: [null, []],
      seoDescription: [null, [Validators.maxLength(2000)]],
      seoTags: [null, []],
      slug: [null, [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.siteName = window.location.origin;
  }

  ngAfterViewInit() {
  
  }

  getWebPage() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getWebPage, {
        id: this.id,
        forEdit: true,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.form.setValue({
              id: response.data.id,
              title: response.data.title,
              description: response.data.description,
              body: response.data.body,
              seoDescription: response.data.seoDescription,
              seoTags: response.data.seoTags,
              slug: response.data.slug,
            });

            this.seoTags = response.data.seoTags ? response.data.seoTags.split(',') : [];
          
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error:any) => {
          this.loading = false;
        },
      );
  }

  add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;
    if ((value || '').trim()) {
      this.seoTags.push(value.trim());
    }
    if (input) {
      input.value = '';
    }
  }

  remove(item:any): void {
    const index = this.seoTags.indexOf(item);

    if (index >= 0) {
      this.seoTags.splice(index, 1);
    }
  }

  /**
   * for bind object in select
   * @param item
   */
  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.key === c2.key : c1 === c2;
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
      body: form.body,
      description: form.description,
      seoDescription: form.seoDescription,
      seoTags: this.seoTags.join(','),
      slug: form.slug,
    };
    this.isSaving = true;
    this.dataService.post(ServerApis.addOrUpdateWebPage, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.router.navigate(['/admin/pages']);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error:any) => {
        this.isSaving = false;
      },
    );
  }
}
