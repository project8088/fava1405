import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'company-signature-info',
  templateUrl: './signature-info.component.html',
  styleUrls: ['./signature-info.component.scss'],
  standalone: false,
})
export class CompanySignatureInfoComponent extends AppBase implements OnInit, AfterViewInit {
  loading?: boolean;
  companyId: string = '';

  baseUrl: string = ServerApis.baseUrl;
  imageSignatureUrl: string = '';

  signatureUrl: string = ServerApis.companyUploadSignature;

  data: any;

  constructor(private customValidators: CustomFormValidators) {
    super();
    this.route.params.subscribe((p) => {
      if (p['id'] != '0' && p['id']) this.companyId = p['id'];
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
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess && response.data) {
          //this.companyId = response.data.companyId;
          this.imageSignatureUrl = response.data.signatureUrl;
        } else {
          var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      });
  }

  getSignature(ev: any) {
    this.imageSignatureUrl = ev.signatureUrl;
  }
}
