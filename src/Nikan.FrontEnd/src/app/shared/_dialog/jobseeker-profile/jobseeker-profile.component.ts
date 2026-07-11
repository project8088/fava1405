import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-jobseeker-profile-dialog',
  templateUrl: './jobseeker-profile.component.html',
  styleUrls: ['./jobseeker-profile.component.scss'],
  standalone: false,
})
export class JobseekerProfileDialogComponent extends AppBase implements OnInit {
  id: string = '';

  constructor(
    private matDialogRef: MatDialogRef<JobseekerProfileDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) {
    super();
    if (_data) {
      this.id = _data.id;
    }
  }

  ngOnInit(): void {}

  closeDialog(result: any) {
    this.matDialogRef.close(result);
  }
}
