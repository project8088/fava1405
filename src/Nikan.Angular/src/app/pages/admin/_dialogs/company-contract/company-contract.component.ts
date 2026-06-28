import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';  
 import { ServerApis } from '../../../../core/server-apis';
import { DataService } from '../../../../core/services/data-service.service';

@Component({
  selector: 'adm-company-contract-dialog',
  templateUrl: './company-contract.component.html',
  styleUrls: ['./company-contract.component.scss']
})
export class AdminCompanyContractDialogComponent implements OnInit {
   companyInfo: any = {};
  loading: boolean = true;
  baseUrl: string = ServerApis.baseUrl;
  imageContractUrl: string = '';

  uploadData: any = {};

  contractUrl: string = ServerApis.companyUploadContract;

   constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<AdminCompanyContractDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
    private fb: FormBuilder,  
     private dataService: DataService
   ) {
  
     this.companyInfo = _data.company;
     this.uploadData = {
       CompanyId: this.companyInfo.companyId
     };
     this.getInfo();
  }

 



  ngOnInit() {
   
  }

  




  getInfo() {
    this.loading = true;
    this.dataService.get(ServerApis.getCompanyContract, {
      companyId: this.companyInfo.companyId
    }).subscribe(response => {
      this.loading = false;
      if (response.isSuccess && response.data) {
        //this.companyId = response.data.companyId;
        this.imageContractUrl = response.data.contractUrl;

      } else {
        var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
        this.toastrService.error(msg);
      }
    }, error => {
      this.loading = false;
    });
  }






  getContract(ev) {
    this.imageContractUrl = ev.contractUrl;

  }
  



}
