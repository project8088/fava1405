import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { merge, of as observableOf } from 'rxjs';
import { catchError, finalize, map, startWith, switchMap } from 'rxjs/operators';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { HelperService } from '@core/services/helper.service';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ServerApis } from '@core/server-apis';
import { CitizenProfileDialogComponent } from '@app/shared/_dialog/citizen-profile/citizen-profile.component';
import { CardProfileDialogComponent } from '@app/shared/_dialog/card-profile/card-profile.component';
import { CardCancellationCitizenCardDialogComponent } from '../dialog/cancellation-citizen-card/cancellation-citizen-card.component';
import { CardDeliveredCitizenCardDialogComponent } from '../dialog/delivered-citizen-card/delivered-citizen-card.component';
import { CardBackCitizenCardDialogComponent } from '../dialog/back-citizen-card/back-citizen-card.component';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'card-citizen-card-advanced-search',
  templateUrl: './citizen-card-advanced-search.component.html',
  styleUrls: ['./citizen-card-advanced-search.component.scss'],
  standalone: false,
})
export class CardCitizenCardAdvancedSearchComponent extends AppBase implements AfterViewInit {
  displayedColumns: string[] = [
    'row',
    'nationCode',
    'citizen',
    'sabtStatus',
    'personalPicture_Confirmed',
    'cardTitle',
    'requestDate',
    'requestStatuse',
    'deliverType',
    'cardNumber',
    'operation',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  baseEnums: any = {};
  isfahanCities: any[] = [];

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
      name: [''],
      nationCode: [''],
      cardNumber: [''],
      requestStatuse: [null],
      cardTypeId: [null],

      sabtStatus: [null],
      pictureConfirmed: [null],
      discountGroupId: [null],
      deliverType: [null],
    });
  }

  ngAfterViewInit() {
    this.getList();
    this.getBaseEnums();
    this.getAppRegisterList();
    this.getDisCountGroupBaseList();

    this.helperService.getIsfahanCities().subscribe((data) => {
      this.isfahanCities = data;
    });

    this.dataService.get(ServerApis.getNationalities).subscribe((data) => {
      this.baseEnums.nationalityList = data.data;
    });
  }

  /**
   * دریافت اطلاعات  پایه
   *
   * */
  getBaseEnums() {
    this.dataService.getEnums().subscribe(
      (response) => {
        if (response) {
          this.baseEnums.sabtStatus = response.sabtStatus;
          this.baseEnums.requestStatuse = response.cardRequestStatus;
          this.baseEnums.groupIds = response.groupIds;
        }
      },
      (error: any) => {
        this.toastrService.error('خطا در ارتباط با سرور!');
      },
    );
  }

  getAppRegisterList() {
    this.dataService.get(ServerApis.getAppRegisterList).subscribe((response) => {
      this.baseEnums.registerTypes = response.data;
    });
  }

  getDisCountGroupBaseList() {
    this.dataService.get(ServerApis.getDisCountGroupBaseList).subscribe((response) => {
      this.baseEnums.discountList = response.data;
    });
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
          return this.dataService.get(ServerApis.citizencardAdvSearch, param);
        }),
        finalize(() => {
          this.isLoadingResults = false;
          this.chdr.detectChanges();
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

  exportExcel() {
    var param: any = this.searchForm.value;
    param.offset = 0;
    param.count = 65000;
    if (param.fromDate) param.fromDate = this.dataService.formatDate(param.fromDate);
    if (param.toDate) param.toDate = this.dataService.formatDate(param.toDate);

    if (param.fromDate) param.fromDate = this.dataService.formatDate(param.fromDate);
    if (param.toDate) param.toDate = this.dataService.formatDate(param.toDate);
    this.dataService.downloadFile(
      ServerApis.citizencardAdvSearch_Export,
      param,
      '',
      'export-citizen-cards.xls',
    );
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

  openCitizenProfile(row: any) {
    this.matDialog.open(CitizenProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        userCode: row.userCode,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }

  openCardProfile(row: any) {
    this.matDialog.open(CardProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        id: row.id,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }

  openBackCard(item: any) {
    this.matDialog
      .open(CardBackCitizenCardDialogComponent, {
        panelClass: 'custom-dialog',
        data: {
          info: item,
        },
        width: '600px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getList();
      });
  }

  openDeliveredCard(item: any) {
    this.matDialog
      .open(CardDeliveredCitizenCardDialogComponent, {
        panelClass: 'custom-dialog',
        data: {
          info: item,
        },
        width: '600px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getList();
      });
  }

  openCancellationCard(item: any) {
    this.matDialog
      .open(CardCancellationCitizenCardDialogComponent, {
        panelClass: 'custom-dialog',
        data: {
          info: item,
        },
        width: '600px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getList();
      });
  }
}
