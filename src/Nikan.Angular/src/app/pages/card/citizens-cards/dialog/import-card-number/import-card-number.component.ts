import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';  
import { ServerApis } from '../../../../../core/server-apis';
import { DataService } from '../../../../../core/services/data-service.service';
import { Router } from '@angular/router';
 

@Component({
  selector: 'card-import-card-number-dialog',
  templateUrl: './import-card-number.component.html',
  styleUrls: ['./import-card-number.component.scss']
})
export class CardImportCardNumberDialogComponent implements OnInit {
  exportInfo: any = {};
  loading: boolean = true;
  baseUrl: string = ServerApis.baseUrl;
  uploadData: any = {};

  uploadUrl: string = ServerApis.importExcelFileCardNumber; 
   constructor(
    private matDialog: MatDialog,
     private matDialogRef: MatDialogRef<CardImportCardNumberDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
     private toastrService: ToastrService,
     private router: Router,
    private fb: FormBuilder,  
     private dataService: DataService
   ) {
  
     this.exportInfo = _data.export;
     this.uploadData = {
       ExportId: this.exportInfo.id
     };
      
  }

 



  ngOnInit() {
   
  }

  

   



  attachmentId(ev) {
    if (ev.importId)
      this.router.navigate(['/card/export-details-citizen-card/' + ev.importId]);
    this.matDialogRef.close();
  }
  



}
