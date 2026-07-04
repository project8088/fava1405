import { Component, OnInit, Inject } from '@angular/core';
import * as moment from 'jalali-moment';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-job-details-dialog',
  templateUrl: './job-details.component.html',
  styleUrls: ['./job-details.component.scss'],
})
export class ViewJobDetailsDialogComponent extends AppBase implements OnInit {
  jobId: string = '';

  constructor(
    private matDialogRef: MatDialogRef<ViewJobDetailsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any
  ) {
      super();
    if (_data) {
      this.jobId = _data.id;
    }
  }

  ngOnInit(): void {}
}
