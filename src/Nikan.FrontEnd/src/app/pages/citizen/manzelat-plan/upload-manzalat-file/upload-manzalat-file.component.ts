import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import Swal from 'sweetalert2';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-upload-manzalat-file',
  templateUrl: './upload-manzalat-file.component.html',
  styleUrls: ['./upload-manzalat-file.component.scss'],
  standalone: false,
})
export class CitizenUploadManzalatDocumentsComponent extends AppBase implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  hasFiles: boolean = false;
  userId?: string;
  loadingState: boolean = false;
  baseUrl: string = ServerApis.baseUrl;
  imageUrl: string = '';
  data: any;
  baseInfo: any;
  uploadInfo: any;
  uploadUrl: string = ServerApis.uploadManzalatAttachment;
  id: string = '';

  constructor() {
    super();
    this.route.params.subscribe((p) => {
      this.id = p['id'];
      this.baseFormInfo();
      this.getCitizenRegisterManzalatForm();
    });
  }

  ngOnInit(): void {}

  baseFormInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getManzalatBaseForm, { id: this.id })
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response.isSuccess && response.data) {
            this.loading = false;
            this.baseInfo = response.data;
          } else {
          }
        },
        (error: any) => {},
      );
  }

  getCitizenRegisterManzalatForm() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getCitizenRegisterManzalatForm, { formBaseId: this.id })
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response.isSuccess && response.data) {
            this.loading = false;
            this.uploadInfo = response.data;
            this.hasFiles = response.data.hasFiles;
          } else {
          }
        },
        (error: any) => {},
      );
  }

  removeDocument() {
    Swal.fire({
      title: 'حذف',
      text: 'آیا برای حذف   اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService
          .get(ServerApis.removeManzalatAttachment + '?id=' + this.id)
          .subscribe((response) => {
            if (response.isSuccess) {
              this.toastrService.success('با موفقیت حذف شد.');
              window.location.reload();
            } else {
              let msg = response.messages
                ? response.messages
                : 'متاسفانه خطایی در سرور رخ داده است!';
              this.toastrService.error(msg);
            }
          });
      }
    });
  }

  getImage(ev: any) {
    this.imageUrl = ev.uploadUrl;
  }
}
