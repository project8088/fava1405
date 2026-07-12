import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import {
  map,
  startWith,
  debounceTime,
  distinctUntilChanged,
  switchMap,
  finalize,
} from 'rxjs/operators';
import {
  MatAutocompleteSelectedEvent,
  MatAutocompleteTrigger,
} from '@angular/material/autocomplete';
import { AppBase } from '@app/app.base';
import { Observable } from 'rxjs';

@Component({
  selector: 'adm-add-card-discount-dialog',
  templateUrl: './add-card-discount.component.html',
  styleUrls: ['./add-card-discount.component.scss'],
  standalone: false,
})
export class CardAddCardDiscountDialogComponent extends AppBase implements OnInit {
  isSaving = false;
  frm: FormGroup;
  isUpdate = false;

  loadingGroups: boolean = false;
  filteredGroups = new Observable<any[]>();
  selectedGroups: any[] = [];

  loadingData?: boolean;
  id: string = '';
  cardTypeId: string = '';
  constructor(
    private matDialogRef: MatDialogRef<CardAddCardDiscountDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.frm = this.fb.group({
      cardTypeId: [null],
      discountPercent: [null, [Validators.required]],
      postalPercentInCity: [null, [Validators.required]],
      discountTitle: ['', [Validators.required]],
      startDate: [null],
      endDate: [null],

      penaltyPercentage: [0, []],
      discountIsActive: [true, []],
      postDeliveryPossibility: [true, []],
      centerDeliveryPossibility: [true, []],
      description: [''],
      groups: [null],
    });

    if (_data.id) {
      this.isUpdate = true;
      this.id = _data.id;
      this.getInfo();
    } else {
      this.isUpdate = false;
    }

    this.cardTypeId = _data.cardTypeId;
  }

  ngOnInit() {
    this.filteredGroups = this.frm.get('groups')!.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((value) => {
        return this._filterGroups(value);
      }),
    );
  }

  getInfo() {
    this.loadingData = true;
    this.dataService
      .get(ServerApis.getCardDiscountInfo, { id: this.id })
      .pipe(
        finalize(() => {
          this.loadingData = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response && response.isSuccess) {
            this.frm.patchValue(response.data);
            this.selectedGroups = response.data.groups ? response.data.groups : [];
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
            this.matDialogRef.close();
          }
        },
        (error: any) => {},
      );
  }

  saveInfo() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    var params = this.frm.value;

    var url = ServerApis.addCardDiscount;
    if (this.isUpdate) {
      url = ServerApis.updateCardDiscount;
      params.id = +this.id;
    }

    if (params.startDate) params.startDate = this.dataService.formatDate(params.startDate);

    if (params.endDate) params.endDate = this.dataService.formatDate(params.endDate);

    params.cardTypeId = +this.cardTypeId;
    params.discountTitle = params.discountTitle;

    params.discountIsActive = params.discountIsActive;
    params.postDeliveryPossibility = params.postDeliveryPossibility;
    params.centerDeliveryPossibility = params.centerDeliveryPossibility;
    params.description = params.description;
    params.postalPercentInCity = +params.postalPercentInCity;
    params.discountPercent = +params.discountPercent;

    var groupIds = [];
    for (let item of this.selectedGroups) groupIds.push(+item.key);

    params.groupIds = groupIds;

    this.dataService
      .post(url, this.frm.value)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response && response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
            this.matDialogRef.close(true);
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
  }

  changePenaltyForPeriodDebt() {
    if (this.frm.get('penaltyForPeriodDebt')?.value) {
      this.frm.get('penaltyPercentage')?.setValidators([Validators.required]);
      this.frm.get('penaltyPercentage')?.updateValueAndValidity();
    } else {
      this.frm.get('penaltyForPeriodDebt')?.setValue(false);
      this.frm.get('penaltyPercentage')?.setValue(0);
      this.frm.get('penaltyPercentage')?.clearValidators();
      this.frm.get('penaltyPercentage')?.updateValueAndValidity();
    }
  }

  private _filterGroups(value: string): Observable<any> {
    if (!value || typeof value !== 'string') return this.filteredGroups;

    const filterValue = value.toLowerCase();

    this.loadingGroups = true;
    return this.dataService
      .get(ServerApis.searchBaseDataGroups, {
        query: filterValue,
        offset: 0,
        count: 20,
      })
      .pipe(
        finalize(() => (this.loadingGroups = false)),
        map((response) => {
          this.loadingGroups = false;
          if (response.isSuccess) return response.data;
          else {
            let msg = response.messages
              ? response.messages
              : 'در یافت اطلاعات از سرور با خطا مواجه شده است.';
            this.toastrService.error(msg);
            return [];
          }
        }),
      );
  }

  /**
   * حذف از اتوکاملیت چیپ
   * @param fruit
   */
  removeAutoChip(list: any[], item: any): void {
    const index = list.indexOf(item);

    if (index >= 0) {
      list.splice(index, 1);
    }
  }
  /**
   * انتخاب اتوکاملیت و اضافه کردن به لیست چیپ
   * @param list
   * @param formControl
   * @param input
   * @param event
   * @param Trigger
   */
  selectedAutoChip(
    list: any[],
    formControl: string,
    input: any,
    event: MatAutocompleteSelectedEvent,
    Trigger: MatAutocompleteTrigger,
  ): void {
    const index = list.findIndex((l) => l.key == event.option.value.key);
    if (index >= 0)
      this.toastrService.warning(event.option.value.text + ' را قبلاً انتخاب کرده اید.', 'تکراری!');
    else list.push(event.option.value);
    input.value = '';
    this.frm.get(formControl)?.setValue(null);
    setTimeout(() => {
      Trigger.openPanel();
    }, 100);
  }
}
