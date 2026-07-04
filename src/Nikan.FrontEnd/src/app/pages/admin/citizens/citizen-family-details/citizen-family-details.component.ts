import { Component, OnInit } from '@angular/core';

import { AdminCitizenRejectFamilyComponent } from '../../_dialogs/citizen-reject-family/citizen-reject-family.component';
import { ServerApis } from '../../../../core/server-apis';
import { CitizenProfileDialogComponent } from '../../../../shared/_dialog/citizen-profile/citizen-profile.component';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-citizen-family-details',
  templateUrl: './citizen-family-details.component.html',
  styleUrls: ['./citizen-family-details.component.scss'],
    standalone: false
})
export class AdminCitizenFamilyDetailsComponent extends AppBase implements OnInit {
  userCode: string;
  familyList: [];

  citizen: any = {};
  loading: boolean = true;
  imageUrl: string;
  baseUrl: string = ServerApis.baseUrl;

  constructor(
) {
      super();
    this.route.params.subscribe((p) => {
      this.userCode = p.id;
    });
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.getInfo();
  }

  getInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getAllCitizenFamilyByAdmin, {
        userCode: this.userCode,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess) {
            this.familyList = response.data ? response.data.familyList : {};
            this.citizen = response.data ? response.data.citizen : {};
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

  openRejectFamilyeDialog(family) {
    this.matDialog
      .open(AdminCitizenRejectFamilyComponent, {
        panelClass: 'custom-dialog',
        data: {
          family,
          userCode: this.userCode,
        },
        width: '500px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getInfo();
      });
  }

  toggleConfirmFamily(family, isAccept: boolean) {
    this.dataService
      .post(ServerApis.confirmFamilyByAdmin, {
        userCode: this.userCode,
        familyUserCode: family.familyUserCode,
        isAccept: isAccept,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess) {
            this.toastrService.success(response.messages);
            this.getInfo();
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

  back() {
    window.history.back();
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
}
