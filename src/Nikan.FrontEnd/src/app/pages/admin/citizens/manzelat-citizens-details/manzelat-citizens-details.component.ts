import { Component, OnInit } from '@angular/core';

import { ServerApis } from '@core/server-apis';
import { ShowImageDialogComponent } from '@app/shared/_dialog/show-image/show-image.component';
import { CitizenProfileDialogComponent } from '@app/shared/_dialog/citizen-profile/citizen-profile.component';
import { AdminUpdateCitizenMobileNumberDialogComponent } from '../_dialog/update-citizen-mobile-number/update-citizen-mobile-number.component';
import { AdminUpdateCitizenSabtStateDialogComponent } from '../_dialog/update-citizen-sabt-state/update-citizen-sabt-state.component';
import { AppBase } from '@app/app.base';
import { AdminCitizenManzelatReviewComponent } from '../_dialog/citizen-manzelat-review/citizen-manzelat-review.component';

@Component({
  selector: 'app-manzelat-citizens-details',
  templateUrl: './manzelat-citizens-details.component.html',
  styleUrls: ['./manzelat-citizens-details.component.scss'],
  standalone: false,
})
export class AdminManzelatCitizensDetailsComponent extends AppBase implements OnInit {
  userCode: string = '';
  data: any[] = [];
  citizen: any = {};
  loading: boolean = true;
  imageUrl: string = '';
  baseUrl: string = ServerApis.baseUrl;

  constructor() {
    super();
    this.route.params.subscribe((p) => {
      this.userCode = p['id'];
    });
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.getInfo();
  }

  getInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getCitizenInfoAndManzaltForm, {
        userCode: this.userCode,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess) {
            this.data = response.data ? response.data.manzaltForms : {};
            this.citizen = response.data ? response.data.citizen : {};
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {
          this.loading = false;
        },
      );
  }

  openReviewDialog(manzelatForm: any, command: any) {
    this.matDialog
      .open(AdminCitizenManzelatReviewComponent, {
        panelClass: 'custom-dialog',
        data: {
          manzelatForm,
          command,
          citizenName: this.citizen.firstName + ' ' + this.citizen.lastName,
          userCode: this.userCode,
        },
        width: '600px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getInfo();
      });
  }

  toggleConfirmFamily(family: any, isAccept: boolean) {
    this.dataService
      .post(ServerApis.confirmFamilyByAdmin, {
        userCode: this.userCode,
        familyId: family.familyCitizenId,
        isAccept: isAccept,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess) {
            this.toastrService.success(response.message);
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {
          this.loading = false;
        },
      );
  }

  back() {
    window.history.back();
  }

  showImageDialog(data: any) {
    this.matDialog
      .open(ShowImageDialogComponent, {
        panelClass: 'custom-dialog',
        data: {
          image: data.fileUrl,
          title: 'تصویر مدرک',
        },
        width: '800px',
      })
      .afterClosed()
      .subscribe((result) => {});
  }
  sendcitizenForAuthentication(citizenId: string) {
    this.dataService
      .get(ServerApis.citizenForAuthenticationByCitizenId, { citizenId: citizenId })
      .subscribe(
        (response) => {
          if (response.isSuccess && response.data) {
            this.toastrService.success(response.messages);
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
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

  openCitizenEditMobileNumber(userCode: string) {
    this.matDialog
      .open(AdminUpdateCitizenMobileNumberDialogComponent, {
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

  openUpdateCitizenSabtStateDialog() {
    this.matDialog
      .open(AdminUpdateCitizenSabtStateDialogComponent, {
        panelClass: 'custom-dialog',
        data: {
          userCode: this.userCode,
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          this.getInfo();
        }
      });
  }
}
