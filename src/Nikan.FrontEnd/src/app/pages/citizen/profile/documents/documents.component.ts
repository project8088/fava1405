import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import Swal from 'sweetalert2';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'app-citizen-documents',
  templateUrl: './documents.component.html',
  styleUrls: ['./documents.component.scss'],
  standalone: false,
})
export class CitizenDocumentsComponent extends AppBase implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  userId?: string;
  cardForm: FormGroup;
  loadingState: boolean = false;
  baseUrl: string = ServerApis.baseUrl;
  imageUrl: string = '';
  data: any;
  uploadUrl: string = ServerApis.uploadDocGroupAttachment;

  baseDocuments: any[] = [];
  displayedColumns: string[] = ['documentGroup', 'description', 'attachedOnDate', 'id'];

  constructor() {
    super();
    this.cardForm = this.fb.group({
      personnelImage: [null, [Validators.required]],
    });

    this.getDocGroupsBaseList();
  }

  ngOnInit(): void {}

  getDocGroupsBaseList() {
    this.loading = true;
    this.dataService.get(ServerApis.getAllDocGrpupsAndUserDocuments)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((data) => {
            this.baseDocuments = data.data;
          });
  }
  removeDocument(id: number, title: string) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا برای حذف "' + title + '" اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService.get(ServerApis.removeUserDocument + '?id=' + id).subscribe((response) => {
          if (response.isSuccess) {
            this.toastrService.success('با موفقیت حذف شد.');
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }

          this.getDocGroupsBaseList();
        });
      }
    });
  }

  saveForm() {
    const form = this.cardForm.getRawValue();
    this.dataService.post(ServerApis.uploadPersonalPicture, form).subscribe(
      (response) => {
        if (response && response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error: any) => {
        this.isSaving = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  getImage(ev: any) {
    this.getDocGroupsBaseList();
    this.imageUrl = ev.uploadUrl;
  }
}
