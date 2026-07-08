import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ServerApis } from '@core/server-apis';
import { AdminUpdateCitizenMobileNumberDialogComponent } from './dialog/update-citizen-mobile-number/update-citizen-mobile-number.component';
import { AdminChangePasswordDialogComponent } from '../users/dialogs/change-user-password/change-user-password.component';
import { AdminUpdateCitizenSabtStateDialogComponent } from './dialog/update-citizen-sabt-state/update-citizen-sabt-state.component';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizens',
  templateUrl: './citizens.component.html',
  styleUrls: ['./citizens.component.scss'],
  standalone: false,
})
export class AdminCitizensComponent extends AppBase implements AfterViewInit {
  displayedColumns: string[] = [
    'row',
    'nationCode',
    'firstName',
    'lastName',
    'fatherName',
    'creationDate',
    'sabtStatus',
    'registerByService',
    'operation',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  searchForm: FormGroup;
  constructor(private customValidator: CustomFormValidators) {
    super();
    this.searchForm = this.fb.group({
      fromDate: [null],
      toDate: [null],
      name: [''],
      nationCode: [''],
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
          return this.dataService.get(ServerApis.searchCitizens, param);
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

  openCitizenEditMobileNumber(userCode:string) {
    this.matDialog
      .open(AdminUpdateCitizenMobileNumberDialogComponent, {
        panelClass: 'custom-dialog',
        minWidth: '600px',
        data: {
          userCode: userCode,
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          this.getList();
        }
      });
  }

  openChangePasswordDialog(row:any) {
    this.matDialog.open(AdminChangePasswordDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        userId: row.citizenId,
        userName: row.nationCode,
        displayName: row.firstName + ' ' + row.lastName,
      },
    });
  }

  openUpdateCitizenSabtStateDialog(userCode:string) {
    this.matDialog
      .open(AdminUpdateCitizenSabtStateDialogComponent, {
        panelClass: 'custom-dialog',
        data: {
          userCode: userCode,
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          this.getList();
        }
      });
  }
}
