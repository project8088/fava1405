import { Component, OnInit } from '@angular/core';
import { CitizenFamilyDialogComponent } from '../_dialogs/family-dialog/family-dialog.component';
import { CitizenProfileComponent } from '../profile.component';
import { ServerApis } from '@core/server-apis';
import Swal from 'sweetalert2';
import { citizenFamilyModel } from '@core/models/citizen/family.model';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-my-family',
  templateUrl: './my-family.component.html',
  styleUrls: ['./my-family.component.scss'],
  standalone: false,
})
export class CitizenMyFamilyComponent extends AppBase implements OnInit {
  familyList: any[] = [];
  familyByfamilyList: any[] = [];

  loading: boolean = true;
  isSaving: boolean = false;
  userId?: string;

  baseEnums: any = {};
  loadingEnums: boolean = true;

  constructor(private profileComponent: CitizenProfileComponent) {
    super();
    this.route.parent.params.subscribe((p) => {
      this.userId = p['id'] && p['id'] != '0' ? p['id'] : '';
      this.getFamilyList();
    });
  }

  ngOnInit(): void {
    this.getAllCitizenFamilyByFamily();
  }

  getFamilyList() {
    this.loading = true;
    this.dataService.get(ServerApis.getAllCitizenFamily).subscribe(
      (response) => {
        this.loading = false;
        if (response && response.isSuccess) {
          this.familyList = response.data ? response.data : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error:any) => {
        this.loading = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  deleteEducation(row: citizenFamilyModel) {
    Swal.fire({
      title: 'حذف عضو خانواده',
      text: 'آیا برای حذف عضو خانواده اطمینان دارید؟',
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
      showConfirmButton: true,
      showCancelButton: true,
    }).then((result) => {
      if (result.value) {
        row.loading = true;
        this.dataService
          .get(ServerApis.removeFamilyByCitizen, {
            id: +row.id,
          })
          .subscribe(
            (response) => {
              row.loading = false;
              if (response.isSuccess) {
                this.toastrService.success('حذف عضو خانواده با موفقیت انجام شد.');
                for (var i = 0; i < this.familyList.length; i++) {
                  if (this.familyList[i].id == row.id) {
                    this.familyList.splice(i, 1);
                  }
                }
                this.profileComponent.getPersonalInfo();
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه سرور با خطا مواجه شده است.';
                this.toastrService.error(msg);
              }
            },
            (error:any) => {
              row.loading = false;
              this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
            },
          );
      }
    });
  }

  openFamilyDialog(family) {
    this.matDialog
      .open(CitizenFamilyDialogComponent, {
        data: family,
        panelClass: 'custom-dialog',
        width: '1000px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          var isUpdate = false;
          for (var i = 0; i < this.familyList.length; i++) {
            if (this.familyList[i].id == result.id) {
              this.familyList[i] = result;
              isUpdate = true;
            }
          }

          if (isUpdate == false) {
            this.familyList.push(result);
            this.profileComponent.getPersonalInfo();
          }
        }
      });
  }

  getAllCitizenFamilyByFamily() {
    this.dataService.get(ServerApis.getAllCitizenFamilyByFamily).subscribe(
      (response) => {
        if (response && response.isSuccess) {
          this.familyByfamilyList = response.data ? response.data.familyList : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error:any) => {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }
}
