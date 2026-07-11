import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs/operators';
import { OWL_OPTIONS } from '../../owal.config';

@Component({
  selector: 'home-services',
  templateUrl: './services.component.html',
  styleUrls: ['./services.component.scss'],
  standalone: false,
})
export class HomeServicesListComponent extends AppBase implements OnInit {
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
      .get(ServerApis.getAppRegisterList)
      .pipe(
        finalize(() => {
          this.isLoadingResults = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        this.isLoadingResults = false;
        if (response.isSuccess && response.data) {
          this.data = response.data ? response.data : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      });
  }
}
