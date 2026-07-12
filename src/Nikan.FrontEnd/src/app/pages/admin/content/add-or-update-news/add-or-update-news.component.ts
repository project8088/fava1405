import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { ENTER } from '@angular/cdk/keycodes';
import { ServerApis } from '@core/server-apis';
import { NewsDto } from '@core/models/news';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'adm-add-or-update-news',
  templateUrl: './add-or-update-news.component.html',
  styleUrls: ['./add-or-update-news.component.scss'],
  standalone: false,
})
export class AdminAddOrUpdateNewsComponent extends AppBase implements OnInit, AfterViewInit {
  isUpdate = false;
  newsId: string = '';
  newsForm: FormGroup;
  baseUrl = ServerApis.baseUrl;

  readonly separatorKeysCodes: number[] = [ENTER];
  seoTags: any[] = [];

  isSaving = false;
  imageUrl: string = '';
  loading?: boolean;

  groupList: any[] = [];
  constructor() {
    super();
    this.route.params.subscribe((p) => {
      if (p['id'] && p['id'] != '0') {
        this.isUpdate = true;
        this.newsId = p['id'];
        this.getNewsInfo();
      } else {
        this.newsId = '';
        this.isUpdate = false;
      }
    });

    this.newsForm = this.fb.group({
      id: [null],
      title: [null, [Validators.required, Validators.maxLength(500)]],
      description: [null, [Validators.required, Validators.maxLength(2000)]],
      body: [null, []],
      seoDescription: [null, [Validators.maxLength(2000)]],
      seoTags: [null, []],
      publishDate: [null, [Validators.required]],
      commentIsActive: [true, []],
      newsGroupId: [null, []],
      isSpecial: [false, []],
      indexOrder: [0],
      isActive: [true, []],
    });
  }

  ngOnInit(): void {
    this.dataService.get(ServerApis.getListNewsGroups, {}).subscribe((response) => {
      if (response.isSuccess) this.groupList = response.data ? response.data : [];
    });
  }

  ngAfterViewInit() {}

  getNewsInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getNews, {
        id: this.newsId,
        forEdit: true,
      })
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess && response.data) {
          this.newsForm.setValue({
            id: response.data.id,
            title: response.data.title,
            description: response.data.description,
            body: response.data.body,
            indexOrder: response.data.indexOrder,
            seoDescription: response.data.seoDescription,
            seoTags: response.data.seoTags,
            commentIsActive: response.data.commentIsActive,
            newsGroupId: response.data.newsGroupId,
            isSpecial: response.data.isSpecial,
            isActive: response.data.isActive,
            publishDate: response.data.publishDate ? new Date(response.data.publishDate) : null,
          });
          this.imageUrl = response.data.imageUrl;

          this.seoTags = response.data.seoTags ? response.data.seoTags.split(',') : [];
        } else {
          var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      });
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

  remove(item: any): void {
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

  getAttachmentId(ev: { uploadUrl: string }) {
    this.imageUrl = ev.uploadUrl;
  }

  save() {
    if (this.newsForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.newsForm.markAllAsTouched();
      return;
    }
    let form = this.newsForm.value;
    let params: NewsDto = {
      id: this.newsId ? +this.newsId : '',
      title: form.title,
      body: form.body,
      commentIsActive: form.commentIsActive,
      description: form.description,
      isSpecial: form.isSpecial,
      indexOrder: form.indexOrder,
      isActive: form.isActive,
      seoDescription: form.seoDescription,
      seoTags: this.seoTags.join(','),
      newsGroupId: form.newsGroupId,
      publishDate: this.dataService.formatDate(form.publishDate),
      imageUrl: this.imageUrl,
    };
    this.isSaving = true;
    this.dataService
      .post(ServerApis.addOrUpdateNews, params)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.router.navigate(['/admin/news-list']);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      });
  }
}
