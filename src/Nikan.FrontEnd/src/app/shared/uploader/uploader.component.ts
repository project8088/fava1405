import { Component, OnInit, EventEmitter, Input, Output } from '@angular/core';

import {
  UploadOutput,
  UploadInput,
  UploadFile,
  humanizeBytes,
  UploaderOptions,
  UploadStatus,
} from 'ngx-uploader';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { ServerApis } from '../../core/server-apis';
import { AuthService } from '../../core/authentication/auth.service';

@Component({
  selector: 'app-uploader',
  templateUrl: './uploader.component.html',
  styleUrls: ['./uploader.component.scss'],
})
export class UploaderComponent implements OnInit {
  @Input('autoUpload') autoUpload: boolean = true;
  @Input('removeAfterUpload') removeAfterUpload: boolean = false;
  @Input('data') data: any = {};
  @Input('btnTitle') btnTitle: string = ' انتخاب فایل';
  @Input('url') url: string = ServerApis.uploaderUrl;
  //['application/pdf']
  @Input('accept') accept: string[] = [
    'image/png',
    'image/jpg',
    'image/gif',
    'image/jpeg',
    'image/bmp',
  ];
  @Output('attachmentId') attachmentId = new EventEmitter<string>();

  files: UploadFile[] = [];
  uploaderInput: EventEmitter<UploadInput>;

  humanizeBytes: Function;
  dragOver: boolean;
  options: UploaderOptions;
  acceptedInputFile: string;

  errorInUpload: boolean = false;
  constructor(
    private toastrService: ToastrService,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.options = {
      concurrency: 1,
      maxUploads: 1,
      maxFileSize: 1024 * 1024 * 5, //5MB
      allowedContentTypes: this.accept,
    };
    this.uploaderInput = new EventEmitter<UploadInput>(); // input events, we use this to emit data to ngx-uploader
    this.humanizeBytes = humanizeBytes;
    this.acceptedInputFile = this.options.allowedContentTypes.join(',');

    console.log(this.accept);
    console.log(this.acceptedInputFile);
  }

  /**
   * آپلود مجوز کاریابی
   * @param output
   */
  onUploadOutput(output: UploadOutput): void {
    switch (output.type) {
      case 'allAddedToQueue':
        if (this.autoUpload) {
          this.startUpload();
        }
        break;
      case 'addedToQueue':
        if (typeof output.file !== 'undefined') {
          this.files.push(output.file);
        }
        break;
      case 'uploading':
        if (typeof output.file !== 'undefined') {
          // update current data in files array for uploading file
          const index = this.files.findIndex(
            (file) => typeof output.file !== 'undefined' && file.id === output.file.id,
          );
          this.files[index] = output.file;
        }
        break;
      case 'removed':
        // remove file from array when removed
        this.files = this.files.filter((file: UploadFile) => file !== output.file);
        break;
      case 'dragOver':
        this.dragOver = true;
        break;
      case 'dragOut':

      case 'rejected':
        if (this.options.allowedContentTypes.indexOf(output.file.type) < 0) {
          this.toastrService.error('نوع فایل مجاز نیست!');
        } else if (output.file.size > this.options.maxFileSize) {
          this.toastrService.error('حداکثر 5 مگابایت', 'حجم فایل بیش از حد مجاز است.');
        }
      case 'drop':
        this.dragOver = false;
        break;
      case 'done':
        if (output.file.responseStatus == 200 && output.file.response.isSuccess) {
          this.toastrService.success('آپلود فایل با موفقیت انجام شد!');
          Swal.fire({
            icon: 'success',
            title: 'آپلود فایل',
            text: 'آپلود فایل با موفقیت انجام شد.',
            confirmButtonText: 'بستن',
            showConfirmButton: true,
            showCancelButton: false,
          });
          this.attachmentId.emit(output.file.response.data);
        } else if (output.file.responseStatus == 200 && output.file.response.isSuccess == false) {
          let msg = output.file.response.messages
            ? output.file.response.messages
            : 'متاسفانه سرور با خطا مواجه شده است.';
          this.toastrService.error(msg);
          this.errorInUpload = true;
        } else {
          this.toastrService.error('متاسفانه آپلود فایل با خطا مواجه شده است.');
          this.errorInUpload = true;
        }
        if (this.removeAfterUpload) {
          this.removeAllFiles(this.uploaderInput);
          this.files = [];
        }

        break;
    }
  }

  /**
   * شروع آپلود فایل
   * */
  startUpload(): void {
    this.errorInUpload = false;
    const event: UploadInput = {
      type: 'uploadAll',
      url: this.url,
      method: 'POST',
      data: this.data,

      withCredentials: false,
      headers: { Authorization: 'Bearer ' + this.authService.getRawAuthToken() }, // <----  set headers
      includeWebKitFormBoundary: true, // <----  set WebKitFormBoundary
    };

    this.uploaderInput.emit(event);
  }

  cancelUpload(id: string, uploaderInput: EventEmitter<UploadInput>): void {
    uploaderInput.emit({ type: 'cancel', id: id });
  }

  removeFile(id: string, uploaderInput: EventEmitter<UploadInput>): void {
    uploaderInput.emit({ type: 'remove', id: id });
  }

  removeAllFiles(uploaderInput: EventEmitter<UploadInput>): void {
    uploaderInput.emit({ type: 'removeAll' });
  }

  onChange() {
    this.removeAllFiles(this.uploaderInput);
    this.files = [];
  }
}
