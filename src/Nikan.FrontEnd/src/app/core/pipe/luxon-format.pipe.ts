import { ConvertNumbersToLatin } from '@core/helper/conver-number-to-latin.helper';
import { Pipe, PipeTransform } from '@angular/core';
import { DateTime } from 'luxon';
import { Calendars } from '@core/models/Calendars';
@Pipe({ standalone: false, name: 'luxonFormat' })
export class LuxonFormatPipe implements PipeTransform {
  /**
   * convert date time format to locale
   * @param value  date time
   * @param format 'yyyy-LL-dd HH:mm:ss'
   * @returns
   */
  transform(value: DateTime | string | Date | undefined, format: string = 'yyyy/LL/dd HH:mm') {
    if (!value) {
      return '';
    }
    const localName = 'fa-IR';
    // Intl not supported in safari
    // let userTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;
    // let userTimeZone = DateTime.local().zoneName;
    let userTimeZone = 'asia/tehran';
    if (typeof value === 'string') {
      value = DateTime.fromISO(value as any);
    }
    if (value instanceof Date) {
      value = DateTime.fromJSDate(value as any);
    }
    let str = value
      .setZone(userTimeZone)
      .setLocale(localName)
      .reconfigure({
        outputCalendar: Calendars.Persian,
      })
      .toFormat(format);
    // console.log(value, format, str,userTimeZone);
    return ConvertNumbersToLatin.fixNumbers(str);
  }
}
