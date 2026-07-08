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
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-advanced-search',
  templateUrl: './citizen-advanced-search.component.html',
  styleUrls: ['./citizen-advanced-search.component.scss'],
  standalone: false,
})
export class AdminCitizenAdvancedSearchComponent extends AppBase implements AfterViewInit {

  displayedColumns: string[] = [
    'row',
    'nationCode',
    'firstName',
    'lastName',
    'birthDate',
    'age',
    'fatherName',
    'mobileNumber',
    'creationDate',
    'sabtStatus',
    'groups',
    'registerByService',
    'operation',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  isDownloadExcel: boolean = false;
  baseEnums: any = {};
  isfahanCities:any[]=[];

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
      birthDateFromDate: [null],
      birthDateToDate: [null],
      name: [''],
      lastname: [''],
      nationCode: [''],
      mobile: [''],
      gender: [null],
      hasFamily: [null],
      faceAuthentication: [null],
      nationality: [null],
      cityId: [null],
      groupId: [null],
      region: [null],
      sabtStatus: [null],
      pictureConfirmed: [null],
      mariageStatus: [null],
      registerByService: [null],
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
      (error:any) => {
        this.toastrService.error('خطا در ارتباط با سرور!');
      },
    );
  }

  getAppRegisterList() {
    this.dataService.get(ServerApis.getAppRegisterList).subscribe(
      (response) => {
        this.baseEnums.registerTypes = response.data;
      },
      (error:any) => {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  getGroups() {
    this.dataService.get(ServerApis.getGroups).subscribe(
      (response) => {
        this.baseEnums.citizenGroups = response.data;
      },
      (error:any) => {
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

    if (param.birthDateFromDate)
      param.birthDateFromDate = this.dataService.formatDate(param.birthDateFromDate);
    if (param.birthDateToDate)
      param.birthDateToDate = this.dataService.formatDate(param.birthDateToDate);

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.citizenAdvancedSearch, param);
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
        userCode: row.userCode,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }

  checkIsDead(item:any) {
    item.loading = true;
    this.dataService.get(ServerApis.checkIsDead, { nationCode: item.nationCode }).subscribe(
      (response) => {
        item.loading = false;
        if (response && response.isSuccess) {
          this.toastrService.success(response.messages);
          this.getList();
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error:any) => {
        item.loading = false;
      },
    );
  }

  exportExcel() {
    this.isDownloadExcel = true;
    var param: any = this.searchForm.value;
    param.offset = 0;
    param.count = 65000;
    if (param.fromDate) param.fromDate = this.dataService.formatDate(param.fromDate);
    if (param.toDate) param.toDate = this.dataService.formatDate(param.toDate);

    if (param.birthDateFromDate)
      param.birthDateFromDate = this.dataService.formatDate(param.birthDateFromDate);
    if (param.birthDateToDate)
      param.birthDateToDate = this.dataService.formatDate(param.birthDateToDate);

    if (param.fromDate) param.fromDate = this.dataService.formatDate(param.fromDate);
    if (param.toDate) param.toDate = this.dataService.formatDate(param.toDate);
    this.dataService.downloadFile(
      ServerApis.citizenAdvancedSearch_Export,
      param,
      '',
      'export-citizens.xls',
    );
    this.isDownloadExcel = false;
  }
}
