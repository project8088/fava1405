import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'app-citizen-manzalat-registration',
  templateUrl: './citizen-manzalat-registration.component.html',
  styleUrls: ['./citizen-manzalat-registration.component.scss'],
  standalone: false,
})
export class AppCitizenManzalatRegistrationComponent extends AppBase implements AfterViewInit {
  userCode: string = '';
  baseUrl: string = ServerApis.baseUrl;

  loading?: boolean;
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor() {
    super();
    this.route.params.subscribe((p) => {
      this.userCode = p['id'] ? p['id'] : null;
    });
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.getManzalatInfo();
  }

  getManzalatInfo() {
    this.loading = true;
    this.dataService
            .get(ServerApis.getCitizenInfoAndManzaltForm, {
              userCode: this.userCode,
            })
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
                if (response.isSuccess) {
                  this.data = response.data ? response.data.manzaltForms : {};
                }
              }, (error: any) => {
              });
  }
}
