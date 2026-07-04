import { Pipe, PipeTransform, Injectable } from '@angular/core';
//example *ngFor="let item of list| sort: 'priority';let i=index;"
@Pipe({
  name: 'sort',
})
@Injectable()
export class ArraySortPipe implements PipeTransform {
  transform(array: any, field: string): any[] {
    if (!Array.isArray(array)) {
      return;
    }
    array.sort((a: any, b: any) => {
      if (a[field] < b[field]) {
        return -1;
      } else if (a[field] > b[field]) {
        return 1;
      } else {
        return 0;
      }
    });
    return array;
  }
}
