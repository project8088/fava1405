import { Pipe, PipeTransform, Injectable } from '@angular/core';
import * as moment from 'jalali-moment';

@Pipe({
  name: 'jalali'
})
@Injectable()
export class JalaliPipe implements PipeTransform {

 

  transform(val: string, format: string = 'YYYY/MM/DD'): string {
    if (val)
      return moment(new Date(val)).locale('fa').format(format);
  }
 

}
