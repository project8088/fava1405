import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-card-profile-dialog',
  templateUrl: './card-profile.component.html',
  styleUrls: ['./card-profile.component.scss'],
  standalone: false,
})
export class CardProfileDialogComponent extends AppBase implements OnInit {
  id: string;

  constructor(
    private matDialogRef: MatDialogRef<CardProfileDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) {
    super();
    debugger;
    if (_data) {
      this.id = _data.id;
    }
  }

  ngOnInit(): void {}

  closeDialog(result) {
    this.matDialogRef.close(result);
  }
}
