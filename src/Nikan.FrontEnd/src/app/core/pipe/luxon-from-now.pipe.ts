import { Pipe, PipeTransform } from '@angular/core';
import { DateTime } from 'luxon';

@Pipe({ standalone: false, name: 'luxonFromNow' })
export class LuxonFromNowPipe implements PipeTransform {
  transform(value: DateTime) {
    if (!value) {
      return '';
    }

    return value.toRelative();
  }
}
