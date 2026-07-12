import { Component, AfterViewInit } from '@angular/core';
import { merge, of as observableOf, finalize } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { CitizenProfileDialogComponent } from '@app/shared/_dialog/citizen-profile/citizen-profile.component';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-citizens-authentication',
  templateUrl: './citizens-authentication.component.html',
  styleUrls: ['./citizens-authentication.component.scss'],
  standalone: false,
})
export class AdminCitizenAuthenticationComponent extends AppBase implements AfterViewInit {
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  loading: boolean = true;
  citizen: any = {};

  searchForm: FormGroup;
  constructor() {
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
          return this.dataService.get(ServerApis.citizenForAuthenticationByAdmin, param);
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
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((data) => {});
  }

  openCitizenProfile(citizenId: string) {
    this.matDialog.open(CitizenProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        id: citizenId,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }
}
