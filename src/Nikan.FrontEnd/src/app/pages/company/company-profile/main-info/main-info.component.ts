import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { ServerApis } from '../../../../core/server-apis';
import { DataService } from '../../../../core/services/data-service.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import * as CkEditor from '../../../../../assets/ckeditor';

@Component({
  selector: 'company-main-info',
  templateUrl: './main-info.component.html',
  styleUrls: ['./main-info.component.scss'],
})
export class CompanyMainInfoComponent implements OnInit, AfterViewInit {
  loading: boolean;
  companyId: string = '';
  mainForm: FormGroup;
  isSaving: boolean;

  htmlEditor: any;

  constructor(
    private fb: FormBuilder,
    private customValidators: CustomFormValidators,
    private dataService: DataService,
    private toastrService: ToastrService,
    private route: ActivatedRoute,
  ) {
    this.mainForm = this.fb.group({
      slagUrl: [
        null,
        [
          Validators.required,
          Validators.maxLength(40),
          this.customValidators.checkEnglishWithoutSpace,
        ],
      ],
      content: [null, []],
      insuranceNumber: [null, [Validators.required]],
      companyRepresentative: [null, [Validators.required, Validators.maxLength(100)]],
      numberOfEmployees: [null, [Validators.required]],
    });

    this.route.params.subscribe((p) => {
      if (p.id != '0' && p.id) this.companyId = p.id;
      this.getAddressInfo();
    });
  }

  ngAfterViewInit(): void {
    this.loadCkEditor(this.mainForm.get('content').value);
  }

  ngOnInit(): void {}

  getAddressInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getCompanyMainInfo, {
        companyId: this.companyId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.companyId = response.data.companyId;

            this.mainForm.setValue({
              slagUrl: response.data.slagUrl,
              content: response.data.content,
              insuranceNumber: response.data.insuranceNumber,
              companyRepresentative: response.data.companyRepresentative,
              numberOfEmployees: response.data.numberOfEmployees
                ? response.data.numberOfEmployees
                : 0,
            });
            setTimeout(() => {
              this.loadCkEditor(this.mainForm.get('content').value);
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

  save() {
    if (this.mainForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.mainForm.markAllAsTouched();
      return false;
    }
    var form = this.mainForm.value;
    this.isSaving = true;

    let dataToPost = {
      companyId: +this.companyId,
      slagUrl: form.slagUrl,
      content: this.htmlEditor ? this.htmlEditor.getData() : '',
      insuranceNumber: form.insuranceNumber,
      companyRepresentative: form.companyRepresentative,
      numberOfEmployees: form.numberOfEmployees,
    };
    this.dataService.post(ServerApis.updateCompanyMainInfo, dataToPost).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
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
          //  if (!isFocused) {

          //  }
          //});
        })
        .catch((error) => {
          //console.warn('Build id: nwwk5h15tym5-uff91zgwvva9');
          console.error(error);
        });
    }
  }
}
