import { Component, OnInit, Inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-view-message-dialog',
  templateUrl: './view-message-dialog.component.html',
  styleUrls: ['./view-message-dialog.component.scss'],
})
export class ViewMessageDialogComponent implements OnInit {
  row: any;

  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<ViewMessageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
  ) {
    if (_data) {
      this.row = _data.row;
    }
  }

  ngOnInit(): void {}
}
