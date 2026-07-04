import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '../../../../../core/server-apis';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-import-refund-excel-dialog',
  templateUrl: './import-refund-excel.component.html',
  styleUrls: ['./import-refund-excel.component.scss'],
    standalone: false
})
export class AdminImportRefundExcelDialogComponent extends AppBase implements OnInit {
  uploadUrl: string = ServerApis.importRefundListFromExcel;

  constructor(
    private matDialogRef: MatDialogRef<AdminImportRefundExcelDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any
  ) {
      super();}

  ngOnInit() {}

  attachmentId(ev) {
    if (ev.importId) this.router.navigate(['/admin/refund-file-details/' + ev.importId]);
    this.matDialogRef.close();
  }
}
