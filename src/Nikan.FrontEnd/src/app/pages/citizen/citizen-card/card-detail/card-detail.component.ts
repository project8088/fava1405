import { ActivatedRoute, Router } from '@angular/router';
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

import { DataService } from '../../../../core/services/data-service.service';
import { HelperService } from 'src/app/core/services/helper.service';
import { MatStepper } from '@angular/material/stepper';
import { ServerApis } from '../../../../core/server-apis';
import { ToastrService } from 'ngx-toastr';

interface ICard {
  attachmentGroup: string;
  buyCardDescription: string;
  cardCost: number;
  cardInfoId: string;
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
  cardInfoId: string;
  cardTypeId: number;
  cardType: string;
  buyCardDescription: string;
  creationDate: string;
  attachmentGroup: string;

  cardCost: number;
  doubleCardCost: number;
  vatForCardCost: number;
  vatForPost: number;
  //تخفیفات
  postageDiscountAmount: number;
  cardDiscountAmount: number;
  totalDiscountAmount: number;

  expirationDate: string;
  operation: string;
  operationId: number;
  postalCostInCity: number;
  postalCostOutCity: number;
  startFromDate: string;

  cardIsActive: boolean;
}
@Component({
  selector: 'app-card-detail',
  templateUrl: './card-detail.component.html',
  styleUrls: ['./card-detail.component.scss'],
})
export class CardDetailComponent implements OnInit {
  citizenId: number;
  addressId: number;

  states;
  loading: boolean = true;
  cardInfoId: ICard;
  RefId: string;
  waitForRedirectToBank: boolean;
  isSaving: boolean;
  groupList: any[] = [];
  form: FormGroup;
  cardDetails: ICardDetails;
  cities;
  addressForm: FormGroup = new FormGroup({
    phone: new FormControl(null, [Validators.required]),
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

  homeAddress;
  workAddress;
  orderDetails;

  constructor(
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private dataService: DataService,
    private router: Router,
    private route: ActivatedRoute,
    private helperService: HelperService,
  ) {
    this.route.queryParams.subscribe((params) => {
      if (params['id']) {
        this.cardInfoId = params['id'];
      } else this.router.navigate(['/citizen-card']);
    });
  }

  ngOnInit() {
    this.getCardDetails();
    this.getAddresses();

    this.addressForm.get('addressType').valueChanges.subscribe((value) => {
      if (value === 2) this.loadHomeAddress();
      else this.loadWorkAddress();
    });
  }
  //getCardInfo
  getCardDetails() {
    this.dataService
      .get(ServerApis.getCitizenCardPriceInfo, {
        cardInfoId: this.cardInfoId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          this.cardDetails = response.data ? response.data : [];
        },
        (error) => {
          this.loading = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
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
    this.homeAddress;
    if (this.homeAddress) {
      this.addressForm.patchValue({
        id: this.homeAddress.id,
        phone: this.homeAddress.phone,
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
        phone: null,
        plaque: null,
        postalCode: null,
      });
  }
  loadWorkAddress() {
    this.workAddress;
    if (this.workAddress) {
      this.addressForm.patchValue({
        id: this.workAddress.id,
        region: this.workAddress.region,
        phone: this.homeAddress.phone,
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
        phone: null,
        alley: null,
        plaque: null,
        postalCode: null,
      });
  }

  submitAddress(stepper: MatStepper) {
    const form = this.addressForm.getRawValue();
    debugger;
    this.isSaving = true;
    return this.dataService
      .post(ServerApis.addOrUpdteCitizenAddressByCitizenForCardAddress, {
        ...form,
      })
      .subscribe(
        (response) => {
          if (response && response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
            this.addressId = response.data.id;
            this.dataService
              .post(ServerApis.cardPriceInfo, {
                addressId: response.data.id,
                CardInfoId: this.cardInfoId,
              })
              .subscribe((response) => {
                this.orderDetails = response.data;
              });
            stepper.next();
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
          this.isSaving = false;
        },
        (error) => {
          this.isSaving = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
  }

  ordercard() {
    this.loading = true;
    this.dataService
      .post(ServerApis.buyCardByCitizens, {
        CardInfoId: this.cardInfoId,
        DeliveringAddressId: +this.addressId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess) {
            if (response.data.isfree) {
              this.toastrService.success(' ثبت درخواست کارت شما با موفقیت ثبت گردید.');
              setTimeout(() => {
                this.router.navigate(['/citizen/citizen-card']);
              }, 1000);
            } else {
              this.RefId = response.data.refId;
              console.log(document.getElementById('payformmellatbank'));
              var form: any = document.getElementById('payformmellatbank');
              this.waitForRedirectToBank = true;
              setTimeout(() => {
                form.submit();
              }, 1000);
            }
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loading = false;
        },
      );
  }
}
