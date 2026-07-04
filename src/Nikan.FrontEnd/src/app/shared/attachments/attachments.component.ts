import { Component, OnInit, Input } from '@angular/core';
import { ServerApis } from '../../core/server-apis';
import { AppBase } from '@app/app.base';

declare var $: any;

@Component({
  selector: 'app-attachments',
  templateUrl: './attachments.component.html',
  styleUrls: ['./attachments.component.scss'],
  standalone: false,
})
export class AttachmentListComponent extends AppBase implements OnInit {
  @Input('guid') guid: string;
  attachments: any[] = [];

  loadingData: boolean;
  uploadUrl: string = ServerApis.uploadAttachment;
  baseUrl: string = ServerApis.baseUrl;

  constructor() {
    super();
  }

  ngOnInit() {
    if (!this.guid) return false;

    this.loadingData = true;
    this.dataService.get(ServerApis.getAttachmentsForUser, { guid: this.guid }).subscribe(
      (response) => {
        this.loadingData = false;
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
      },
      (error) => {
        this.loadingData = false;
      },
    );
  }
}
