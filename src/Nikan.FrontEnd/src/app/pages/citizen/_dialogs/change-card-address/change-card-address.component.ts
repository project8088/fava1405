import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { ServerApis } from '../../../../core/server-apis';
import { HelperService } from '../../../../core/services/helper.service';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-change-card-address-dialog',
  templateUrl: './change-card-address.component.html',
  styleUrls: ['./change-card-address.component.scss'],
    standalone: false
})
export class ChangeCardAddressDialogComponent extends AppBase implements OnInit {
  isSaving: boolean;
  id: string;
  isfahanCities: any[];
  homeForm: FormGroup;
  loading: boolean = true;
  info: any;
  constructor(
    private matDialogRef: MatDialogRef<ChangeCardAddressDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private helperService: HelperService,
    private customValidator: CustomFormValidators
  ) {
      super();
    this.info = _data.info;
    debugger;

    if (_data.info.id && _data.info.id != '0') {
      this.id = _data.info.id;
      this.loadAddress();
    }

    this.helperService.getIsfahanCities().subscribe((data) => {
      this.isfahanCities = data;
    });
    this.homeForm = this.fb.group({
      phone: ['', [Validators.required]],
      region: [null],
      id: [null],
      street: [null, [Validators.required]],
      alley: [null],
      plaque: [null, [Validators.required]],
      cityId: [null, [Validators.required]],
      postalCode: [null, [Validators.required]],
    });
  }

  ngOnInit() {}

  saveInfo() {
    if (this.homeForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.homeForm.markAllAsTouched();
      return false;
    }

    this.isSaving = true;
    var url = ServerApis.updateCitizenCardAddressByCitizen;
    var params = this.homeForm.value;
    params.cardId = this.info.id;
    params.phone = params.phone;
    params.region = params.region;
    params.street = params.street;
    params.alley = params.alley;
    params.plaque = params.plaque;

    params.cityId = +params.cityId;
    params.postalCode = params.postalCode;

    this.dataService.post(url, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response && response.isSuccess) {
          this.toastrService.success('آدرس جدید تحویل کارت با موفقیت ثبت شد');
          this.matDialogRef.close(true);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.isSaving = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  loadAddress() {
    this.dataService
      .get(ServerApis.getCitizenAddress, { id: this.info.deliveringAddressId })
      .subscribe((data) => {
        if (data.data) {
          this.homeForm.patchValue({
            id: data.data.id,
            region: data.data.region,
            phone: data.data.phone,
            street: data.data.street,
            alley: data.data.alley,
            plaque: data.data.plaque,
            cityId: String(data.data.cityId),
            postalCode: data.data.postalCode,
          });
        }
      });
  }
}
