import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { HelperService } from '@core/services/helper.service';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ServerApis } from '../../../../core/server-apis';
import { CitizenProfileDialogComponent } from '../../../../shared/_dialog/citizen-profile/citizen-profile.component';
import { CardProfileDialogComponent } from '../../../../shared/_dialog/card-profile/card-profile.component';
import { AdminBackCitizenCardDialogComponent } from '../dialog/back-citizen-card/back-citizen-card.component';
import { AdminDeliveredCitizenCardDialogComponent } from '../dialog/delivered-citizen-card/delivered-citizen-card.component';
import { AdminCancellationCitizenCardDialogComponent } from '../dialog/cancellation-citizen-card/cancellation-citizen-card.component';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-citizen-card-export-details',
  templateUrl: './citizen-card-export-details.component.html',
  styleUrls: ['./citizen-card-export-details.component.scss'],
    standalone: false
})
export class AdminCitizenCardExportDetailsComponent extends AppBase implements AfterViewInit {
  displayedColumns: string[] = [
    'row',
    'nationCode',
    'citizen',
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
  exportId: string;
  baseEnums: any = {};

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;
  constructor(
    private customValidator: CustomFormValidators,
    private helperService: HelperService,
  ) {
      super();
    this.route.params.subscribe((p) => {
      this.exportId = p.id;
    });

    this.searchForm = this.fb.group({
      name: [''],
      nationCode: [''],
      cardNumber: [''],
      requestStatuse: [null],
      exportId: [0],
    });
  }

  ngAfterViewInit() {
    this.getList();
  }

  getList() {
    var param: any = this.searchForm.value;
    param.offset = this.paginator ? this.paginator.pageIndex : 0;
    param.count = this.paginator ? this.paginator.pageSize : 10;
    param.exportId = +this.exportId;

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.getPagedCardInfoExportDetails, param);
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
        this.data = data;
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

  openCitizenProfile(row) {
    this.matDialog.open(CitizenProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        userCode: row.userCode,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }

  openCardProfile(row) {
    this.matDialog.open(CardProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        id: row.citizenCardInfoId,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }

  openBackCard(item) {
    this.matDialog
      .open(AdminBackCitizenCardDialogComponent, {
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

  openDeliveredCard(item) {
    this.matDialog
      .open(AdminDeliveredCitizenCardDialogComponent, {
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

  openCancellationCard(item) {
    this.matDialog
      .open(AdminCancellationCitizenCardDialogComponent, {
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
