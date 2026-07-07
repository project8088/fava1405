import { Component, AfterViewInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { AdminChangePasswordDialogComponent } from '@app/pages/admin/users/dialogs/change-user-password/change-user-password.component';
import { CardUpdateCitizenMobileNumberDialogComponent } from '@app/pages/card/dialog/update-citizen-mobile-number/update-citizen-mobile-number.component';

@Component({
  selector: 'app-view-citizen',
  templateUrl: './view-citizen.component.html',
  styleUrls: ['./view-citizen.component.scss'],
  standalone: false,
})
export class ViewCitizenComponent extends AppBase implements AfterViewInit {
  citizenId: string = '';
  info: any = {};
  loading: boolean = true;
  imageUrl: string = '';
  baseUrl: string = ServerApis.baseUrl;
  constructor() {
    super();
    this.route.params.subscribe((p) => {
      this.citizenId = p['id'] ? p['id'] : null;
    });
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.getInfo();
  }

  getInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getCitizenFullInfo, {
        id: this.citizenId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess) {
            this.info = response.data ? response.data : {};
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loading = false;
        },
      );
  }

  openChangePasswordDialog(row: any) {
    this.matDialog.open(AdminChangePasswordDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        userId: row.citizenId,
        userName: row.nationCode,
        displayName: row.firstName + ' ' + row.lastName,
      },
    });
  }
  openCitizenEditMobileNumber(userCode: string) {
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
          this.getInfo();
        }
      });
  }
  back() {
    window.history.back();
  }
}
