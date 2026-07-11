import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-add-or-update-card',
  templateUrl: './add-or-update-card.component.html',
  styleUrls: ['./add-or-update-card.component.scss'],
  standalone: false,
})
export class AdminAddOrUpdateCardComponent extends AppBase implements OnInit, AfterViewInit {
  isUpdate=false;
  id: string ='';
  storeForm: FormGroup;
  baseUrl = ServerApis.baseUrl;

  isSaving=false;

    loading?: boolean;
  cardTypeId: string ='';
  cardTypeList: any[] = [];

  constructor() {
    super();
    this.route.params.subscribe((p) => {
      this.cardTypeId = p['cardTypeId'];

      if (p['id'] && p['id'] != '0') {
        this.isUpdate = true;
        this.id = p['id'];
        this.getStoreInfo();
      } else {
        this.id = '';
        this.isUpdate = false;
      }
    });

    this.storeForm = this.fb.group({
      cardInfoId: [null],
      cardTypeId: [null],
      doubleCardCost: [null, [Validators.required]],
      cardCost: [null, [Validators.required]],
      postalCostInCity: [null, [Validators.required]],
      buyCardDescription: [null],
      vatForCardCost: [0, [Validators.required, Validators.max(100), Validators.min(0)]],
      vatForPost: [0, [Validators.required, Validators.max(100), Validators.min(0)]],
      cardIsActive: [true, []],
    });
  }

  ngOnInit(): void {
    this.dataService.get(ServerApis.getCardTypeBaseList, {}).subscribe((response) => {
      if (response.isSuccess) this.cardTypeList = response.data ? response.data : [];
    });
  }

  ngAfterViewInit() {
  }

  getStoreInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getCardInfo, {
        cardInfoId: this.id,
        forEdit: true,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            response.data.cardTypeId = response.data.cardTypeId.toString();
            this.storeForm.patchValue(response.data);

          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error:any) => {
          this.loading = false;
        },
      );
  }

  save() {
    if (this.storeForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.storeForm.markAllAsTouched();
        return ;
    }

    let form = this.storeForm.value;

    let params = form;
    params.cardTypeId = +this.cardTypeId;
    this.isSaving = true;
    this.dataService.post(ServerApis.addOrUpdateCard, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.router.navigate(['/admin/card-list']);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error:any) => {
        this.isSaving = false;
      },
    );
  }
  /**
   * for bind object in select
   * @param item
   */
  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? +c1.key === c2.key : c1 === c2;
  }

}
