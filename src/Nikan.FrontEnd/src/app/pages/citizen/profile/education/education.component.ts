import { Component, OnInit } from '@angular/core';
import { karjoEducationDto } from '@core/models/citizen/education';
import Swal from 'sweetalert2';
import { CitizenEducationDialogComponent } from '../_dialogs/education-dialog/education-dialog.component';
import { CitizenProfileComponent } from '../profile.component';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-education',
  templateUrl: './education.component.html',
  styleUrls: ['./education.component.scss'],
  standalone: false,
})
export class CitizenEducationComponent extends AppBase implements OnInit {
  educationList: karjoEducationDto[] = [];

  loading: boolean = true;
  isSaving: boolean = false;
  userId?: string;

  baseEnums: any = {};
  loadingEnums: boolean = true;

  constructor(private profileComponent: CitizenProfileComponent) {
    super();
    this.route.parent.params.subscribe((p) => {
      this.userId = p['id'] && p['id'] != '0' ? p['id'] : '';
      this.getEducationList();
    });
  }

  ngOnInit(): void {}

  getEducationList() {
    this.loading = true;
    this.dataService.get(ServerApis.getAllEducationByCitizen).subscribe(
      (response) => {
        this.loading = false;
        if (response && response.isSuccess) {
          this.educationList = response.data ? response.data : [];
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

  deleteEducation(row: karjoEducationDto) {
    Swal.fire({
      title: 'حذف تحصیلات',
      text: 'آیا برای حذف تحصیلات اطمینان دارید؟',
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
      showConfirmButton: true,
      showCancelButton: true,
    }).then((result) => {
      if (result.value) {
        row.loading = true;
        this.dataService
          .delete(ServerApis.deleteCitizenEducation, {
            id: +row.id,
          })
          .subscribe(
            (response) => {
              row.loading = false;
              if (response.isSuccess) {
                this.toastrService.success('حذف تحصیلات با موفقیت انجام شد.');
                for (var i = 0; i < this.educationList.length; i++) {
                  if (this.educationList[i].id == row.id) {
                    this.educationList.splice(i, 1);
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

  openEducationDialog(item:any) {
    this.matDialog
      .open(CitizenEducationDialogComponent, {
        data: {
          userId: this.userId,
          education: item,
        },
        panelClass: 'custom-dialog',
        // width: '600px'
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          var isUpdate = false;
          for (var i = 0; i < this.educationList.length; i++) {
            if (this.educationList[i].id == result.id) {
              this.educationList[i] = result;
              isUpdate = true;
            }
          }

          if (isUpdate == false) {
            this.educationList.push(result);
            this.profileComponent.getPersonalInfo();
          }
        }
      });
  }
}
