import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-view-message-dialog',
  templateUrl: './view-message-dialog.component.html',
  styleUrls: ['./view-message-dialog.component.scss'],
    standalone: false
})
export class ViewMessageDialogComponent extends AppBase implements OnInit {
  row: any;

  constructor(
    private matDialogRef: MatDialogRef<ViewMessageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any
  ) {
      super();
    if (_data) {
      this.row = _data.row;
    }
  }

  ngOnInit(): void {}
}
