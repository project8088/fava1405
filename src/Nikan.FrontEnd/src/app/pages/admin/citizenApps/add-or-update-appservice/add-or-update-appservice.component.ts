import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';
import * as CkEditor from '../../../../../assets/ckeditor';

@Component({
  selector: 'app-add-or-update-appservice',
  templateUrl: './add-or-update-appservice.component.html',
  styleUrls: ['./add-or-update-appservice.component.scss'],
})
export class AdminAddOrUpdateAppserviceComponent implements OnInit, AfterViewInit {
  isUpdate: boolean;
  serviceId: string;
  storeForm: FormGroup;
  baseUrl = ServerApis.baseUrl;

  isSaving: boolean;
  imageUrl: string = '';
  loading: boolean;

  htmlEditor: any;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private toastrService: ToastrService,
    private dataService: DataService,
    private router: Router,
  ) {
    this.route.params.subscribe((p) => {
      if (p.id && p.id != '0') {
        this.isUpdate = true;
        this.serviceId = p.id;
        this.getStoreInfo();
      } else {
        this.serviceId = '';
        this.isUpdate = false;
      }
    });

    this.storeForm = this.fb.group({
      serviceId: [null],
      serviceName: [null, [Validators.required, Validators.maxLength(500)]],
      description: [null, [Validators.required, Validators.maxLength(2000)]],
      terms: [null],
      cssClass: [null],
      icon: [null],
      paramName1: [null],
      paramName2: [null],
      paramValue1: [null],
      paramValue2: [null],
      link: [null, [Validators.required]],

      priority: [0],
      isLinkService: [true, []],
      isNeedAuthenticate: [true, []],
      openInNewWindow: [true, []],
      isMain: [true, []],
      haveTerms: [true, []],
      isActive: [true, []],
      isShowInDashbordCitizen: [false, []],
    });
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    if (!this.isUpdate) this.loadCkEditor('');
  }

  getStoreInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getAppInfo, {
        id: this.serviceId,
        forEdit: true,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.storeForm.patchValue(response.data);
            this.imageUrl = response.data.imageUrl;
            setTimeout(() => {
              this.loadCkEditor(response.data.terms);
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
    return c1 && c2 ? +c1.key === c2.key : c1 === c2;
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
    if (this.storeForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.storeForm.markAllAsTouched();
      return false;
    }

    let form = this.storeForm.value;
    form.imageUrl = this.imageUrl;
    let params = form;
    if (this.htmlEditor.getData()) params.terms = this.htmlEditor.getData();
    this.isSaving = true;
    this.dataService.post(ServerApis.addOrUpdateAppService, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.router.navigate(['/admin/appService-list']);
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
