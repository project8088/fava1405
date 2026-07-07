import { MatPaginatorIntl } from '@angular/material/paginator';
import { Injectable } from '@angular/core';

@Injectable()
export class getFarsiPaginatorIntl extends MatPaginatorIntl {
  override itemsPerPageLabel = 'در هر صفحه';
  override nextPageLabel = 'بعدی';
  override previousPageLabel = 'قبلی';
  override firstPageLabel = 'اولین صفحه';
  override lastPageLabel = 'آخرین صفحه';
  override getRangeLabel = (page: number, pageSize: number, length: number): string => {
    if (length === 0 || pageSize === 0) {
      return ' 0 از ' + length;
    }
    length = Math.max(length, 0);
    const startIndex = page * pageSize;
    // If the start index exceeds the list length, do not try and fix the end index to the end.
    const endIndex =
      startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
    return 'نشان دادن ' + (startIndex + 1) + ' تا ' + endIndex + ' از ' + length + ' نتیجه';
  };
}
