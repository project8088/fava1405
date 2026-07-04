import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';

import { AuthService } from '@core/authentication/auth.service';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { DataService } from '../../../../core/services/data-service.service';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ServerApis } from '../../../../core/server-apis';
import { ToastrService } from 'ngx-toastr';
import { CitizenProfileDialogComponent } from '../../../../shared/_dialog/citizen-profile/citizen-profile.component';
import Swal from 'sweetalert2';
import { AdminUpdateCitizenSabtStateDialogComponent } from '../dialog/update-citizen-sabt-state/update-citizen-sabt-state.component';

@Component({
  selector: 'app-manzelat-citizens',
  templateUrl: './manzelat-citizens.component.html',
  styleUrls: ['./manzelat-citizens.component.scss'],
})
export class AdminManzelatCitizensComponent implements AfterViewInit {
  displayedColumns: string[] = [
    'row',
    'checkbox',
    'nationCode',
    'firstName',
    'lastName',
    'fatherName',
    'creationDate',
    'age',
    'mobile',
    'formTitle',
    'sabtStatus',
    'documentUploaded',
    'formStatuse',
  ];
  isDownloadExcel: boolean = false;
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  manzalatFormStatuse: [];
  sendingSms: boolean = false;
  selectAll: boolean = false;

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
  ) {
    this.searchForm = this.fb.group({
      fromDate: [null],
      toDate: [null],
      birthDateFromDate: [null],
      birthDateToDate: [null],
      name: [''],
      gender: [null],
      sabtStatus: [null],
      mariageStatus: [null],
      formStatuse: [''],
      inManzalatGroups: [null],
      hasCard: [null],
      manzalatFormType: [''],
      nationCode: [''],
    });
  }

  ngAfterViewInit() {
    this.getList();
    this.getManzalatFormStatuse();
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
          return this.dataService.get(ServerApis.searchManzaltCitizens, param);
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
      ServerApis.searchManzaltCitizens_Export,
      param,
      '',
      'export-manzalat-citizens.xls',
    );
    this.isDownloadExcel = false;
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

  getManzalatFormStatuse() {
    this.dataService.getEnums().subscribe((data) => {
      this.manzalatFormStatuse = data.manzalatFormStatuse;
    });
  }

  openCitizenProfile(row) {
    this.matDialog.open(CitizenProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        id: row.citizenId,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }

  sendSms() {
    var selectedIds = [];
    for (var i = 0; i < this.data.length; i++) {
      if (this.data[i].selected && this.data[i].sabtStatus == 0)
        selectedIds.push(this.data[i].citizenId);
    }

    if (selectedIds.length == 0) {
      this.toastrService.warning(
        'رکورد هایی که می خواهید برای آن ها پیامک ارسال شود را انتخاب کنید.',
      );
      return;
    }

    Swal.fire({
      title: 'ارسال پیامک',
      text:
        'شما ' +
        selectedIds.length +
        ' مورد برای ارسال پیامک انتخاب کرده اید. برای ارسال پیامک اطمینان دارید؟',
      showCancelButton: true,
      showConfirmButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.isConfirmed) {
        this.sendingSms = true;
        this.dataService
          .post(ServerApis.sendSabtAhvalCitizensSms, {
            ExportId: 0,
            Ids: selectedIds,
          })
          .subscribe(
            (response) => {
              this.sendingSms = false;
              if (response.isSuccess) {
                Swal.fire({
                  title: 'پیامک با موفقیت ارسال شد',
                  text: response.messages,
                });
                this.toastrService.success('پیامک با موفقیت ارسال شد.');
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            },
            (error) => {
              this.sendingSms = false;
              this.toastrService.error('متاسفانه خطایی در سرور رخ داده است!');
            },
          );
      }
    });
  }

  selectUnselectAll() {
    for (var i = 0; i < this.data.length; i++) {
      this.data[i].selected = this.selectAll;
    }
  }
}
