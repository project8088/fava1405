import { Component, OnInit } from '@angular/core';
import { AuthUser } from '@core/authentication/user.model';
import { ServerApis } from '@core/server-apis';
import { CustomFormValidators } from '@core/custom-validator/form-validation';

import { Title, Meta } from '@angular/platform-browser';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'home-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
  standalone: false,
})
export class MainProductDetailsComponent extends AppBase implements OnInit {
  id: string ='';
  user: AuthUser | null;
  loadingData?: boolean;
  product: any;

  baseUrl: string = ServerApis.baseUrl;
  waitForRedirectToBank: boolean = false;
  RefId: string = '';
  loadingPay: boolean= false;

  constructor(
    private customValidator: CustomFormValidators,
    private titleService: Title,
    private metaService: Meta,
  ) {
    super();
    this.route.params.subscribe((p) => {
      this.id = p['id'];
      this.getDetailsInfo();
    });
    this.user = this.authService.currentUserValue;
  }

  ngOnInit() {}

  getDetailsInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getCompanyProduct, { id: this.id }).subscribe(
      (response) => {
        this.loadingData = false;
        if (response.isSuccess) {
          this.product = response.data;

          if (this.product.seoTags) {
            this.titleService.setTitle(this.product.seoTags.seoTitle);
            this.metaService.addTags([
              { name: 'keywords', content: this.product.seoTags.seokeywords },
              { name: 'description', content: this.product.seoTags.seoDescription },
              { name: 'robots', content: 'index, follow' },
            ]);
          }
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.router.navigate(['/home/products']);
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loadingData = false;
      },
    );
  }

  pay() {
    this.loadingPay = true;
    this.dataService
      .get(ServerApis.buyProduct, {
        productId: +this.id,
      })
      .subscribe(
        (response) => {
          this.loadingPay = false;
          if (response.isSuccess) {
            this.RefId = response.data.refId;
            var form: any = document.getElementById('payFormMellat');
            this.waitForRedirectToBank = true;
            setTimeout(() => {
              form.submit();
            }, 1000);
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loadingPay = false;
        },
      );
  }
}
