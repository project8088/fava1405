import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';

import { AdminCompanyChangeStatusDialogComponent } from '../../_dialogs/company-change-status/company-change-status.component';
import { AdminCompanyContractDialogComponent } from '../../_dialogs/company-contract/company-contract.component';
import { AuthService } from '@core/authentication/auth.service';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { DataService } from '../../../../core/services/data-service.service';
import { HelperService } from '@core/services/helper.service';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { RegisterServiceModel } from '@core/models/register-service.model';
import { Router } from '@angular/router';
import { ServerApis } from '../../../../core/server-apis';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { CitizenProfileDialogComponent } from '../../../../shared/_dialog/citizen-profile/citizen-profile.component';
import { CardProfileDialogComponent } from '../../../../shared/_dialog/card-profile/card-profile.component';
import { AdminBackCitizenCardDialogComponent } from '../dialog/back-citizen-card/back-citizen-card.component';
import { AdminDeliveredCitizenCardDialogComponent } from '../dialog/delivered-citizen-card/delivered-citizen-card.component';
import { AdminCancellationCitizenCardDialogComponent } from '../dialog/cancellation-citizen-card/cancellation-citizen-card.component';

@Component({
  selector: 'adm-citizen-card-advanced-search',
  templateUrl: './citizen-card-advanced-search.component.html',
  styleUrls: ['./citizen-card-advanced-search.component.scss'],
})
export class AdminCitizenCardAdvancedSearchComponent implements AfterViewInit {
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

  baseEnums: any = {};
  isfahanCities;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;
  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private router: Router,
    private fb: FormBuilder,
    private customValidator: CustomFormValidators,
    private helperService: HelperService,
  ) {
    this.searchForm = this.fb.group({
      fromDate: [null],
      toDate: [null],
      name: [''],
      nationCode: [''],
      cardNumber: [''],
      requestStatuse: [null],
      cardTypeId: [null],
      hasCard: [null],
      hasdiscount: [null],
      sabtStatus: [null],
      pictureConfirmed: [null],
      discountGroupId: [null],
      deliverType: [null],
      groupId: [null],
      gender: [null],
    });
  }

  ngAfterViewInit() {
    this.getList();
    this.getBaseEnums();
    this.getAppRegisterList();
    this.getDisCountGroupBaseList();
    this.getGroups();
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
      (error) => {
        this.toastrService.error('خطا در ارتباط با سرور!');
      },
    );
  }

  getAppRegisterList() {
    this.dataService.get(ServerApis.getAppRegisterList).subscribe(
      (response) => {
        this.baseEnums.registerTypes = response.data;
      },
      (error) => {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  getGroups() {
    this.dataService.get(ServerApis.getGroups).subscribe(
      (response) => {
        this.baseEnums.citizenGroups = response.data;
      },
      (error) => {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
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
        id: row.id,
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

  getDisCountGroupBaseList() {
    this.dataService.get(ServerApis.getDisCountGroupBaseList).subscribe(
      (response) => {
        this.baseEnums.discountList = response.data;
      },
      (error) => {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
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
}
