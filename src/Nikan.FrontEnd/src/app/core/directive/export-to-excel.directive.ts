import { Directive, Input } from '@angular/core';
import * as XLSX from 'xlsx';
import { MatTableDataSource } from '@angular/material/table';
import { saveAs } from 'file-saver';
import { BookType } from 'xlsx';
import { DateTime } from 'luxon';
import { LuxonFormatPipe } from '@core/pipe/luxon-format.pipe';

@Directive({
  selector: '[matTableExporter]',
  exportAs: 'matTableExporter',
  standalone: false,
})
export class ExportToExcelDirective {
  @Input() dataSource!: MatTableDataSource<any>;
  @Input() hiddenColumns: number[] = []; // ستون‌های مخفی

  // متدی برای خروجی گرفتن جدول به فرمت مشخص (xlsx)
  exportTable(format: BookType = 'xlsx', options?: { fileName: string }): void {
    const fileName = options?.fileName || 'table_data';

    // استخراج داده‌ها و فیلتر کردن ستون‌های مخفی
    const dataToExport = this.dataSource.filteredData.map((row) => {
      const filteredRow = Object.keys(row)
        .filter((key, index) => !this.hiddenColumns.includes(index)) // حذف ستون‌های مشخص‌شده
        .reduce((obj: any, key) => {
          obj[key] = row[key];
          if (obj[key] instanceof DateTime) {
            obj[key] = new LuxonFormatPipe().transform(obj[key], 'F');
          } else if (obj[key] !== null && typeof obj[key] == 'object') {
            obj[key] = obj[key].toString();
          }
          return obj;
        }, {});
      return filteredRow;
    });

    // تبدیل داده‌ها به فرمت worksheet
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataToExport);

    // ایجاد یک کتاب (workbook)
    const workbook: XLSX.WorkBook = {
      Sheets: { Sheet1: worksheet },
      SheetNames: ['Sheet1'],
    };

    // تولید فایل اکسل با فرمت xlsx
    const excelBuffer: any = XLSX.write(workbook, { bookType: format, type: 'array' });

    // ذخیره فایل اکسل
    this.saveAsExcelFile(excelBuffer, fileName);
  }

  private saveAsExcelFile(buffer: any, fileName: string): void {
    const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
    const data: Blob = new Blob([buffer], { type: EXCEL_TYPE });
    saveAs(data, `${fileName}_export_${new Date().getTime()}.xlsx`);
  }
}
