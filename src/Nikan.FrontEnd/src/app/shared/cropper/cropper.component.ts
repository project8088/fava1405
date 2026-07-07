import { Component, Input, OnInit } from '@angular/core';
import { ImageCroppedEvent, ImageTransform } from 'ngx-image-cropper';
import { AuthUser } from '@core/authentication/user.model';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-cropper',
  templateUrl: './cropper.component.html',
  styleUrls: ['./cropper.component.scss'],
  standalone: false,
})
export class CropperComponent extends AppBase implements OnInit {
  imageChangedEvent: any = '';
  croppedImage: any = '';

  loading?: boolean;
  saving = false;
  canvasRotation = 0;
  rotation = 0;
  transform: ImageTransform = {};
  user: AuthUser | null = null;
  @Input('imageUrl') imageUrl: string = '';
  @Input('uploadUrl') uploadUrl: string = '';
  @Input('data') data: any;
  @Input('showSaveButton') showSaveButton = true;

  imageBase64: string = '';

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.setCropperImage(this.imageUrl);
  }
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

  async setCropperImage(url: string) {
    const result: any = await fetch(url);
    const blob = await result.blob();
    let reader = new FileReader();
    reader.readAsDataURL(blob);
    reader.onload = () => {
      console.log(reader.result);
      this.imageBase64 = (reader.result as any) ?? ' ';
    };
  }

  save() {
    this.saving = true;
    var url = this.uploadUrl;
    if (!url) return;
    var b64toBlob = this.convertBase64ImageToBlobFile(this.croppedImage);

    const data = this.data;

    this.dataService.postFormData(url, { file: b64toBlob, ...data }).subscribe(
      (response) => {
        this.saving = false;
        if (response.isSuccess) {
          this.toastrService.success('ذخیره تصویر با موفقیت انجام شد.');

          var userImage = this.user?.isCitizen ? response.data.uploadUrl : response.data.imageUrl;
        } else {
          var msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.saving = false;
      },
    );
  }
}
