import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';

import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { HelperService } from '@core/services/helper.service';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ServerApis } from '../../../../core/server-apis';
import Swal from 'sweetalert2';
import { CitizenProfileDialogComponent } from '../../../../shared/_dialog/citizen-profile/citizen-profile.component';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-citizen-authentication-search',
  templateUrl: './citizen-authentication-search.component.html',
  styleUrls: ['./citizen-authentication-search.component.scss'],
    standalone: false
})
export class AdminCitizenAuthenticationSearchComponent extends AppBase implements AfterViewInit {
  displayedColumns: string[] = [
    'row',
    'nationCode',
    'firstName',
    'lastName',
    'birthDate',
    'fatherName',
    'registerByService',
    'creationDate',
    'sabtStatus',
    'lastUpdateOnDate',

    'authenticationByService',
    'requestId',
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
    private customValidator: CustomFormValidators,
    private helperService: HelperService,
  ) {
      super();
    this.searchForm = this.fb.group({
      fromDate: [null],
      toDate: [null],
      registerByService: [null],
      nationCode: [''],
      sabtStatus: [null],
    });
  }

  ngAfterViewInit() {
    this.getList();
    this.getBaseEnums();
    this.getAppRegisterList();
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
          //this.baseEnums.sabtStatus = response.sabtStatus;
          this.baseEnums.maritalStatus = response.maritalStatus;
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
          return this.dataService.get(ServerApis.searchCitizensAuthentication, param);
        }),
        map((response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.citizens ? response.data.citizens : [];
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
}
