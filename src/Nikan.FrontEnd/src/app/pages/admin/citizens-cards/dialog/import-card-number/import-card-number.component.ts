import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '../../../../../core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-import-card-number-dialog',
  templateUrl: './import-card-number.component.html',
  styleUrls: ['./import-card-number.component.scss'],
  standalone: false,
})
export class AdminImportCardNumberDialogComponent extends AppBase implements OnInit {
  exportInfo: any = {};
  loading: boolean = true;
  baseUrl: string = ServerApis.baseUrl;

  uploadData: any = {};

  uploadUrl: string = ServerApis.importExcelFileCardNumber;
  constructor(
    private matDialogRef: MatDialogRef<AdminImportCardNumberDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) {
    super();
    this.exportInfo = _data.export;
    this.uploadData = {
      ExportId: this.exportInfo.id,
    };
  }

  ngOnInit() {}

  attachmentId(ev) {
    if (ev.importId) this.router.navigate(['/admin/export-details-citizen-card/' + ev.importId]);
    this.matDialogRef.close();
  }
}
