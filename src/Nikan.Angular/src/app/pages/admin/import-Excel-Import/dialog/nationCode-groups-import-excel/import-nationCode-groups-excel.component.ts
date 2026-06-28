import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
 import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'; 
  import { ServerApis } from '../../../../../core/server-apis';
import { Router } from '@angular/router';

@Component({
  selector: 'adm-import-nationCode-groups-excel-dialog',
  templateUrl: './import-nationCode-groups-excel.component.html',
  styleUrls: ['./import-nationCode-groups-excel.component.scss']
})
export class AdminImportNationCodeGroupsExcelDialogComponent implements OnInit {
  uploadUrl: string = ServerApis.importGroupNationCodeFromExcel;
  data: any = {
    groupId: 0,
  };




  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<AdminImportNationCodeGroupsExcelDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
     private router: Router
  ) { 
    if (_data.groupId) {
      this.data.groupId = _data.groupId;
    } else {
      toastrService.error("groupId  وجود ندارد!");
      matDialogRef.close(); 
    }

  }
   
  ngOnInit() {
   
  }



  attachmentId(ev) {
    if (ev.importId)
      this.router.navigate(['/admin/citizen-in-group/' + ev.importId]);
    this.matDialogRef.close();
  }

 

  completeUpload(ev) {
    this.data.groupId =0;
    this.ngOnInit();
  }




}
