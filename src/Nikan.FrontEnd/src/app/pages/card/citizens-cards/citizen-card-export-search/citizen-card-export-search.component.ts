import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';

import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { HelperService } from '@core/services/helper.service';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ServerApis } from '@core/server-apis';
import Swal from 'sweetalert2';
import { CitizenProfileDialogComponent } from '../../../../shared/_dialog/citizen-profile/citizen-profile.component';
import { CardProfileDialogComponent } from '../../../../shared/_dialog/card-profile/card-profile.component';
import { CardImportCardNumberDialogComponent } from '../dialog/import-card-number/import-card-number.component';
import { CardNewExportCardDialogComponent } from '../dialog/new-export-card/new-export-card.component';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'card-citizen-card-export-search',
  templateUrl: './citizen-card-export-search.component.html',
  styleUrls: ['./citizen-card-export-search.component.scss'],
  standalone: false,
})
export class CardCitizenCardExportSearchComponent extends AppBase implements AfterViewInit {
  displayedColumns: string[] = [
    'row',
    'export_Number',
    'creationDate',
    'dateSend',
    'exporterByUser',
    'countExport',
    'dateReceive',
    'importerByUser',
    'operation',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  baseEnums: any = {};
  isfahanCities;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  searchForm: FormGroup;
  constructor(
    private customValidator: CustomFormValidators,
    private helperService: HelperService,
  ) {
    super();
    this.searchForm = this.fb.group({
      fromDate: [null],
      toDate: [null],
    });
  }

  ngAfterViewInit() {
    this.getList();
  }

  getList() {
    var param: any = this.searchForm.value;
    param.offset = this.paginator ? this.paginator.pageIndex : 0;
    param.count = this.paginator ? this.paginator.pageSize : 10;
    if (param.fromDate) param.fromDate = this.dataService.formatDate(param.fromDate);
    if (param.toDate) param.toDate = this.dataService.formatDate(param.toDate);

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.getPagedCardInfoExport, param);
        }),
        map((response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.items ? response.data.items : [];
            this.listCount = response.data.totalItems ? response.data.totalItems : 0;
            return items;
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        }),
        catchError((err) => {
          this.isLoadingResults = false;
          return observableOf([]);
        }),
      )
      .subscribe((data) => {
       this.dataSource.data = data;
      });
  }

  pageEvent(event: PageEvent) {
    this.getList();
  }

  applyFilter() {
    if (this.paginator) {
      this.paginator.firstPage();
    }
    this.getList();
  }

  openCitizenProfile(row:any) {
    this.matDialog.open(CitizenProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        id: row.citizenId,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }

  openCardProfile(row:any) {
    this.matDialog.open(CardProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        id: row.id,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }

  createZipFilePicture(item:any) {
    item.loading = true;
    this.dataService.get(ServerApis.getExportCardPicture, { id: item.id }).subscribe(
      (response) => {
        item.loading = false;
        if (response && response.isSuccess) {
          this.toastrService.success('فایل زیپ تصاویر با موفقیت ایجاد شد.');
          this.router.navigate(['/card/export-details-citizen-card/' + item.id]);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        item.loading = false;
      },
    );
  }

  sendToPrint(item:any) {
    Swal.fire({
      title: 'آیا برای ارسال به چاپ اطمینان دارید؟',
      text: 'در صورت تائید وضعیت کارتهای این خروجی به حالت ارسال برای چاپ تبدیل خواهند شد. ',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        item.loading = true;
        this.dataService
          .get(ServerApis.sendToPrint, {
            id: item.id,
          })
          .subscribe(
            (response) => {
              item.loading = false;
              if (response.isSuccess) {
                this.toastrService.success(response.messages);
                this.router.navigate(['/card/export-details-citizen-card/' + item.id]);
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            },
            (error) => {
              item.loading = false;
            },
          );
      }
    });
  }

  delete(row:any) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا برای حذف اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        row.loading = true;
        this.dataService
          .get(ServerApis.removeExportCard, {
            id: row.id,
          })
          .subscribe(
            (response) => {
              row.loading = false;
              if (response.isSuccess) {
                this.toastrService.success('با موفقیت حذف شد.');
                this.getList();
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            },
            (error) => {
              row.loading = false;
            },
          );
      }
    });
  }

  openGetCardNumberDialog(item:any) {
    this.matDialog.open(CardImportCardNumberDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        export: item,
      },
      width: '600px',
    });
  }
  opennewExportDialog() {
    this.matDialog
      .open(CardNewExportCardDialogComponent, {
        panelClass: 'custom-dialog',
        minWidth: '600px',
        data: {},
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          this.getList();
        }
      });
  }
}
