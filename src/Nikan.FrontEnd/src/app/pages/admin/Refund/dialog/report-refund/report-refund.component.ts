import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '../../../../../core/custom-validator/form-validation';
import { ServerApis } from '../../../../../core/server-apis';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-report-refund-dialog',
  templateUrl: './report-refund.component.html',
  styleUrls: ['./report-refund.component.scss'],
    standalone: false
})
export class AdminReportRefundDialogComponent extends AppBase implements OnInit {
  isSaving: boolean;
  frm: FormGroup;
  loading: boolean = true;
  importId: string;
  infoReport: any;
  constructor(
    private matDialogRef: MatDialogRef<AdminReportRefundDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators
  ) {
      super();
    if (_data) {
      this.importId = _data.id;
      this.getReport();
    }
  }

  ngOnInit() {}

  getReport() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getReportRefund, {
        importId: this.importId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.infoReport = response.data;
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loading = false;
        },
      );
  }
}
