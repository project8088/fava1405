import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
 import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'; 
  import { ServerApis } from '../../../../../core/server-apis';
import { Router } from '@angular/router';

@Component({
  selector: 'adm-import-refund-excel-dialog',
  templateUrl: './import-refund-excel.component.html',
  styleUrls: ['./import-refund-excel.component.scss']
})
export class AdminImportRefundExcelDialogComponent implements OnInit {
  uploadUrl: string = ServerApis.importRefundListFromExcel;

  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<AdminImportRefundExcelDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
     private router: Router
  ) { 
   

  }
   
  ngOnInit() {
   
  }



  attachmentId(ev) {
    if (ev.importId)
      this.router.navigate(['/admin/refund-file-details/' + ev.importId]);
    this.matDialogRef.close();
  }

 

 




}
