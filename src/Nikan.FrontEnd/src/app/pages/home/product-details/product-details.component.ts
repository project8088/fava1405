import { Component, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { DataService } from '../../../core/services/data-service.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthUser } from '../../../core/authentication/user.model';
import { AuthService } from '../../../core/authentication/auth.service';
import { ServerApis } from '../../../core/server-apis';
import { CustomFormValidators } from '../../../core/custom-validator/form-validation';

import { Title, Meta } from '@angular/platform-browser';
@Component({
  selector: 'home-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class MainProductDetailsComponent implements OnInit {
  id: string;
  user: AuthUser;
  loadingData: boolean;
  product: any;

  baseUrl: string = ServerApis.baseUrl;
  waitForRedirectToBank: boolean;
  RefId: string = '';
  loadingPay: boolean;

  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private router: Router,
    private matDialog: MatDialog,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authService: AuthService,
    private customValidator: CustomFormValidators,
    private titleService: Title,
    private metaService: Meta
  ) {


    this.route.params.subscribe(p => {
      this.id = p.id;
      this.getDetailsInfo();
    });
    this.user = this.authService.currentUserValue;
  }



  ngOnInit() {

  }




  getDetailsInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getCompanyProduct, { id: this.id }).subscribe(response => {
      this.loadingData = false;
      if (response.isSuccess) {
        this.product = response.data;

        if (this.product.seoTags) {
          this.titleService.setTitle(this.product.seoTags.seoTitle);
          this.metaService.addTags([
            { name: 'keywords', content: this.product.seoTags.seokeywords },
            { name: 'description', content: this.product.seoTags.seoDescription },
            { name: 'robots', content: 'index, follow' }
          ]);
        }
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.router.navigate(['/home/products']);
        this.toastrService.error(msg);
      }

    }, error => {
      this.loadingData = false;

    });
  }





  pay() {
    this.loadingPay = true;
    this.dataService.get(ServerApis.buyProduct, {
      productId: +this.id
    }).subscribe(response => {
      this.loadingPay = false;
      if (response.isSuccess) {
        this.RefId = response.data.refId;
        var form: any = document.getElementById('payFormMellat');
        this.waitForRedirectToBank = true;
        setTimeout(() => { form.submit(); }, 1000);
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
      this.loadingPay = false;
    });

  }



}


