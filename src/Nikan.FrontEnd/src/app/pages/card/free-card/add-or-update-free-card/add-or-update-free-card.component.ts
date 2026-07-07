import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'card-add-or-update-free-card',
  templateUrl: './add-or-update-free-card.component.html',
  styleUrls: ['./add-or-update-free-card.component.scss'],
  standalone: false,
})
export class CardAddOrUpdateFreeCardComponent extends AppBase implements OnInit, AfterViewInit {
  isUpdate=false;
  id: string ='';
  storeForm: FormGroup;
  baseUrl = ServerApis.baseUrl;
  baseEnums: any = {};

  isSaving=false;

    loading?: boolean;

  cardTypeList: any[] = [];

  constructor() {
    super();
    this.route.params.subscribe((p) => {
      if (p['id'] && p['id'] != '0') {
        this.isUpdate = true;
        this.id = p['id'];
        this.getStoreInfo();
      } else {
        this.id = '';
        this.isUpdate = false;
      }
    });
    this.getBaseEnums();
    this.getGroups();
    this.getcenterList();

    this.storeForm = this.fb.group({
      discountTitle: [null, [Validators.required]],
      cardTypeId: [null, [Validators.required]],
      groupId: [null, [Validators.required]],
      deliverType: [null, [Validators.required]],
      centerID: [null],
      imagerReviewStatusFormFreeCard: [null, [Validators.required]],
      freeCardApplicantOrganization: [null, [Validators.required]],
      letterNumber: [null, [Validators.required]],
      description: [null],
      attachmentGroup: [null],
    });
  }

  ngOnInit(): void {}

  ngAfterViewInit() {}

  getBaseEnums() {
    this.dataService.getEnums().subscribe(
      (response) => {
        if (response) {
          this.baseEnums.imagerReviewStatusFormFreeCard = response.imagerReviewStatusFormFreeCard;
          this.baseEnums.cardDeliverType = response.cardDeliverType;

          debugger;
        }
      },
      (error) => {
        this.toastrService.error('خطا در ارتباط با سرور!');
      },
    );

    this.dataService.get(ServerApis.getActiveCardTypeBaseList, {}).subscribe((response) => {
      if (response.isSuccess) this.baseEnums.cardTypeList = response.data ? response.data : [];
    });
  }

  getGroups() {
    this.dataService.get(ServerApis.getFreeCardGroups).subscribe(
      (response) => {
        this.baseEnums.citizenGroups = response.data;
      },
      (error) => {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }
  getcenterList() {
    this.dataService.get(ServerApis.getAllCardDeliveryCenters).subscribe((response) => {
      if (response.isSuccess) this.baseEnums.centerList = response.data ? response.data : [];
      else {
        let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
        this.toastrService.error(msg);
      }
    });
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

            setTimeout(() => {}, 1000);
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error) => {
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

    this.isSaving = true;
    this.dataService.post(ServerApis.addRequestFreeCard, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.router.navigate(['/card/free-request-card-list']);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
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
