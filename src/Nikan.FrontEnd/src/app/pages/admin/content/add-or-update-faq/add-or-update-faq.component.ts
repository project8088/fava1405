import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import * as CkEditor from '../../../../../assets/ckeditor';
import { ServerApis } from '../../../../core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-add-or-update-faq',
  templateUrl: './add-or-update-faq.component.html',
  styleUrls: ['./add-or-update-faq.component.scss'],
  standalone: false,
})
export class AdminAddOrUpdateFaqComponent extends AppBase implements OnInit, AfterViewInit {
  isUpdate: boolean;
  faqId: string;
  faqForm: FormGroup;

  htmlEditor: any;
  isSaving: boolean;
  attachmentId: string = '';
  loading: boolean;

  groupList: any[] = [];

  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

  tagNames: any[] = [];

  constructor() {
    super();
    this.faqForm = this.fb.group({
      id: [null],
      title: [null, [Validators.required, Validators.maxLength(500)]],
      description: [null],
      questionGroupTypeId: [null, [Validators.required]],
      tagNames: [null, []],
      isActive: [true, []],
    });
  }

  ngAfterViewInit() {
    if (!this.isUpdate) this.loadCkEditor('');
  }
  ngOnInit(): void {
    this.getGroups();
    this.route.params.subscribe((p) => {
      if (p.id && p.id != '0') {
        this.isUpdate = true;
        this.faqId = p.id;
        this.getFaqInfo();
      } else {
        this.faqId = '';
        this.isUpdate = false;
      }
    });
  }

  getGroups() {
    this.dataService.get(ServerApis.getFaqGroups).subscribe((response) => {
      if (response.isSuccess) this.groupList = response.data ? response.data : [];
      else {
        let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
        this.toastrService.error(msg);
      }
    });
  }

  getFaqInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getFaq, {
        id: this.faqId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.faqForm.setValue({
              id: response.data.id,
              title: response.data.title,
              description: response.data.description,
              tagNames: response.data.tagNames,
              questionGroupTypeId: +response.data.questionGroupTypeId,
              isActive: response.data.isActive,
            });
            this.tagNames = response.data.tagNames ? response.data.tagNames.split(',') : [];
            setTimeout(() => {
              this.loadCkEditor(response.data.description);
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
      this.tagNames.push(value.trim());
    }
    if (input) {
      input.value = '';
    }
  }

  remove(item): void {
    const index = this.tagNames.indexOf(item);

    if (index >= 0) {
      this.tagNames.splice(index, 1);
    }
  }

  /**
   * for bind object in select
   * @param item
   */
  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.id == c2.id : c1 == c2;
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

  getAttachmentId(ev) {
    this.attachmentId = ev;
  }

  save() {
    if (this.faqForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.faqForm.markAllAsTouched();
      return false;
    }

    let descriptionBody = this.htmlEditor ? this.htmlEditor.getData() : '';
    if (!descriptionBody) {
      this.toastrService.warning('محتوای پاسخ به سوال را وارد کنید.');
      return false;
    }
    let form = this.faqForm.value;
    let data = {
      id: this.faqId ? this.faqId : '',
      title: form.title,
      description: descriptionBody,
      tagNames: this.tagNames.join(','),
      questionGroupTypeId: form.questionGroupTypeId,
      isActive: form.isActive,
    };
    this.isSaving = true;
    this.dataService.post(ServerApis.addOrUpdateFaq, data).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.router.navigate(['/admin/faq-list']);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.isSaving = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است!');
      },
    );
  }
}
