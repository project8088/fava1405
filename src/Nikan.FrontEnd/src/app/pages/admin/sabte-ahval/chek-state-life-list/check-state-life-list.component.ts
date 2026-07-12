import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { CitizenProfileDialogComponent } from '@app/shared/_dialog/citizen-profile/citizen-profile.component';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'app-check-state-life-list',
  templateUrl: './check-state-life-list.component.html',
  styleUrls: ['./check-state-life-list.component.scss'],
  standalone: false,
})
export class AdminCheckStateLifeListComponent extends AppBase implements AfterViewInit, OnInit {
  id?: number;

  displayedColumns: string[] = [
    'row',
    'firstName',
    'lastName',
    'fatherName',
    'nationCode',
    'birthDate',
    'verifyDate',
    'sabtStatus',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  searchForm: FormGroup;

  sendingSms: boolean = false;

  constructor(private customValidator: CustomFormValidators) {
    super();
    this.route.params.subscribe((p) => {
      this.id = p['id'];
    });
    this.searchForm = this.fb.group({
      title: [null],
    });
  }

  ngOnInit() {}

  ngAfterViewInit() {
    this.getList();
  }

  getList() {
    this.isLoadingResults = true;
    var param: any = {
      offset: 0,
      count: 10000,
      exportId: this.id,
    };

    this.data = [];
    this.dataService.get(ServerApis.getAllCitizenExported, param)
      .pipe(
        finalize(() => {
          this.isLoadingResults = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response.isSuccess && response.data) {
                this.data = response.data.items ? response.data.items : [];
                this.dataSource.data = this.data;
                this.listCount = response.data.totalItems ? response.data.totalItems : 0;
              } else {
                let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            }, (error: any) => {
            });
  }

  applyFilter() {
    this.dataSource.filter = this.searchForm.get('title')?.value;
  }

  checkIsDead(item: any) {
    item.loading = true;
    this.dataService.get(ServerApis.checkIsDead, { nationCode: item.nationCode }).subscribe(
      (response) => {
        item.loading = false;
        if (response && response.isSuccess) {
          this.toastrService.success(response.messages);
          this.getList();
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error: any) => {
        item.loading = false;
      },
    );
  }

  openCitizenProfile(row: any) {
    this.matDialog.open(CitizenProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        userCode: row.userCode,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }
}
