import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';

import { AdminCompanyChangeStatusDialogComponent } from '../../_dialogs/company-change-status/company-change-status.component';
import { AdminCompanyContractDialogComponent } from '../../_dialogs/company-contract/company-contract.component';
import { AuthService } from 'src/app/core/authentication/auth.service';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { DataService } from '../../../../core/services/data-service.service';
import { HelperService } from 'src/app/core/services/helper.service';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { RegisterServiceModel } from 'src/app/core/models/register-service.model';
import { Router } from '@angular/router';
import { ServerApis } from '../../../../core/server-apis';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { CitizenProfileDialogComponent } from '../../../../shared/_dialog/citizen-profile/citizen-profile.component';
import { CardProfileDialogComponent } from '../../../../shared/_dialog/card-profile/card-profile.component';
import { AdminBackCitizenCardDialogComponent } from '../dialog/back-citizen-card/back-citizen-card.component';
import { AdminDeliveredCitizenCardDialogComponent } from '../dialog/delivered-citizen-card/delivered-citizen-card.component';
import { AdminCancellationCitizenCardDialogComponent } from '../dialog/cancellation-citizen-card/cancellation-citizen-card.component';
import { AdminImportCardNumberDialogComponent } from '../dialog/import-card-number/import-card-number.component';

@Component({
  selector: 'adm-citizen-card-export-search',
  templateUrl: './citizen-card-export-search.component.html',
  styleUrls: ['./citizen-card-export-search.component.scss'],
})
export class AdminCitizenCardExportSearchComponent implements AfterViewInit {
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
        id: row.citizenId,
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

  createZipFilePicture(item) {
    item.loading = true;
    this.dataService.get(ServerApis.getExportCardPicture, { id: item.id }).subscribe(
      (response) => {
        item.loading = false;
        if (response && response.isSuccess) {
          this.toastrService.success('فایل زیپ تصاویر با موفقیت ایجاد شد.');
          this.router.navigate(['/admin/export-details-citizen-card/' + item.id]);
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

  openGetCardNumberDialog(item) {
    this.matDialog.open(AdminImportCardNumberDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        export: item,
      },
      width: '600px',
    });
  }
}
