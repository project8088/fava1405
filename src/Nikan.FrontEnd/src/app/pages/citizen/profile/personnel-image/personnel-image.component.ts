import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';

import { CitizenProfileComponent } from '../profile.component';
import { ServerApis } from '@core/server-apis';
import { SideNavMenuComponent } from 'src/app/shared/side-nav-menu/side-nav-menu.component';
import { UploadUserAvatarDialogComponent } from 'src/app/shared/_dialog/upload-avatar/upload-avatar.component';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-personnel-image',
  templateUrl: './personnel-image.component.html',
  styleUrls: ['./personnel-image.component.scss'],
})
export class CitizenPersonnelImageComponent extends AppBase implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  personalPictureIsUploaded: boolean = false;
  personalPicture_Confirmed: number = null;
  userId: string;
  cardForm: FormGroup;
  loadingState: boolean;
  baseUrl: string = ServerApis.baseUrl;
  imageUrl: string = '';
  data: any;
  uploadUrl: string = ServerApis.uploadPersonalPicture;
  imageStates = ['عدم تایید', 'تایید شده', 'در حال بررسی'];

  constructor(
    private profileComponent: CitizenProfileComponent,
    // private navBar: SideNavMenuComponent
  ) {
      super();
    this.cardForm = this.fb.group({
      personnelImage: [null, [Validators.required]],
    });

    this.getShortCitizenInfoByCitizen();
  }

  ngOnInit(): void {}

  getShortCitizenInfoByCitizen() {
    this.dataService.get(ServerApis.getShortCitizenInfoByCitizen).subscribe((data) => {
      this.loading = false;
      this.imageUrl = data.data.personalPictureUrl;
      this.personalPictureIsUploaded = data.data.personalPictureIsUploaded;
      this.personalPicture_Confirmed = data.data.personalPicture_Confirmed || 0;
    });
  }

  getImage(ev) {
    this.imageUrl = ev.uploadUrl;
    window.location.reload();
  }

  openUploadDialog() {
    this.matDialog
      .open(UploadUserAvatarDialogComponent, {
        data: { imageUrl: this.imageUrl },
        panelClass: 'custom-dialog',
        width: '85%',
        height: '90%',
      })
      .afterClosed()
      .subscribe((res) => {
        if (res) {
          this.profileComponent.getPersonalInfo();
          //this.navBar.getCitizenImage();
          this.imageUrl = res + '?v=' + Math.random() * 1000;
        }
      });
  }
}
