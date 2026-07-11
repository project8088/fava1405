import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ImageCroppedEvent, ImageTransform } from 'ngx-image-cropper';
import { AuthUser } from '@core/authentication/user.model';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-upload-avatar-dialog',
  templateUrl: './upload-avatar.component.html',
  styleUrls: ['./upload-avatar.component.scss'],
  standalone: false,
})
export class UploadUserAvatarDialogComponent extends AppBase implements OnInit {
  imageChangedEvent: any = '';
  croppedImage: any = '';

  loading?: boolean;
  saving = false;
  canvasRotation = 0;
  rotation = 0;
  transform: ImageTransform = {};

  imageUrl: string = '';
  imageBase64: string = '';

  user?: AuthUser | null;
  constructor(
    private matDialogRef: MatDialogRef<UploadUserAvatarDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) {
    super();
    this.user = this.authService.currentUserValue;
    this.imageUrl = ServerApis.baseUrl + _data.imageUrl;
    this.setCropperImage(this.imageUrl);
  }

  ngOnInit(): void {}

  fileChangeEvent(event: any): void {
    this.loading = true;
    this.imageChangedEvent = event;
  }
  imageCropped(event: ImageCroppedEvent) {
    this.croppedImage = event.base64;
  }
  imageLoaded() {
    this.loading = false;
    // show cropper
  }
  cropperReady() {
    // cropper ready
    console.log('cropperReady: ');
  }
  loadImageFailed() {
    // show message
    this.toastrService.error(
      'متاسفانه برش فایل انتخابی شما امکان پذیر نیست!',
      'لطفا فایل تصویر دیگری را انتخاب کنید',
    );
  }

  async setCropperImage(url: string) {
    const result: any = await fetch(url);
    const blob = await result.blob();
    let reader = new FileReader();
    reader.readAsDataURL(blob);
    reader.onload = () => {
      console.log(reader.result);
      this.imageBase64 = (reader.result as any) ?? '';
    };
  }

  save() {
    if (!this.user) return;
    var url = '';
    if (this.user.isJobseeker) url = ServerApis.uploadJobseekerImage;
    else if (this.user.isCompany) url = ServerApis.uploadCompanyImage;
    else if (this.user.isCitizen) url = ServerApis.uploadPersonalPicture;

    if (!url) return;

    this.saving = true;
    var b64toBlob = this.convertBase64ImageToBlobFile(this.croppedImage);

    this.dataService.postFormData(url, { file: b64toBlob }).subscribe(
      (response) => {
        this.saving = false;
        if (response.isSuccess) {
          this.toastrService.success('ذخیره تصویر با موفقیت انجام شد.');

          var userImage = this.user?.isCitizen ? response.data.uploadUrl : response.data.imageUrl;
          this.matDialogRef.close(userImage);
        } else {
          var msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      },
      (error: any) => {
        this.saving = false;
      },
    );
  }

  convertBase64ImageToBlobFile(b64Data: string, contentType = 'image/png', sliceSize = 1024) {
    b64Data = b64Data.replace('data:image/png;base64,', '');
    const byteCharacters = atob(b64Data);
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      const slice = byteCharacters.slice(offset, offset + sliceSize);

      const byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }

      const byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }

    var blob: any = new Blob(byteArrays, { type: contentType });
    blob.name = 'avatar' + Math.round(Math.random() * 10000) + '.png';
    return blob;
  }

  rotateLeft() {
    this.loading = true;
    this.canvasRotation--;
    this.flipAfterRotate();
  }

  rotateRight() {
    this.loading = true;
    this.canvasRotation++;
    this.flipAfterRotate();
  }
  private flipAfterRotate() {
    const flippedH = this.transform.flipH;
    const flippedV = this.transform.flipV;
    this.transform = {
      ...this.transform,
      flipH: flippedV,
      flipV: flippedH,
    };
  }
}
