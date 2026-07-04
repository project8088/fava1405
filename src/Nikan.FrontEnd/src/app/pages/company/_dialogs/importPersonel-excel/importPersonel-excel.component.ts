import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Router } from '@angular/router';
import { ServerApis } from '../../../../core/server-apis';

@Component({
  selector: 'app-importPersonel-excel-dialog',
  templateUrl: './importPersonel-excel.component.html',
  styleUrls: ['./importPersonel-excel.component.scss'],
})
export class CompanyImportExcelDialogComponent implements OnInit {
  uploadUrl: string = ServerApis.importPersonnelFromExcel;

  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<CompanyImportExcelDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
    private router: Router,
  ) {}

  ngOnInit() {}

  attachmentId(ev) {
    if (ev.importId) this.router.navigate(['/company/citizen-excel-files-details/' + ev.importId]);
    this.matDialogRef.close();
  }
}
