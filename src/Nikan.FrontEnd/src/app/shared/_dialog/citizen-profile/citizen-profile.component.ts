import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-profile-dialog',
  templateUrl: './citizen-profile.component.html',
  styleUrls: ['./citizen-profile.component.scss'],
  standalone: false,
})
export class CitizenProfileDialogComponent extends AppBase implements OnInit {
  userCode: string = '';

  constructor(
    private matDialogRef: MatDialogRef<CitizenProfileDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) {
    super();
    debugger;
    if (_data) {
      this.userCode = _data.userCode;
    }
  }

  ngOnInit(): void {}

  closeDialog(result: boolean) {
    this.matDialogRef.close(result);
  }
}
