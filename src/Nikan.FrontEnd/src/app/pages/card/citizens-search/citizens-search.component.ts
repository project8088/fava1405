import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { CustomFormValidators } from '../../../core/custom-validator/form-validation';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ServerApis } from '../../../core/server-apis';
import { CitizenProfileDialogComponent } from '../../../shared/_dialog/citizen-profile/citizen-profile.component';
import { CardUpdateCitizenMobileNumberDialogComponent } from '../dialog/update-citizen-mobile-number/update-citizen-mobile-number.component';
import { CardUpdateCitizenSabtStateByCardDialogComponent } from '../dialog/update-citizen-sabt-state-by-card/update-citizen-sabt-state-by-card.component';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizens-search',
  templateUrl: './citizens-search.component.html',
  styleUrls: ['./citizens-search.component.scss'],
  standalone: false,
})
export class CardCitizenSearchComponent extends AppBase implements AfterViewInit {
  displayedColumns: string[] = [
    'row',
    'nationCode',
    'firstName',
    'lastName',
    'fatherName',
    'creationDate',
    'sabtStatus',
  ];
  baseUrl: string = ServerApis.baseUrl;
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  loading: boolean = true;
  citizen: any = {};
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;
  constructor(private customValidator: CustomFormValidators) {
    super();
    this.searchForm = this.fb.group({
      birthDate: [null],
      nationCode: [''],
    });
  }

  ngAfterViewInit() {}

  getinfo() {
    this.loading = true;

    var param: any = this.searchForm.value;
    if (param.birthDate) param.birthDate = this.dataService.formatDate(param.birthDate);

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.loading = false;
          return this.dataService.get(ServerApis.searchCitizenByCardUser, param);
        }),
        map((response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.citizen = response.data ? response.data : {};
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        }),
        catchError((err) => {
          this.loading = false;
          return observableOf([]);
        }),
      )
      .subscribe((data) => {});
  }

  openCitizenProfile(userCode) {
    this.matDialog.open(CitizenProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        userCode: userCode,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }

  openCitizenEditMobileNumber(userCode) {
    this.matDialog
      .open(CardUpdateCitizenMobileNumberDialogComponent, {
        panelClass: 'custom-dialog',
        minWidth: '600px',
        data: {
          userCode: userCode,
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          this.getinfo();
        }
      });
  }

  openUpdateCitizenSabtState(userCode) {
    this.matDialog
      .open(CardUpdateCitizenSabtStateByCardDialogComponent, {
        panelClass: 'custom-dialog',
        minWidth: '600px',
        data: {
          userCode: userCode,
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          this.getinfo();
        }
      });
  }
}
