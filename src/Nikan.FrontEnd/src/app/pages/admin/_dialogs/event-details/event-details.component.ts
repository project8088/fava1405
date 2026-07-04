import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'admin-event-details-dialog',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.scss'],
})
export class AdminViewEventDetailsDialogComponent extends AppBase implements OnInit {
  id: string = '';

  constructor(
    private matDialogRef: MatDialogRef<AdminViewEventDetailsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any
  ) {
      super();
    if (_data) {
      this.id = _data.id;
    }
  }

  ngOnInit(): void {}
}
