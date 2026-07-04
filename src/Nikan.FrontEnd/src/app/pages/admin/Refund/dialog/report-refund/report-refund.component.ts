import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '../../../../../core/custom-validator/form-validation';
import { DataService } from '../../../../../core/services/data-service.service';
import { ServerApis } from '../../../../../core/server-apis';

@Component({
  selector: 'adm-report-refund-dialog',
  templateUrl: './report-refund.component.html',
  styleUrls: ['./report-refund.component.scss'],
})
export class AdminReportRefundDialogComponent implements OnInit {
  isSaving: boolean;
  frm: FormGroup;
  loading: boolean = true;
  importId: string;
  infoReport: any;
  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<AdminReportRefundDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private customValidator: CustomFormValidators,
    private dataService: DataService,
  ) {
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
