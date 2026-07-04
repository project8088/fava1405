import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import * as CkEditor from '../../../../../assets/ckeditor';
import { ServerApis } from '../../../../core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-add-or-update-notification',
  templateUrl: './add-or-update-notification.component.html',
  styleUrls: ['./add-or-update-notification.component.scss'],
  standalone: false,
})
export class AdminAddOrUpdateNotificationComponent
  extends AppBase
  implements OnInit, AfterViewInit
{
  isUpdate: boolean;
  notificationId: string;
  notyForm: FormGroup;

  htmlEditor: any;
  isSaving: boolean;
  imageUrl: string = '';
  loading: boolean;
  baseUrl = ServerApis.baseUrl;

  constructor() {
    super();
    this.route.params.subscribe((p) => {
      if (p.id && p.id != '0') {
        this.isUpdate = true;
        this.notificationId = p.id;
        this.getNotificationInfo();
      } else {
        this.notificationId = '';
        this.isUpdate = false;
      }
    });

    this.notyForm = this.fb.group({
      id: [null],
      notificationNumber: [null, [Validators.required]],
      title: [null, [Validators.required, Validators.maxLength(500)]],
      description: [null, [Validators.required, Validators.maxLength(2000)]],
      body: [null, []],
      publishDate: [null, [Validators.required]],
      endDate: [null, [Validators.required]],
      isActive: [true, []],
      isPrivate: [false, []],
    });
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    setTimeout(() => {
      if (!this.isUpdate) this.loadCkEditor('');
    }, 500);
  }

  getNotificationInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getNotification, {
        id: this.notificationId,
        forEdit: true,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.notyForm.setValue({
              id: response.data.id,
              title: response.data.title,
              description: response.data.description,
              body: response.data.body,
              isActive: response.data.isActive,
              isPrivate: response.data.isPrivate,
              publishDate: response.data.publishDate ? new Date(response.data.publishDate) : null,
              endDate: response.data.endDate ? new Date(response.data.endDate) : null,
              notificationNumber: response.data.notificationNumber
                ? response.data.notificationNumber
                : '',
            });
            this.imageUrl = response.data.imageUrl;
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

  getAttachmentId(ev) {
    this.imageUrl = ev.uploadUrl;
  }

  save() {
    if (this.notyForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.notyForm.markAllAsTouched();
      return false;
    }
    let notificationBody = this.htmlEditor ? this.htmlEditor.getData() : '';
    if (!notificationBody) {
      this.toastrService.warning('محتوای خبر را وارد کنید.');
      return false;
    }
    let form = this.notyForm.value;
    let params: any = {
      id: this.notificationId ? +this.notificationId : '',
      title: form.title,
      body: notificationBody,
      description: form.description,
      isActive: form.isActive,
      isPrivate: form.isPrivate,
      publishDate: this.dataService.formatDate(form.publishDate),
      endDate: this.dataService.formatDate(form.endDate),
      imageUrl: this.imageUrl,
      notificationNumber: form.notificationNumber,
    };
    this.isSaving = true;
    this.dataService.post(ServerApis.addOrUpdateNotifications, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.router.navigate(['/admin/notifications']);
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
