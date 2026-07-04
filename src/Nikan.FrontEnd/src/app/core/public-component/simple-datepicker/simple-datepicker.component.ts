import { Component, forwardRef, Input, OnInit, Output, Self } from '@angular/core';
import {
  ControlValueAccessor,
  Validator,
  FormControl,
  NgControl,
  AbstractControl,
  ValidationErrors,
  Validators,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
} from '@angular/forms';
import { RequireMatch } from '../../custom-validator/requireMatch';
import * as moment from 'jalali-moment';
import { JalaliMomentDateAdapter } from '../../jalali/jalali-moment-date-adapter';
import { EventEmitter } from '@angular/core';
import { AppBase } from "@app/app.base";

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'simple-jalali-datepicker',
  templateUrl: './simple-datepicker.component.html',
  styleUrls: ['./simple-datepicker.component.scss'],,
  // providers: [
  //   {
  //     provide: NG_VALUE_ACCESSOR,
  //     useExisting: forwardRef(() => SimpleJalaliDatepickerComponent),
  //     multi: true
  //   },
  //   {
  //     provide: NG_VALIDATORS,
  //     multi: true,
  //     useExisting: SimpleJalaliDatepickerComponent
  //   }
  // ]
    standalone: false
})
export class SimpleJalaliDatepickerComponent extends AppBase implements OnInit, ControlValueAccessor, Validator {
  @Input() required = false;
  @Input() disabled = false;
  @Input() appearance = 'outline';
  @Input() label = '';
  @Output() date = new EventEmitter<{ date: string; jDate: string }>();
  day = 0;
  month = 0;
  year = 0;
  dayList = [];
  monthList = [
    { id: 1, title: 'فروردین' },
    { id: 2, title: 'اردیبهشت' },
    { id: 3, title: 'خرداد' },
    { id: 4, title: 'تیر' },
    { id: 5, title: 'مرداد' },
    { id: 6, title: 'شهریور' },
    { id: 7, title: 'مهر' },
    { id: 8, title: 'آبان' },
    { id: 9, title: 'آذر' },
    { id: 10, title: 'دی' },
    { id: 11, title: 'بهمن' },
    { id: 12, title: 'اسفند' },
  ];
  yearList = [];
  isLeapYear = false;
  constructor(@Self() public ngControl: NgControl) {
      super();
    ngControl.valueAccessor = this;
    const momentLocaleData = moment.localeData('fa');

    for (let i = 1; i <= 29; i++) {
      this.dayList.push(i);
    }
    // this.monthList = momentLocaleData.jMonths();

    const date = new Date();
    const now = moment.jConvert.toJalali(date.getFullYear(), date.getMonth(), date.getDate());
    for (let i = now.jy; i >= now.jy - 99; i--) {
      this.yearList.push(i);
    }
  }

  // tslint:disable-next-line: variable-name
  private _onChange: (val: string) => void;
  // tslint:disable-next-line: variable-name
  private _onTouched: () => void;
  ngOnInit(): void {
    const validators = [this.validate];
    if (this.required) {
      validators.push(Validators.required);
    }
    this.ngControl.control.setValidators(validators);
    this.ngControl.control.updateValueAndValidity();
  }

  validate(control: AbstractControl): ValidationErrors {
    const valueObj = control.value;
    if (valueObj && !moment(valueObj).clone().locale('fa').isValid()) {
      return { invalid: true };
    } else {
      return null;
    }
  }
  registerOnValidatorChange?(fn: () => void): void {}
  writeValue(obj: any): void {
    if (obj) {
      let jDate: any = moment.from(obj, 'en').format('jYYYY/jMM/jDD');
      jDate = jDate.split('/');
      this.day = +jDate[2];
      this.month = +jDate[1];
      this.year = +jDate[0];
    }
  }
  registerOnChange(fn: any): void {
    this._onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this._onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onChange(): void {
    this.isLeapYear = moment.jIsLeapYear(this.year);
    if (this.year && this.month && this.day) {
      const shamsiDate = this.year + '/' + this.month + '/' + this.day;
      const miladyDate = moment.from(shamsiDate, 'fa', 'jYYYY/jMM/jDD').format('YYYY/MM/DD');
      if (this.isValid(miladyDate)) {
        this.ngControl.control.setValue(miladyDate);
        this.date.emit({
          date: miladyDate,
          jDate: shamsiDate,
        });
      } else {
        this.date.emit(null);
      }
    }
  }

  isValid(date: any): boolean {
    if (date === 'Invalid date') {
      return false;
    }
    return moment(date).clone().locale('fa').isValid();
  }
}
