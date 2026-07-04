import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '../../../../core/server-apis';
import * as CkEditor from '../../../../../assets/ckeditor';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'company-add-or-update-product',
  templateUrl: './add-or-update-product.component.html',
  styleUrls: ['./add-or-update-product.component.scss'],
    standalone: false
})
export class CompanyAddOrUpdateProductComponent extends AppBase implements OnInit, AfterViewInit {
  isUpdate: boolean;
  id: string;
  productForm: FormGroup;
  baseUrl = ServerApis.baseUrl;

  isSaving: boolean;
  imageUrl: string = '';
  loading: boolean;

  parentProductList: any[] = [];

  productGroupList: any[] = [];
  htmlEditor: any;
  loadingProductGroup: boolean;
  constructor(
) {
      super();
    this.route.params.subscribe((p) => {
      if (p.id && p.id != '0') {
        this.isUpdate = true;
        this.id = p.id;
        this.getInfo();
      } else {
        this.id = '';
        this.isUpdate = false;
      }
    });

    this.productForm = this.fb.group({
      id: [null],
      title: [null, [Validators.required, Validators.maxLength(500)]],
      description: [null, [Validators.required, Validators.maxLength(2000)]],
      content: [null],
      code: [null, [Validators.required]],
      productParentId: [null, [Validators.required]],
      productGroupId: [null, [Validators.required]],
      price: [null, [Validators.required]],
      isActive: [true, []],
    });
  }

  ngOnInit(): void {
    this.dataService.get(ServerApis.getProductParentGroups).subscribe((response) => {
      if (response.isSuccess) this.parentProductList = response.data;
      else {
        var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
        this.toastrService.error(msg);
      }
    });
  }

  getProductByParent() {
    this.loadingProductGroup = true;
    this.dataService
      .get(ServerApis.getProductGroupsByParentId, {
        parentId: this.productForm.get('productParentId').value,
      })
      .subscribe(
        (response) => {
          this.loadingProductGroup = false;
          if (response.isSuccess) {
            this.productGroupList = response.data ? response.data : [];
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loadingProductGroup = false;
        },
      );
  }

  ngAfterViewInit() {
    if (!this.isUpdate) this.loadCkEditor('');
  }

  getInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getCompanyProduct, {
        id: this.id,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            response.data.productGroupId = response.data.productGroupId.toString();
            this.productForm.patchValue(response.data);

            if (response.data.productParentId) this.getProductByParent();

            this.imageUrl = response.data.imageUrl;
            setTimeout(() => {
              this.loadCkEditor(response.data.content);
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
    if (this.productForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.productForm.markAllAsTouched();
      return false;
    }

    let form = this.productForm.value;
    form.imageUrl = this.imageUrl;
    let params = form;
    if (this.htmlEditor.getData()) params.content = this.htmlEditor.getData();
    this.isSaving = true;
    this.dataService.post(ServerApis.addOrUpdateCompnayProduct, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.router.navigate(['/company/products']);
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
