import { Component, OnInit, Inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-notification-details-dialog',
  templateUrl: './notification-details.component.html',
  styleUrls: ['./notification-details.component.scss'],
})
export class ViewNotificationDetailsDialogComponent implements OnInit {
  id: string = '';

  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<ViewNotificationDetailsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
  ) {
    if (_data) {
      this.id = _data.id;
    }
  }

  ngOnInit(): void {}
}
