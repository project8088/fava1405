import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '../../../../../core/server-apis';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-import-citizen-excel-dialog',
  templateUrl: './import-citizen-excel.component.html',
  styleUrls: ['./import-citizen-excel.component.scss'],
    standalone: false
})
export class AdminImportCitizenExcelDialogComponent extends AppBase implements OnInit {
  uploadUrl: string = ServerApis.importCitizenListFromExcel;

  constructor(
    private matDialogRef: MatDialogRef<AdminImportCitizenExcelDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any
  ) {
      super();}

  ngOnInit() {}

  attachmentId(ev) {
    if (ev.importId) this.router.navigate(['/admin/citizen-register-file-details//' + ev.importId]);
    this.matDialogRef.close();
  }
}
