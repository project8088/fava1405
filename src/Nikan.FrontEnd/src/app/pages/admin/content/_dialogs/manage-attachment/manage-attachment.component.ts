import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '@core/server-apis';
import Swal from 'sweetalert2';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

declare var $: any;

@Component({
  selector: 'app-manage-attachment-dialog',
  templateUrl: './manage-attachment.component.html',
  styleUrls: ['./manage-attachment.component.scss'],
  standalone: false,
})
export class ManageAttachmentDialogComponent extends AppBase implements OnInit {
  attachments: any[] = [];

  loadingData?: boolean;
  uploadUrl: string = ServerApis.uploadAttachment;
  baseUrl: string = ServerApis.baseUrl;

  data: any = {
    caption: '',
    guid: '',
  };

  constructor(
    private matDialogRef: MatDialogRef<ManageAttachmentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) {
    super();
    if (_data.guid) {
      this.data.guid = _data.guid;
    } else {
      this.toastrService.error('guid  وجود ندارد!');
      matDialogRef.close();
      // this.data.guid = this.newGuid();
    }
  }

  ngOnInit(): void {
    this.loadingData = true;
    this.dataService
      .get(ServerApis.getAttachmentsForAdmin, { guid: this.data.guid })
      .pipe(
        finalize(() => {
          this.loadingData = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess) {
          this.attachments = response.data ? response.data : [];
          this.attachments.forEach((item) => {
            var t = item.filePath.split('.');
            item.fileExtension = t[t.length - 1];
            item.isImage =
              ['jpg', 'jpeg', 'png', 'gif', 'tif', 'bmp'].indexOf(
                item.fileExtension.toLowerCase(),
              ) > -1
                ? true
                : false;
          });
          setTimeout(() => {
            $('.lightGallery').lightGallery({
              selector: 'a',
              thumbnail: false,
            });
          }, 1000);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      });
  }

  completeUpload(ev: any) {
    this.data.caption = '';
    this.ngOnInit();
  }

  newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = (Math.random() * 16) | 0,
        v = c == 'x' ? r : (r & 0x3) | 0x8;
      return v.toString(16);
    });
  }

  deleteAttachment(row: any) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا برای حذف فایل اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        row.loading = true;
        this.dataService.get(ServerApis.removeAttachment, { id: row.id }).subscribe(
          (response) => {
            row.loading = false;
            if (response.isSuccess) {
              this.toastrService.success('حذف فایل با موفقیت انجام شد.');
              this.ngOnInit();
            } else {
              let msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
              this.toastrService.error(msg);
            }
          },
          (error: any) => {
            row.loading = false;
          },
        );
      }
    });
  }
}
