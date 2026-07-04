import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { ENTER } from '@angular/cdk/keycodes';
import * as CkEditor from '../../../../../assets/ckeditor';
import { ServerApis } from '../../../../core/server-apis';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-add-or-update-page',
  templateUrl: './add-or-update-page.component.html',
  styleUrls: ['./add-or-update-page.component.scss'],
    standalone: false
})
export class AdminAddOrUpdatePageComponent extends AppBase implements OnInit, AfterViewInit {
  isUpdate: boolean;
  id: string;
  form: FormGroup;

  readonly separatorKeysCodes: number[] = [ENTER];
  seoTags: any[] = [];

  htmlEditor: any;
  isSaving: boolean;
  loading: boolean;
  siteName: string;
  constructor(
    private customValidator: CustomFormValidators,
  ) {
      super();
    this.route.params.subscribe((p) => {
      if (p.id && p.id != '0') {
        this.isUpdate = true;
        this.id = p.id;
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
    setTimeout(() => {
      if (!this.isUpdate) this.loadCkEditor('');
    }, 500);
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
            setTimeout(() => {
              this.loadCkEditor(response.data.body);
            }, 1000);
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

  remove(item): void {
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

  /**
   * لود کردن html editor
   * */
  loadCkEditor(content) {
    if (!this.htmlEditor && document.querySelector('.html-editor')) {
      document.querySelector('.html-editor').innerHTML = '';
      CkEditor.create(document.querySelector('.html-editor'), {
        removePlugins: ['Title'],
        toolbar: {
          items: [
            'heading',
            '|',
            'bold',
            'italic',
            'underline',
            'link',
            'bulletedList',
            'numberedList',
            '|',
            'indent',
            'alignment',
            'outdent',
            'pageBreak',
            '|',
            'fontBackgroundColor',
            'fontColor',
            'fontFamily',
            'fontSize',
            'highlight',
            'removeFormat',
            '|',
            'imageUpload',
            'blockQuote',
            'insertTable',
            'mediaEmbed',
            'code',
            'codeBlock',
            'exportPdf',
            'horizontalLine',
            'specialCharacters',
            'todoList',
            '|',
            'undo',
            'redo',
          ],
        },
        language: 'fa',
        image: {
          // Configure the available styles.
          styles: ['alignLeft', 'alignCenter', 'alignRight', 'full', 'side'],
          // You need to configure the image toolbar, too, so it shows the new style
          // buttons as well as the resize buttons.
          toolbar: [
            'imageStyle:alignLeft',
            'imageStyle:alignCenter',
            'imageStyle:alignRight',
            '|',
            'imageTextAlternative',
            'imageStyle:full',
            'imageStyle:side',
          ],
        },
        table: {
          contentToolbar: [
            'tableColumn',
            'tableRow',
            'mergeTableCells',
            'tableCellProperties',
            'tableProperties',
          ],
        },
        licenseKey: '',
        title: {
          placeholder: 'عنوان را در این قسمت تایپ کنید',
        },
        placeholder: 'محتوای خود را در این قسمت بنویسید و یا Paste کنید.',
      })
        .then((editor) => {
          //window.editor = editor;
          this.htmlEditor = editor;
          if (content) {
            this.htmlEditor.setData(content);
          }
          //this.htmlEditor.model.document.on('change', () => {
          //});
          //on blure
          //editor.ui.focusTracker.on('change:isFocused', (evt, name, isFocused) => {
          // // if (!isFocused)

          //});
        })
        .catch((error) => {
          //console.warn('Build id: nwwk5h15tym5-uff91zgwvva9');
          console.error(error);
        });
    }
  }

  save() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
      return false;
    }
    let newsBody = this.htmlEditor ? this.htmlEditor.getData() : '';
    if (!newsBody) {
      this.toastrService.warning('محتوای خبر را وارد کنید.');
      return false;
    }
    let form = this.form.value;
    let params: any = {
      id: this.id ? +this.id : '',
      title: form.title,
      body: newsBody,
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
      (error) => {
        this.isSaving = false;
      },
    );
  }
}
