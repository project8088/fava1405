import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'adm-manzalat-base-list',
  templateUrl: './manzalat-base-list.component.html',
  styleUrls: ['./manzalat-base-list.component.scss'],
  standalone: false,
})
export class AdminManzalatBaseFromListComponent extends AppBase implements OnInit {
  userGroupList: any[] = [];
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  loading: boolean = true;

  groupList: any[] = [];

  constructor(private customValidator: CustomFormValidators) {
    super();
  }

  ngOnInit() {
    this.getList();
  }

  getList() {
    this.loading = true;
    this.dataService.get(ServerApis.getManzalatBaseForms, {})
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response && response.isSuccess) {
                this.userGroupList = response.data ? response.data : [];
              } else {
                let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            }, (error: any) => {
              this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
            });
  }
}
