import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '../../../../core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-importPersonel-excel-dialog',
  templateUrl: './importPersonel-excel.component.html',
  styleUrls: ['./importPersonel-excel.component.scss'],
  standalone: false,
})
export class CompanyImportExcelDialogComponent extends AppBase implements OnInit {
  uploadUrl: string = ServerApis.importPersonnelFromExcel;

  constructor(
    private matDialogRef: MatDialogRef<CompanyImportExcelDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) {
    super();
  }

  ngOnInit() {}

  attachmentId(ev) {
    if (ev.importId) this.router.navigate(['/company/citizen-excel-files-details/' + ev.importId]);
    this.matDialogRef.close();
  }
}
