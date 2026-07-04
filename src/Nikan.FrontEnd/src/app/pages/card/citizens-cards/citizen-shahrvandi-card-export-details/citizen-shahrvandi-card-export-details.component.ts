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
import { AppBase } from '@app/app.base';

@Component({
  selector: 'card-citizen-shahrvandi-card-export-details',
  templateUrl: './citizen-shahrvandi-card-export-details.component.html',
  styleUrls: ['./citizen-shahrvandi-card-export-details.component.scss'],
  standalone: false,
})
export class CardShahrvandiCitizenCardExportDetailsComponent
  extends AppBase
  implements AfterViewInit
{
  displayedColumns: string[] = [
    'nationCode',
    'citizenFirstName',
    'citizenLastName',
    'citizenFullName',
    'lenCitizenFullName',
    'lenNationCode',
    'expaireDate',
    'operation',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  exportId: string;
  baseEnums: any = {};
  baseUrl = ServerApis.baseUrl;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;
  constructor(
    private customValidator: CustomFormValidators,
    private helperService: HelperService,
  ) {
    super();
    this.route.params.subscribe((p) => {
      this.exportId = p['id'];
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
    param.count = this.paginator ? this.paginator.pageSize : 2000;
    param.exportId = +this.exportId;

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(
            ServerApis.getPagedShahrvandiCardInfoExportDetailsForSend,
            param,
          );
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
        id: row.citizenCardInfoId,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }
}
