import { Component, OnInit, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { ControlValueAccessor, Validators, FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { RequireMatch } from '../../custom-validator/requireMatch';
import { Observable, finalize } from 'rxjs';
import { ServerApis } from '../../server-apis';
import { startWith, map } from 'rxjs/operators';
import { MatAutocompleteTrigger, MatAutocomplete } from '@angular/material/autocomplete';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'input-refund-users',
  templateUrl: './input-refund-users.component.html',
  styleUrls: ['./input-refund-users.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputRefundUsersAutoCompleteComponent),
      multi: true,
    },
  ],
  standalone: false,
})
export class InputRefundUsersAutoCompleteComponent
  extends AppBase
  implements ControlValueAccessor, OnInit
{
  @Input() disabled: boolean = false;

  @Input('label') label: string = 'مسئول استرداد';
  @Input('required') required: boolean = false;

  List: any[] = [];

  @Output('optionSelected') optionSelected = new EventEmitter<any>();

  loading?: boolean;
  filteredList = new Observable<any[]>();
  myControl = new FormControl();

  constructor() {
    super();
  }

  ngOnInit() {
    this.getCompanyList();

    if (this.myControl) {
      if (this.required) {
        this.myControl.setValidators([Validators.required, RequireMatch]);
        this.myControl.updateValueAndValidity();
      }
    }
  }

  private propagateChange = (_: any) => {};

  writeValue(value: any): void {
    //if (value !== undefined) {
    //  this.myControl.setValue(value.toString());
    //}
  }
  registerOnChange(fn: any): void {
    // this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    // this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  getCompanyList() {
    if (this.myControl) {
      this.myControl.reset('');
      this.List = [];
    }

    this.loading = true;
    this.dataService
      .get(ServerApis.getAllRefunAccessUsers, {})
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        this.List = response.data ? response.data : [];
        this.filteredList = this.myControl.valueChanges.pipe(
          startWith(''),
          map((value) => {
            if (value === null || value === undefined) return '';
            else if (typeof value === 'string') return value;
            else if (value.text) return value.text;
            else return '';
          }),
          map((value) => (value ? this._filter(value, this.List) : this.List)),
        );
      });
  }

  /**
   * فیلتر بر روی لیست ها برای اتوکامپلیت
   * @param name  عبارت جستجو
   * @param list   لیست
   */
  private _filter(name: string, list: any[]): any[] {
    const filterValue = name.toLowerCase();
    return list.filter((option) => option.text.toLowerCase().indexOf(filterValue) === 0);
  }

  clearItem(trigger: MatAutocompleteTrigger, auto: MatAutocomplete) {
    setTimeout(() => {
      auto.options.forEach((item) => {
        item.deselect();
      });
      this.myControl.reset('');
      trigger.openPanel();
    }, 100);
  }

  /**
   * for bind object in autocomplete
   * @param item
   */
  displayFn(item: any): string {
    return item && item.text ? item.text : '';
  }

  onChange() {
    if (this.optionSelected) this.optionSelected.emit(this.myControl.value);
  }
}
