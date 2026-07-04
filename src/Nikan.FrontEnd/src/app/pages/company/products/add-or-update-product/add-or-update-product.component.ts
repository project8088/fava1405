import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '../../../../core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'company-add-or-update-product',
  templateUrl: './add-or-update-product.component.html',
  styleUrls: ['./add-or-update-product.component.scss'],
  standalone: false,
})
export class CompanyAddOrUpdateProductComponent extends AppBase implements OnInit, AfterViewInit {
  isUpdate=false;
  id: string;
  productForm: FormGroup;
  baseUrl = ServerApis.baseUrl;

  isSaving=false;
  imageUrl: string = '';
    loading?: boolean;

  parentProductList: any[] = [];

  productGroupList: any[] = [];
  loadingProductGroup: boolean;
  constructor() {
    super();
    this.route.params.subscribe((p) => {
      if (p['id'] && p['id'] != '0') {
        this.isUpdate = true;
        this.id = p['id'];
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
        parentId: this.productForm.get('productParentId')?.value,
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



  getAttachmentId(ev:{uploadUrl:string}) {
    this.imageUrl = ev.uploadUrl;
  }

  save() {
    if (this.productForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.productForm.markAllAsTouched();
        return ;
    }

    let form = this.productForm.value;
    form.imageUrl = this.imageUrl;
    let params = form;
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
