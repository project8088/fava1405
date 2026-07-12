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
import { CitizenProfileDialogComponent } from '@app/shared/_dialog/citizen-profile/citizen-profile.component';
import { CardProfileDialogComponent } from '@app/shared/_dialog/card-profile/card-profile.component';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'card-citizen-card-search-in-queue',
  templateUrl: './citizen-card-search-in-queue.component.html',
  styleUrls: ['./citizen-card-search-in-queue.component.scss'],
  standalone: false,
})
export class CardCitizenCardSearchInQueueComponent extends AppBase implements AfterViewInit {
  displayedColumns: string[] = [
    'row',
    'nationCode',
    'citizen',
    'courseNumber',
    'queueName',
    'cardTitle',
    'queueOnDate',

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
      name: [''],
      nationCode: [''],
      cardNumber: [''],
      cardTypeId: [null],
      queueInputType: [null],
      courseNumber: [''],
    });
  }

  ngAfterViewInit() {
    this.getList();
  }

  getAppRegisterList() {
    this.dataService.get(ServerApis.getAppRegisterList).subscribe((response) => {
      this.baseEnums.registerTypes = response.data;
    });
  }
  getList() {
    var param: any = this.searchForm.value;
    param.offset = this.paginator ? this.paginator.pageIndex : 0;
    param.count = this.paginator ? this.paginator.pageSize : 10;

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.searchCardInQueue, param);
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
        id: row.requestId,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }
}
