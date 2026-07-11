import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { CropperComponent } from '@app/shared/cropper/cropper.component';

@Component({
  selector: 'app-citizen-edit-image',
  templateUrl: './citizen-edit-image.component.html',
  styleUrls: ['./citizen-edit-image.component.scss'],
  standalone: false,
})
export class AdminCitizenEditImageDialogComponent extends AppBase implements OnInit {
  citizen: any;
  baseUrl = ServerApis.baseUrl;
  isSaving = false;
  uploadUrl: string = ServerApis.uploadPersonalPictureByAdmin;

  @ViewChild('cropper') cropper!: CropperComponent;
  constructor(
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private matDialogRef: MatDialogRef<AdminCitizenEditImageDialogComponent>,
  ) {
    super();
    this.citizen = _data;
  }

  ngOnInit(): void {}
  saveInfo() {
    this.cropper.save();
    // this.matDialogRef.close(true);
  }
}
