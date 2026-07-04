import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { ServerApis } from '../../../../core/server-apis';
import { DataService } from '../../../../core/services/data-service.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import * as CkEditor from '../../../../../assets/ckeditor';

@Component({
  selector: 'company-signature-info',
  templateUrl: './signature-info.component.html',
  styleUrls: ['./signature-info.component.scss'],
})
export class CompanySignatureInfoComponent implements OnInit, AfterViewInit {
  loading: boolean;
  companyId: string = '';

  baseUrl: string = ServerApis.baseUrl;
  imageSignatureUrl: string = '';

  signatureUrl: string = ServerApis.companyUploadSignature;

  data: any;

  constructor(
    private fb: FormBuilder,
    private customValidators: CustomFormValidators,
    private dataService: DataService,
    private toastrService: ToastrService,
    private route: ActivatedRoute,
  ) {
    this.route.params.subscribe((p) => {
      if (p.id != '0' && p.id) this.companyId = p.id;
      this.data = {
        CompanyId: this.companyId,
        CompanyName: '',
        SignatureUrl: '',
      };
      this.getInfo();
    });
  }

  ngAfterViewInit(): void {}

  ngOnInit(): void {}

  getInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getCompanySignature, {
        companyId: this.companyId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            //this.companyId = response.data.companyId;
            this.imageSignatureUrl = response.data.signatureUrl;
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

  getSignature(ev) {
    this.imageSignatureUrl = ev.signatureUrl;
  }
}
