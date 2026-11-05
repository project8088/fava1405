import {
  Component,
  Input,
  OnInit,
  OnChanges,
  SimpleChanges,
  Output,
  EventEmitter,
  forwardRef,
} from '@angular/core';

import {
  ControlValueAccessor,
  Validator,
  AbstractControl,
  ValidationErrors,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
} from '@angular/forms';

import { DateTime } from 'luxon';
import { AppBase } from '@app/app.base';

// =============================================================
// Persian Calendar Helpers
// =============================================================

interface PersianDate {
  jy: number;
  jm: number;
  jd: number;
}

const PERSIAN_TIME_ZONE = 'UTC';

const persianFormatter = new Intl.DateTimeFormat('en', {
  calendar: 'persian',
  year: 'numeric',
  month: '2-digit',
  day: '2-digit',
  numberingSystem: 'latn',
  timeZone: PERSIAN_TIME_ZONE,
});

/**
 * Extract Persian date parts from a Gregorian date.
 */
function extractPersianParts(date: DateTime | Date): PersianDate {
  const jsDate = date instanceof DateTime ? date.toUTC().toJSDate() : date;

  const parts = persianFormatter.formatToParts(jsDate);

  const get = (type: string): number => {
    const value = parts.find((p) => p.type === type)?.value;
    return Number(value || 0);
  };

  return {
    jy: get('year'),
    jm: get('month'),
    jd: get('day'),
  };
}

/**
 * Compare two Persian dates.
 */
function comparePersianDates(a: PersianDate, b: PersianDate): number {
  if (a.jy !== b.jy) return a.jy - b.jy;
  if (a.jm !== b.jm) return a.jm - b.jm;
  return a.jd - b.jd;
}

/**
 * Find an exact Persian date using binary search.
 *
 * Persian new year occurs around March 20-22.
 * This function does not depend on month-length
 * calculation, so recursive calls cannot occur.
 */
function findPersianDate(jy: number, jm: number, jd: number): DateTime {
  if (
    !Number.isInteger(jy) ||
    !Number.isInteger(jm) ||
    !Number.isInteger(jd) ||
    jm < 1 ||
    jm > 12 ||
    jd < 1 ||
    jd > 31
  ) {
    return DateTime.invalid('Invalid Persian date');
  }

  const target: PersianDate = { jy, jm, jd };

  let low = DateTime.fromObject(
    {
      year: jy + 621,
      month: 3,
      day: 19,
    },
    { zone: 'utc' },
  ).startOf('day');

  let high = DateTime.fromObject(
    {
      year: jy + 622,
      month: 3,
      day: 23,
    },
    { zone: 'utc' },
  ).startOf('day');

  if (!low.isValid || !high.isValid) {
    return DateTime.invalid('Invalid Gregorian range');
  }

  while (low.toMillis() <= high.toMillis()) {
    const lowDay = low.toMillis() / 86400000;
    const highDay = high.toMillis() / 86400000;

    const midDay = Math.floor((lowDay + highDay) / 2);

    const mid = DateTime.fromMillis(midDay * 86400000, { zone: 'utc' });

    const comparison = comparePersianDates(extractPersianParts(mid), target);

    if (comparison === 0) {
      return mid;
    }

    if (comparison < 0) {
      low = mid.plus({ days: 1 });
    } else {
      high = mid.minus({ days: 1 });
    }
  }

  return DateTime.invalid('Persian date does not exist');
}

/**
 * Check whether a Persian year is leap.
 *
 * Difference between two consecutive Persian year
 * starting dates must be 366 days.
 */
function isLeapJalaaliYear(jy: number): boolean {
  const currentYearStart = findPersianDate(jy, 1, 1);
  const nextYearStart = findPersianDate(jy + 1, 1, 1);

  if (!currentYearStart.isValid || !nextYearStart.isValid) {
    return false;
  }

  return nextYearStart.diff(currentYearStart, 'days').days === 366;
}

/**
 * Get maximum day of a Persian month.
 */
function getMaxDayInJalaliMonth(jy: number, jm: number): number {
  if (jm < 1 || jm > 12) return 31;

  if (jm <= 6) return 31;

  if (jm <= 11) return 30;

  return isLeapJalaaliYear(jy) ? 30 : 29;
}

/**
 * Convert Persian date to Gregorian DateTime.
 */
function jalaaliToGregorian(jy: number, jm: number, jd: number): DateTime {
  if (
    !Number.isInteger(jy) ||
    !Number.isInteger(jm) ||
    !Number.isInteger(jd) ||
    jm < 1 ||
    jm > 12
  ) {
    return DateTime.invalid('Invalid Persian date');
  }

  const maxDay = getMaxDayInJalaliMonth(jy, jm);

  if (jd < 1 || jd > maxDay) {
    return DateTime.invalid('Invalid Persian day');
  }

  return findPersianDate(jy, jm, jd);
}

/**
 * Convert Gregorian DateTime to Persian date.
 */
function gregorianToJalaali(dt: DateTime): PersianDate {
  return extractPersianParts(dt);
}

/**
 * Validate a Gregorian date string.
 */
function isValidGregorianString(value: unknown): boolean {
  if (typeof value !== 'string') return false;

  const match = /^(\d{4})\/(\d{2})\/(\d{2})$/.exec(value);

  if (!match) return false;

  const [, year, month, day] = match;

  const dt = DateTime.fromObject(
    {
      year: Number(year),
      month: Number(month),
      day: Number(day),
    },
    { zone: 'utc' },
  );

  return dt.isValid && dt.toFormat('yyyy/MM/dd') === value;
}

// =============================================================
// Component
// =============================================================

@Component({
  selector: 'simple-jalali-datepicker',
  templateUrl: './simple-datepicker.component.html',
  styleUrls: ['./simple-datepicker.component.scss'],

  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SimpleJalaliDatepickerComponent),
      multi: true,
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => SimpleJalaliDatepickerComponent),
      multi: true,
    },
  ],

  standalone: false,
})
export class SimpleJalaliDatepickerComponent
  extends AppBase
  implements OnInit, OnChanges, ControlValueAccessor, Validator
{
  @Input() required = false;
  @Input() disabled = false;
  @Input() label = '';

  @Output()
  date = new EventEmitter<{
    date: string;
    jDate: string;
  } | null>();

  day = 0;
  month = 0;
  year = 0;

  dayList: number[] = [];

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

  yearList: number[] = [];

  isLeapYear = false;
  isTouched = false;
  hasInvalidDate = false;

  private _onChange: (value: string | null) => void = () => {};
  private _onTouched: () => void = () => {};
  private _onValidatorChange: () => void = () => {};

  constructor() {
    super();

    this.updateDayList();
  }

  ngOnInit(): void {
    this.initializeYears();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['required']) {
      this._onValidatorChange();
    }

    if (changes['disabled']) {
      // The disabled state is also controlled by CVA.
    }
  }

  private initializeYears(): void {
    const today = DateTime.now().setZone('Asia/Tehran').startOf('day').toUTC();

    const currentYear = gregorianToJalaali(today).jy;

    this.yearList = Array.from({ length: 100 }, (_, index) => currentYear - index);
  }

  // ===========================================================
  // Validator
  // ===========================================================

  validate(control: AbstractControl): ValidationErrors | null {
    const value = control.value;

    if (value === null || value === undefined || value === '') {
      return this.required ? { required: true } : null;
    }

    if (!isValidGregorianString(value)) {
      return { invalid: true };
    }

    return null;
  }

  registerOnValidatorChange(fn: () => void): void {
    this._onValidatorChange = fn;
  }

  // ===========================================================
  // ControlValueAccessor
  // ===========================================================

  writeValue(value: string | null): void {
    if (!value) {
      this.clearSelection();
      return;
    }

    if (!isValidGregorianString(value)) {
      this.clearSelection();
      this.hasInvalidDate = true;
      return;
    }

    const dt = DateTime.fromFormat(value, 'yyyy/MM/dd', { zone: 'utc' });

    if (!dt.isValid) {
      this.clearSelection();
      this.hasInvalidDate = true;
      return;
    }

    const jDate = gregorianToJalaali(dt);

    this.year = jDate.jy;
    this.month = jDate.jm;
    this.day = jDate.jd;

    this.isLeapYear = isLeapJalaaliYear(this.year);

    this.updateDayList();

    this.hasInvalidDate = false;
  }

  registerOnChange(fn: (value: string | null) => void): void {
    this._onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this._onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  // ===========================================================
  // User interaction
  // ===========================================================

  onChange(): void {
    if (this.disabled) return;

    this.isTouched = true;
    this._onTouched();

    this.isLeapYear = this.year ? isLeapJalaaliYear(this.year) : false;

    this.updateDayList();

    if (!this.year || !this.month || !this.day) {
      this.emitEmpty();
      return;
    }

    const gDate = jalaaliToGregorian(this.year, this.month, this.day);

    if (!gDate.isValid) {
      this.emitEmpty();
      return;
    }

    const miladyDate = gDate.toFormat('yyyy/MM/dd');

    const shamsiDate = this.formatJalaaliDate(this.year, this.month, this.day);

    this.hasInvalidDate = false;

    this._onChange(miladyDate);

    this.date.emit({
      date: miladyDate,
      jDate: shamsiDate,
    });

    this._onValidatorChange();
  }

  onTouched(): void {
    this.isTouched = true;
    this._onTouched();
  }

  private emitEmpty(): void {
    this._onChange(null);
    this.date.emit(null);
    this._onValidatorChange();
  }

  private clearSelection(): void {
    this.day = 0;
    this.month = 0;
    this.year = 0;

    this.isLeapYear = false;

    this.updateDayList();

    this.hasInvalidDate = false;
  }

  // ===========================================================
  // Helpers
  // ===========================================================

  private updateDayList(): void {
    const maxDay = this.month ? getMaxDayInJalaliMonth(this.year, this.month) : 31;

    this.dayList = Array.from({ length: maxDay }, (_, index) => index + 1);

    if (this.day > maxDay) {
      this.day = 0;
    }
  }

  get showRequiredError(): boolean {
    return this.required && this.isTouched && (!this.year || !this.month || !this.day);
  }

  get showInvalidError(): boolean {
    return this.hasInvalidDate;
  }

  private formatJalaaliDate(year: number, month: number, day: number): string {
    const pad = (value: number) => String(value).padStart(2, '0');

    return `${year}/${pad(month)}/${pad(day)}`;
  }

  /**
   * Format Gregorian date using Persian calendar.
   */
  formatPersian(gDate: DateTime, format = 'yyyy/MM/dd'): string {
    return gDate
      .setZone('Asia/Tehran')
      .setLocale('fa-IR')
      .reconfigure({ outputCalendar: 'persian' })
      .toFormat(format);
  }
}
