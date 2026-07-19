import { Component, Input, OnInit, Output, EventEmitter, forwardRef } from '@angular/core';
import {
  ControlValueAccessor,
  Validator,
  AbstractControl,
  ValidationErrors,
  Validators,
  FormControl,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
} from '@angular/forms';
import { DateTime } from 'luxon';
import { AppBase } from '@app/app.base';

// =============================================================
// Persian (Jalaali) calendar helpers
// Luxon's outputCalendar only affects display — not parsing.
// We use Intl.DateTimeFormat + binary search for conversion.
// =============================================================

const persianFormatter = new Intl.DateTimeFormat('en', {
  calendar: 'persian',
  year: 'numeric',
  month: '2-digit',
  day: '2-digit',
  numberingSystem: 'latn',
});

interface PersianDate {
  jy: number;
  jm: number;
  jd: number;
}

/** استخراج اجزای شمسی از یک تاریخ میلادی (با کمک Intl) */
function extractPersianParts(date: DateTime | Date): PersianDate {
  const jsDate = date instanceof DateTime ? date.toJSDate() : date;
  const parts = persianFormatter.formatToParts(jsDate);
  const get = (type: string): number =>
    parseInt(parts.find((p) => p.type === type)?.value || '0', 10);
  return { jy: get('year'), jm: get('month'), jd: get('day') };
}

/** مقایسه دو تاریخ شمسی */
function comparePersianDates(a: PersianDate, b: PersianDate): number {
  if (a.jy !== b.jy) return a.jy - b.jy;
  if (a.jm !== b.jm) return a.jm - b.jm;
  return a.jd - b.jd;
}

/** تبدیل میلادی → شمسی */
function gregorianToJalaali(dt: DateTime): PersianDate {
  return extractPersianParts(dt);
}

/** تبدیل شمسی → میلادی با جستجوی دودویی (O(log n)) */
function jalaaliToGregorian(jy: number, jm: number, jd: number): DateTime {
  // اعتبارسنجی اولیه
  if (jm < 1 || jm > 12) return DateTime.invalid('Invalid Jalaali month');
  const maxDay = getMaxDayInJalaliMonth(jy, jm);
  if (jd < 1 || jd > maxDay) return DateTime.invalid('Invalid Jalaali day');

  // بازه تقریبی: شروع سال شمسی بین 20 مارس تا 22 مارس است
  let low = DateTime.fromObject({ year: jy + 620, month: 3, day: 20 }, { zone: 'utc' });
  let high = DateTime.fromObject({ year: jy + 622, month: 3, day: 22 }, { zone: 'utc' });

  // اگر بازه خیلی کوچک بود، گسترشش بده
  let guard = 0;
  while (comparePersianDates(extractPersianParts(low), { jy, jm, jd }) > 0 && guard++ < 10) {
    low = low.minus({ days: 1 });
  }
  guard = 0;
  while (comparePersianDates(extractPersianParts(high), { jy, jm, jd }) < 0 && guard++ < 10) {
    high = high.plus({ days: 1 });
  }

  // جستجوی دودویی
  while (low < high) {
    const diff = Math.floor(high.diff(low, 'days').days);
    if (diff === 0) break;
    const mid = low.plus({ days: diff });
    if (comparePersianDates(extractPersianParts(mid), { jy, jm, jd }) < 0) {
      low = mid.plus({ days: 1 });
    } else {
      high = mid;
    }
  }

  return low;
}

/** بررسی کبیسه بودن سال شمسی */
function isLeapJalaaliYear(jy: number): boolean {
  const esfand30 = jalaaliToGregorian(jy, 12, 30);
  if (!esfand30.isValid) return false;
  const back = gregorianToJalaali(esfand30);
  return back.jy === jy && back.jm === 12 && back.jd === 30;
}

/** تعداد روزهای یک ماه شمسی */
function getMaxDayInJalaliMonth(jy: number, jm: number): number {
  if (jm <= 0) return 31; // قبل از انتخاب ماه
  if (jm <= 6) return 31;
  if (jm <= 11) return 30;
  return isLeapJalaaliYear(jy) ? 30 : 29;
}

@Component({
  // tslint:disable-next-line: component-selector
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
      multi: true,
      useExisting: SimpleJalaliDatepickerComponent,
    },
  ],
  standalone: false,
})
export class SimpleJalaliDatepickerComponent
  extends AppBase
  implements OnInit, ControlValueAccessor, Validator
{
  @Input() required = false;
  @Input() disabled = false;
  @Input() label = '';
  @Output() date = new EventEmitter<{ date: string; jDate: string } | null>();

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

  // tslint:disable-next-line: variable-name
  private _onChange = (val: string) => {};
  // tslint:disable-next-line: variable-name
  private _onTouched = () => {};
  myControl = new FormControl();

  constructor() {
    super();

    this.updateDayList();

    // سال شمسی جاری برای yearList
    const today = DateTime.now().setZone('Asia/Tehran');
    const jToday = gregorianToJalaali(today);

    for (let i = jToday.jy; i >= jToday.jy - 99; i--) {
      this.yearList.push(i);
    }
  }

  ngOnInit(): void {
    const validators = [this.validate];
    if (this.required) {
      validators.push(Validators.required);
    }
    this.myControl.setValidators(validators);
    this.myControl.updateValueAndValidity();
  }

  // ---------- Validator ----------
  validate(control: AbstractControl): ValidationErrors | null {
    const valueObj = control.value;
    if (valueObj && !this.isValidGregorianString(valueObj)) {
      return { invalid: true };
    }
    return null;
  }

  registerOnValidatorChange?(fn: () => void): void {}

  // ---------- ControlValueAccessor ----------
  writeValue(obj: any): void {
    if (obj) {
      const dt = DateTime.fromFormat(obj, 'yyyy/MM/dd', { zone: 'utc' });
      if (dt.isValid) {
        const jDate = gregorianToJalaali(dt);
        this.day = jDate.jd;
        this.month = jDate.jm;
        this.year = jDate.jy;
        this.updateDayList();
      }
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

  // ---------- وقتی کاربر مقداری انتخاب می‌کنه ----------
  onChange(): void {
    if (this.year == 0 || this.month == 0 || this.day == 0) {
      return;
    }
    
    this.isLeapYear = isLeapJalaaliYear(this.year);
    this.updateDayList();

    if (this.year && this.month && this.day) {
      const gDate = jalaaliToGregorian(this.year, this.month, this.day);
      const shamsiDate = this.formatJalaaliDate(this.year, this.month, this.day);

      if (gDate.isValid) {
        const miladyDate = gDate.toFormat('yyyy/MM/dd');
        this.myControl.setValue(miladyDate);
        this.date.emit({ date: miladyDate, jDate: shamsiDate });
      } else {
        this.date.emit(null);
      }
    }
  }

  // ---------- Helper ها ----------
  private updateDayList(): void {
    const maxDay = getMaxDayInJalaliMonth(this.year, this.month);
    this.dayList = Array.from({ length: maxDay }, (_, i) => i + 1);
    if (this.day > maxDay) {
      this.day = 0;
    }
  }

  private isValidGregorianString(date: any): boolean {
    if (!date || typeof date !== 'string') return false;
    const dt = DateTime.fromFormat(date, 'yyyy/MM/dd');
    return dt.isValid;
  }

  private formatJalaaliDate(year: number, month: number, day: number): string {
    const pad = (n: number) => String(n).padStart(2, '0');
    return `${year}/${pad(month)}/${pad(day)}`;
  }

  /**
   * اختیاری: فرمت کامل شمسی با Luxon (مثلاً برای نمایش در Tooltip یا جاهای دیگه)
   * مثال خروجی: ۱۴۰۲/۰۱/۰۱
   */
  formatPersian(gDate: DateTime, format = 'yyyy/MM/dd'): string {
    return gDate
      .setZone('Asia/Tehran')
      .setLocale('fa-IR')
      .reconfigure({ outputCalendar: 'persian' })
      .toFormat(format);
  }
}
