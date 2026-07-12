import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'adm-company-contract-dialog',
  templateUrl: './company-contract.component.html',
  styleUrls: ['./company-contract.component.scss'],
  standalone: false,
})
export class AdminCompanyContractDialogComponent extends AppBase implements OnInit {
  companyInfo: any = {};
  loading: boolean = true;
  baseUrl: string = ServerApis.baseUrl;
  imageContractUrl: string = '';

  uploadData: any = {};

  contractUrl: string = ServerApis.companyUploadContract;

  constructor(
    private matDialogRef: MatDialogRef<AdminCompanyContractDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) {
    super();
    this.companyInfo = _data.company;
    this.uploadData = {
      CompanyId: this.companyInfo.companyId,
    };
    this.getInfo();
  }

  ngOnInit() {}

  getInfo() {
    this.loading = true;
    this.dataService
            .get(ServerApis.getCompanyContract, {
              companyId: this.companyInfo.companyId,
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
                  this.imageContractUrl = response.data.contractUrl;
                } else {
                  var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
                  this.toastrService.error(msg);
                }
              }, (error: any) => {
              });
  }

  getContract(ev: any) {
    this.imageContractUrl = ev.contractUrl;
  }
}
