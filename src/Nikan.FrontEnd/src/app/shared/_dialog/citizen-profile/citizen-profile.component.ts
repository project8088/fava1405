import { Component, OnInit, Inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-citizen-profile-dialog',
  templateUrl: './citizen-profile.component.html',
  styleUrls: ['./citizen-profile.component.scss'],
})
export class CitizenProfileDialogComponent implements OnInit {
  userCode: string;

  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<CitizenProfileDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) {
    debugger;
    if (_data) {
      this.userCode = _data.userCode;
    }
  }

  ngOnInit(): void {}

  closeDialog(result) {
    this.matDialogRef.close(result);
  }
}
