import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '@core/server-apis';
import { HelperService } from '@core/services/helper.service';
import { MatStepper } from '@angular/material/stepper';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

interface ICard {
  attachmentGroup: string;
  buyCardDescription: string;
  cardCost: number;
  cardInfoId: number;
  cardIsActive: boolean;
  cardType: number;
  cardTypeId: number;
  creationDate: string;
  doubleCardCost: number;
  expirationDate: string;
  operation: string;
  operationId: number;
  postalCostInCity: number;
  postalCostOutCity: number;
  startFromDate: string;
  vatForCardCost: number;
  vatForPost: number;
}

interface ICardDetails {
  attachmentGroup: string;
  buyCardDescription: string;
  cardCost: number;
  cardInfoid: string;
  cardIsActive: boolean;
  cardType: string;
  cardTypeId: number;
  creationDate: string;
  doubleCardCost: number;
  expirationDate: string;
  operation: string;
  operationId: number;
  postalCostInCity: number;
  postalCostOutCity: number;
  startFromDate: string;
  vatForCardCost: number;
  vatForPost: number;
}
@Component({
  selector: 'app-buy-card',
  templateUrl: './buy-card.component.html',
  styleUrls: ['./buy-card.component.scss'],
  standalone: false,
})
export class BuyCardDialogComponent extends AppBase implements OnInit {
  citizenId!: number;
  states: any[] = [];
  loading: boolean = true;
  card: ICard;

  isSaving = false;
  groupList: any[] = [];
  form: FormGroup;
  cardDetails?: ICardDetails;
  cities: any[] = [];
  addressForm: FormGroup = new FormGroup({
    phoneNumber: new FormControl(null, [Validators.required]),
    region: new FormControl(null),
    addressType: new FormControl(2),
    id: new FormControl(null),
    street: new FormControl(null, [Validators.required]),
    alley: new FormControl(null),
    plaque: new FormControl(null),
    stateId: new FormControl('400', [Validators.required]),
    cityId: new FormControl('403', [Validators.required]),
    postalCode: new FormControl(null, [Validators.required]),
  });

  homeAddress: any;
  workAddress: any;
  orderDetails: any;

  constructor(
    private matDialogRef: MatDialogRef<BuyCardDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private helperService: HelperService,
  ) {
    super();
    this.form = this.fb.group({
      feedbackDescription: [null, [Validators.required]],
      feedbackId: [null, [Validators.required]],
    });
    this.card = _data.card;
  }

  ngOnInit() {
    this.getCardDetails();
    this.getAddresses();

    this.addressForm.get('addressType')?.valueChanges.subscribe((value) => {
      if (value === 2) this.loadHomeAddress();
      else this.loadWorkAddress();
    });
  }

  getCardDetails() {
    this.dataService
      .get(ServerApis.getCardInfo, {
        cardInfoId: this.card.cardInfoId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          this.cardDetails = response.data ? response.data : [];
        },
        (error: any) => {
          this.loading = false;
        },
      );
  }

  saveAddress() {
    this.dataService
      .get(ServerApis.cardPriceInfo, {
        addressId: this.addressForm.get('id')?.value,
        cardInfoId: this.card.cardInfoId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          this.cardDetails = response.data ? response.data : [];
        },
        (error: any) => {
          this.loading = false;
        },
      );
  }
  saveInfo() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
      return;
    }

    var formValue = this.form.value;
    this.isSaving = true;
    this.dataService
      .post(ServerApis.addFeedbacke, {
        citizenId: +this.citizenId,
        feedbackId: formValue.feedbackId,
        feedbackDescription: formValue.feedbackDescription ? formValue.feedbackDescription : '',
      })
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
          this.matDialogRef.close(true);
        } else {
          var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
          this.toastrService.error(msg);
        }
      });
  }

  getAddresses() {
    this.dataService.get(ServerApis.getCitizenOfficeAddress).subscribe((data) => {
      this.workAddress = data.data;
    });

    this.dataService.get(ServerApis.getCitizenHomeAddress).subscribe((data) => {
      this.homeAddress = data.data;
      this.loadHomeAddress();
    });
  }
  loadHomeAddress() {
    if (this.homeAddress) {
      this.addressForm.patchValue({
        id: this.homeAddress.id,
        region: this.homeAddress.region,
        street: this.homeAddress.street,
        alley: this.homeAddress.alley,
        plaque: this.homeAddress.plaque,
        postalCode: this.homeAddress.postalCode,
      });
    } else
      this.addressForm.patchValue({
        id: null,
        region: null,
        street: null,
        alley: null,
        plaque: null,
        postalCode: null,
      });
    this.chdr.detectChanges();
  }
  loadWorkAddress() {
    this.workAddress;
    if (this.workAddress) {
      this.addressForm.patchValue({
        id: this.workAddress.id,
        region: this.workAddress.region,
        street: this.workAddress.street,
        alley: this.workAddress.alley,
        plaque: this.workAddress.plaque,
        postalCode: this.workAddress.postalCode,
      });
    } else
      this.addressForm.patchValue({
        id: null,
        region: null,
        street: null,
        alley: null,
        plaque: null,
        postalCode: null,
      });
    this.chdr.detectChanges();
  }

  submitAddress(stepper: MatStepper) {
    const form = this.addressForm.getRawValue();

    this.isSaving = true;
    return this.dataService
      .post(ServerApis.addOrUpdteCitizenAddressByCitizen, {
        ...form,
      })
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response && response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
          this.dataService
            .post(ServerApis.cardPriceInfo, {
              addressId: response.data.id,
              cardIfoId: this.card.cardInfoId,
            })
            .pipe(
              finalize(() => {
                this.isSaving = false;
                this.chdr.detectChanges();
              }),
            )
            .subscribe((response) => {
              this.orderDetails = response.data;
            });
          stepper.next();
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      });
  }
}
