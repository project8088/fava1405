import { Component, OnInit, Inject } from '@angular/core'; 
import { ToastrService } from 'ngx-toastr';
import * as moment from 'jalali-moment';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-job-details-dialog',
  templateUrl: './job-details.component.html',
  styleUrls: ['./job-details.component.scss']
})
export class ViewJobDetailsDialogComponent implements OnInit {
  jobId: string = '';



  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<ViewJobDetailsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any, 
    private toastrService: ToastrService
  ) {
    if (_data) {
      this.jobId = _data.id;
    }
  }

  ngOnInit(): void {
  }






}
