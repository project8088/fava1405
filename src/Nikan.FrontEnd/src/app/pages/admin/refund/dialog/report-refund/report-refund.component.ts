import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'adm-report-refund-dialog',
  templateUrl: './report-refund.component.html',
  styleUrls: ['./report-refund.component.scss'],
  standalone: false,
})
export class AdminReportRefundDialogComponent extends AppBase implements OnInit {
  isSaving = false;
  loading: boolean = true;
  importId: string = '';
  infoReport: any;
  constructor(
    private matDialogRef: MatDialogRef<AdminReportRefundDialogComponent>,
    @Inject(MAT_DIALOG_DATA) _data: any,
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
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess && response.data) {
          this.infoReport = response.data;
        } else {
          var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      });
  }
}
