import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { merge, of as observableOf } from 'rxjs';

import { AdminCitizenEditImageDialogComponent } from '../../_dialogs/citizen-edit-image/citizen-edit-image.component';
import { AdminCitizenImageDialogComponent } from '../../_dialogs/citizen-image/citizen-image.component';
import { AdminCitizenRejectImageDialogComponent } from '../../_dialogs/citizen-reject-image/citizen-reject-image.component';
import { AdminCitizenSmsListDialogComponent } from '../../_dialogs/citizen-sms-list/citizen-sms-list.component';
import { ServerApis } from '@core/server-apis';
import { CitizenProfileDialogComponent } from '../../../../shared/_dialog/citizen-profile/citizen-profile.component';
import { AppBase } from '@app/app.base';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-citizens-pictures',
  templateUrl: './citizens-pictures.component.html',
  styleUrls: ['./citizens-pictures.component.scss'],
  standalone: false,
})
export class AdminCitizensPicturesComponent extends AppBase implements OnInit {
  searchForm: FormGroup;
  isLoadingResults: boolean = false;
  listCount: number = 10;
  dataSource = new MatTableDataSource();
  baseEnums: any = {};
  baseUrl = ServerApis.baseUrl;
  personalPictureStatuses: any[] = [];
  list: any;
  loading: boolean = true;
  loadingData: boolean = true;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor() {
    super();
    this.searchForm = this.fb.group({
      fromDate: [null],
      toDate: [null],
      hasCard: [null],
      groupId: [null],
      pictureConfirmed: [false],
      nationCode: [''],
    });

    this.getGroups();
  }

  ngOnInit(): void {
    this.getList();
    this.dataService.getEnums().subscribe((data) => {
      this.personalPictureStatuses = data.personalPicture;
    });
  }

  getGroups() {
    this.dataService.get(ServerApis.getGroups).subscribe(
      (response) => {
        this.baseEnums.citizenGroups = response.data;
      },
      (error: any) => {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  getList() {
    this.loadingData = true;
    this.dataSource.data = [];
    var param: any = this.searchForm.value;
    param.offset = this.paginator ? this.paginator.pageIndex : 0;
    param.count = this.paginator ? this.paginator.pageSize : 50;
    if (param.fromDate) param.fromDate = this.dataService.formatDate(param.fromDate);
    if (param.toDate) param.toDate = this.dataService.formatDate(param.toDate);

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.searchImageCitizens, param);
        }),
        map((response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.citizens ? response.data.citizens : [];
            this.list = items;

            this.listCount = response.data.totalItems ? response.data.totalItems : 0;
            this.loadingData = false;
            return items;
          } else {
            this.loadingData = false;
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        }),
        catchError((err) => {
          this.loadingData = false;
          this.isLoadingResults = false;
          return observableOf([]);
        }),
      )
      .subscribe((data) => {
        this.dataSource.data = data;
      });
  }
  getEnums() {}

  confirmImage(citizen: any) {
    citizen.isConfirming = true;
    this.dataService
      .get(ServerApis.acceptCitizenPicture + '?citizenId=' + citizen.citizenId)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastrService.success('تصویر شهروند با موفقیت تایید شد');
          //this.getList();
          for (let user of this.list) {
            if (user.citizenId === citizen.citizenId) {
              user.personalPicture_Confirmed = 1;
            }
          }
          citizen.isConfirming = false;
        } else {
          citizen.isConfirming = false;
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
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

  copyPicture() {
    this.dataService.get(ServerApis.copyPicture).subscribe((response) => {
      if (response.isSuccess) {
        this.toastrService.success(response.messages);
      } else {
      }
    });
  }

  openImageDialog(citizen: any) {
    this.matDialog
      .open(AdminCitizenImageDialogComponent, {
        panelClass: 'custom-dialog',
        data: citizen,
        width: '800px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getList();
      });
  }
  openMessagesDialog(citizen: any) {
    this.matDialog
      .open(AdminCitizenSmsListDialogComponent, {
        panelClass: 'custom-dialog',
        data: citizen,
        width: '800px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getList();
      });
  }

  openEditImageDialog(citizen: any) {
    this.matDialog
      .open(AdminCitizenEditImageDialogComponent, {
        panelClass: 'custom-dialog',
        data: citizen,
        width: '800px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          citizen.imageVersion = Math.random() * 1000;
        }
      });
  }
  openRejectImageDialog(citizen: any) {
    this.matDialog
      .open(AdminCitizenRejectImageDialogComponent, {
        panelClass: 'custom-dialog',
        data: citizen,
        width: '500px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          console.log(result);
          for (let user of this.list) {
            if (user.citizenId === citizen.citizenId) {
              user.personalPicture_Confirmed = 0;
            }
          }
        }
      });
  }
  openCitizenProfile(userCode: string) {
    this.matDialog.open(CitizenProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        userCode: userCode,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }
  refreshImage(citizen: any) {
    citizen.imageVersion = Math.random() * 1000;
  }
}
