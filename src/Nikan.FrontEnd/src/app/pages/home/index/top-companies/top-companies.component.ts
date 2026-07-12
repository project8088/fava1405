import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { OWL_OPTIONS } from '../../owal.config';
import { finalize } from 'rxjs';

@Component({
  selector: 'home-top-companies',
  templateUrl: './top-companies.component.html',
  styleUrls: ['./top-companies.component.scss'],
  standalone: false,
})
export class HomeTopCompaniesListComponent extends AppBase implements OnInit {
  data: any[] = [];
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  baseUrl: string = ServerApis.baseUrl;

  owlOptions = OWL_OPTIONS;
  constructor() {
    super();
  }

  ngOnInit() {
    this.getList();
  }

  getList() {
    this.isLoadingResults = true;
    return this.dataService
      .get(ServerApis.getTopCompanies)
      .pipe(
        finalize(() => {
          this.isLoadingResults = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response.isSuccess && response.data) {
            this.data = response.data ? response.data : [];
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
  }
}
