import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { merge, of as observableOf } from 'rxjs';
import { ServerApis } from '@core/server-apis';
import { AdminCitizenSmsListDialogComponent } from '../../admin/_dialogs/citizen-sms-list/citizen-sms-list.component';
import { AdminCitizenImageDialogComponent } from '../../admin/_dialogs/citizen-image/citizen-image.component';
import { AdminCitizenEditImageDialogComponent } from '../../admin/_dialogs/citizen-edit-image/citizen-edit-image.component';
import { AdminCitizenRejectImageDialogComponent } from '../../admin/_dialogs/citizen-reject-image/citizen-reject-image.component';
import { CitizenProfileDialogComponent } from '../../../shared/_dialog/citizen-profile/citizen-profile.component';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-card-citizens-pictures',
  templateUrl: './card-citizens-pictures.component.html',
  styleUrls: ['./card-citizens-pictures.component.scss'],
    standalone: false
})
export class CardCitizensPicturesComponent extends AppBase implements OnInit {
  searchForm: FormGroup;
  isLoadingResults: boolean = false;
  listCount: number = 10;
  data;
  baseUrl = ServerApis.baseUrl;
  personalPictureStatuses: [];
  list: any;
  loading: boolean = true;
  loadingData: boolean = true;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
) {
      super();
    this.searchForm = this.fb.group({
      fromDate: [null],
      toDate: [null],
      pictureConfirmed: [false],
      nationCode: [''],
    });
  }

  applyFilter() {
    if (this.paginator) {
      this.paginator.firstPage();
    }
    this.getList();
    this.getEnums();
  }

  ngOnInit(): void {
    this.getList();
    this.dataService.getEnums().subscribe((data) => {
      this.personalPictureStatuses = data.personalPicture;
    });
  }

  getList() {
    this.loadingData = true;
    this.data = [];
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
          return this.dataService.get(ServerApis.searchImageCardCitizens, param);
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
        this.data = data;
      });
  }
  getEnums() {}

  confirmImage(citizen) {
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

  openImageDialog(citizen) {
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
  openMessagesDialog(citizen) {
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

  openEditImageDialog(citizen) {
    this.matDialog
      .open(AdminCitizenEditImageDialogComponent, {
        panelClass: 'custom-dialog',
        data: citizen,
        width: '800px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getList();
      });
  }
  openRejectImageDialog(citizen) {
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
  refreshImage(citizen) {
    citizen.imageVersion = Math.random() * 1000;
  }
}
