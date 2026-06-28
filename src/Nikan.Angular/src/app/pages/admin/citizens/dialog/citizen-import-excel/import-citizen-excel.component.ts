import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
 import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'; 
  import { ServerApis } from '../../../../../core/server-apis';
import { Router } from '@angular/router';

@Component({
  selector: 'adm-import-citizen-excel-dialog',
  templateUrl: './import-citizen-excel.component.html',
  styleUrls: ['./import-citizen-excel.component.scss']
})
export class AdminImportCitizenExcelDialogComponent implements OnInit {
  uploadUrl: string = ServerApis.importCitizenListFromExcel;

  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<AdminImportCitizenExcelDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
     private router: Router
  ) { 
   

  }
   
  ngOnInit() {
   
  }



  attachmentId(ev) {
    if (ev.importId)
      this.router.navigate(['/admin/citizen-register-file-details//' + ev.importId]);
    this.matDialogRef.close();
  }

 

 




}
