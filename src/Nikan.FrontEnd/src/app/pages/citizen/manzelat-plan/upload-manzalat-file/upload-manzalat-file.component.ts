import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { DataService } from 'src/app/core/services/data-service.service';
import { ServerApis } from 'src/app/core/server-apis';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-upload-manzalat-file',
    templateUrl: './upload-manzalat-file.component.html',
    styleUrls: ['./upload-manzalat-file.component.scss'],
})
export class CitizenUploadManzalatDocumentsComponent implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  hasFiles: boolean = false;
  userId: string;
  cardForm: FormGroup;
  loadingState: boolean;
  baseUrl: string = ServerApis.baseUrl;
  imageUrl: string = '';
  data: any;
  baseInfo: any;
  uploadInfo: any;
  uploadUrl: string = ServerApis.uploadManzalatAttachment;
   id: string;

 
  constructor(
      private toastrService: ToastrService,
      private route: ActivatedRoute,
      private fb: FormBuilder,
      private router: Router,
      private dataService: DataService
  ) {

      this.route.params.subscribe(p => {
          this.id = p.id;
        this.baseFormInfo();
        this.getCitizenRegisterManzalatForm();
          
      });

   
  }

  ngOnInit(): void {}

  
    baseFormInfo() {
        this.loading = true;
        this.dataService.get(ServerApis.getManzalatBaseForm, { id: this.id }).subscribe(response => {
            this.loading = false;
            if (response.isSuccess && response.data) { 
                this.loading = false;
                this.baseInfo = response.data;

            } else {
                
            }
        }, error => {
            this.loading = false;
        });
    }

  getCitizenRegisterManzalatForm() {
    this.loading = true;
    this.dataService.get(ServerApis.getCitizenRegisterManzalatForm, { formBaseId: this.id }).subscribe(response => {
      this.loading = false;
      if (response.isSuccess && response.data) {
        this.loading = false;
        this.uploadInfo = response.data;
        this.hasFiles = response.data.hasFiles;
      } else {

      }
    }, error => {
      this.loading = false;
    });
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




  getImage(ev) { 
    this.imageUrl = ev.uploadUrl;
  }
}
