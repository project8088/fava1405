import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-job-details-dialog',
  templateUrl: './job-details.component.html',
  styleUrls: ['./job-details.component.scss'],
  standalone: false,
})
export class ViewJobDetailsDialogComponent extends AppBase implements OnInit {
  jobId: string = '';

  constructor(
    private matDialogRef: MatDialogRef<ViewJobDetailsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) {
    super();
    if (_data) {
      this.jobId = _data.id;
    }
  }

  ngOnInit(): void {}
}
