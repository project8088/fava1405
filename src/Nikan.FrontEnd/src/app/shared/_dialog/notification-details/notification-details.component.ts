import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-notification-details-dialog',
  templateUrl: './notification-details.component.html',
  styleUrls: ['./notification-details.component.scss'],
    standalone: false
})
export class ViewNotificationDetailsDialogComponent extends AppBase implements OnInit {
  id: string = '';

  constructor(
    private matDialogRef: MatDialogRef<ViewNotificationDetailsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any
  ) {
      super();
    if (_data) {
      this.id = _data.id;
    }
  }

  ngOnInit(): void {}
}
