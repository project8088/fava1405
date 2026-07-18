import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { Observable, finalize } from 'rxjs';
import { map, startWith, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import {
  MatAutocompleteSelectedEvent,
  MatAutocompleteTrigger,
} from '@angular/material/autocomplete';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'card-new-export-card-dialog',
  templateUrl: './new-export-card.component.html',
  styleUrls: ['./new-export-card.component.scss'],
  standalone: false,
})
export class CardNewExportCardDialogComponent extends AppBase implements OnInit {
  isSaving = false;
  frm: FormGroup;
  isUpdate = false;

  loadingServices: boolean = false;
  filteredServices = new Observable<any[]>();
  selectedGroups: any[] = [];

  loadingData?: boolean;
  id: string = '';
  constructor(
    private matDialogRef: MatDialogRef<CardNewExportCardDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.frm = this.fb.group({
      startDate: [null],
      endDate: [null],
      nationality: [null],
      cardTypeId: [null, [Validators.required]],
      inTheCity: [null],
      deliverType: [null],
      groupIds: [null],
    });
  }

  ngOnInit() {
    this.filteredServices = this.frm.get('groupIds')!.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((value) => {
        return this._filterServices(value);
      }),
    );
  }

  saveInfo() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return;
    }

    this.isSaving = true;

    var url = ServerApis.newExportCard;

    var params = this.frm.value;
    if (params.startDate) params.startDate = this.dataService.formatDate(params.startDate);

    if (params.endDate) params.endDate = this.dataService.formatDate(params.endDate);

    params.commercialRatio = +params.commercialRatio;
    params.dailyChargeAmount = +params.dailyChargeAmount;
    params.penaltyPercentage = +params.penaltyPercentage;
    params.tax = +params.tax;
    params.title = params.title;
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
      .subscribe((response) => {
        if (response && response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
          this.matDialogRef.close(true);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      });
  }

  /**
   * جستجوی مهارت ها
   * @param value
   */
  private _filterServices(value: string) {
    if (!value || typeof value !== 'string') return this.filteredServices;

    const filterValue = value.toLowerCase();

    this.loadingServices = true;
    return this.dataService
      .get(ServerApis.searchBaseDataGroups, {
        query: filterValue,
        offset: 0,
        count: 20,
      })
      .pipe(
        map(
          (response) => {
            this.loadingServices = false;
            if (response.isSuccess) return response.data;
            else {
              let msg = response.messages
                ? response.messages
                : 'در یافت اطلاعات از سرور با خطا مواجه شده است.';
              this.toastrService.error(msg);
            }
          },
          (error: any) => {
            this.toastrService.error('خطا در ارتباط با سرور!');
            this.loadingServices = false;
          },
        ),
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
  displayFn(item: any): string {
    return item && item.text ? item.text : '';
  }
}
