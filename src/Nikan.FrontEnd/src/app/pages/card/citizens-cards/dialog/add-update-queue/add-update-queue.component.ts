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
  selector: 'adm-add-update-queue-dialog',
  templateUrl: './add-update-queue.component.html',
  styleUrls: ['./add-update-queue.component.scss'],
  standalone: false,
})
export class CardAddOrUpadateQueueDialogComponent extends AppBase implements OnInit {
  isSaving = false;
  frm: FormGroup;
  isUpdate = false;

  courseId: string = '';
  loadingServices: boolean = false;
  filteredServices = new Observable<any[]>();
  selectedServices: any[] = [];

  loadingData?: boolean;
  id: string = '';
  constructor(
    private matDialogRef: MatDialogRef<CardAddOrUpadateQueueDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.frm = this.fb.group({
      id: [null],
      name: ['', [Validators.required]],
      cardTypeId: ['', [Validators.required]],
      indexOrder: [null, [Validators.required]],
      defaultColor: [null, [Validators.required]],
      description: [''],
      groupIds: [null],
      isLock: [false, []],
      groups: [null],
    });

    if (_data.item) {
      this.isUpdate = true;
      var item = Object.assign({}, _data.item);
      this.id = _data.item.id;
      this.getInfo();
    } else {
      this.isUpdate = false;
    }

    this.courseId = _data.courseId;
  }

  ngOnInit() {
    this.filteredServices = this.frm.get('groups')!.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((value) => {
        return this._filterServices(value);
      }),
    );
  }

  getInfo() {
    this.loadingData = true;
    this.dataService
      .get(ServerApis.getDistributionQueueInfo, { id: this.id })
      .pipe(
        finalize(() => {
          this.loadingData = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response && response.isSuccess) {
          this.frm.patchValue(response.data);
          this.selectedServices = response.data.groups ? response.data.groups : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
          this.matDialogRef.close();
        }
      });
  }

  saveInfo() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return;
    }

    this.isSaving = true;

    var url = ServerApis.addCardDistributionQueue;
    if (this.isUpdate) url = ServerApis.updateCardDistributionQueue;

    var params = this.frm.value;

    params.cardTypeId = +params.cardTypeId;
    params.indexOrder = +params.indexOrder;
    params.name = params.name;
    params.defaultColor = params.defaultColor;
    params.description = params.description;
    params.iSLock = params.iSLock;
    params.isActive = params.isActive;

    params.coursesId = +this.courseId;

    var serviceIds = [];
    for (let item of this.selectedServices) serviceIds.push(+item.key);

    params.groupIds = serviceIds;

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
