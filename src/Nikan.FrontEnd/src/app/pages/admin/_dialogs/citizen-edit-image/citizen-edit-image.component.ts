import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from 'src/app/core/server-apis';
import { CropperComponent } from 'src/app/shared/cropper/cropper.component';

@Component({
  selector: 'app-citizen-edit-image',
  templateUrl: './citizen-edit-image.component.html',
  styleUrls: ['./citizen-edit-image.component.scss'],
})
export class AdminCitizenEditImageDialogComponent implements OnInit {
  citizen: any;
  baseUrl = ServerApis.baseUrl;
  isSaving: boolean;
  uploadUrl: string = ServerApis.uploadPersonalPictureByAdmin;

  @ViewChild('cropper') cropper: CropperComponent;
  constructor(
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private matDialogRef: MatDialogRef<AdminCitizenEditImageDialogComponent>,
  ) {
    this.citizen = _data;
  }

  ngOnInit(): void {}
  getImage(event) {}
  saveInfo() {
    this.cropper.save();
    // this.matDialogRef.close(true);
  }
}
