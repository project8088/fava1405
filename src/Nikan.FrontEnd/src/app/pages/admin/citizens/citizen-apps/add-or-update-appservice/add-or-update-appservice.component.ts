import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-add-or-update-appservice',
  templateUrl: './add-or-update-appservice.component.html',
  styleUrls: ['./add-or-update-appservice.component.scss'],
  standalone: false,
})
export class AdminAddOrUpdateAppserviceComponent extends AppBase implements OnInit, AfterViewInit {
  isUpdate = false;
  serviceId?: string;
  storeForm: FormGroup;
  baseUrl = ServerApis.baseUrl;

  isSaving = false;
  imageUrl: string = '';
  loading?: boolean;

  constructor() {
    super();
    this.route.params.subscribe((p) => {
      if (p['id'] && p['id'] != '0') {
        this.isUpdate = true;
        this.serviceId = p['id'];
        this.getStoreInfo();
      } else {
        this.serviceId = '';
        this.isUpdate = false;
      }
    });

    this.storeForm = this.fb.group({
      serviceId: [null],
      serviceName: [null, [Validators.required, Validators.maxLength(500)]],
      description: [null, [Validators.required, Validators.maxLength(2000)]],
      terms: [null],
      cssClass: [null],
      icon: [null],
      paramName1: [null],
      paramName2: [null],
      paramValue1: [null],
      paramValue2: [null],
      link: [null, [Validators.required]],

      priority: [0],
      isLinkService: [true, []],
      isNeedAuthenticate: [true, []],
      openInNewWindow: [true, []],
      isMain: [true, []],
      haveTerms: [true, []],
      isActive: [true, []],
      isShowInDashbordCitizen: [false, []],
    });
  }

  ngOnInit(): void {}

  ngAfterViewInit() {}

  getStoreInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getAppInfo, {
        id: this.serviceId,
        forEdit: true,
      })
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        this.loading = false;
        if (response.isSuccess && response.data) {
          this.storeForm.patchValue(response.data);
          this.imageUrl = response.data.imageUrl;
        } else {
          var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      });
  }

  /**
   * for bind object in select
   * @param item
   */
  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? +c1.key === c2.key : c1 === c2;
  }

  getAttachmentId(ev: { uploadUrl: string }) {
    this.imageUrl = ev.uploadUrl;
  }

  save() {
    if (this.storeForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.storeForm.markAllAsTouched();
      return;
    }

    let form = this.storeForm.value;
    form.imageUrl = this.imageUrl;
    let params = form;
    this.isSaving = true;
    this.dataService
      .post(ServerApis.addOrUpdateAppService, params)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.router.navigate(['/admin/citizen/appService-list']);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      });
  }
}
