import { Component, OnInit } from '@angular/core';
import { karjoEducationDto } from 'src/app/core/models/citizen/education';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { MatDialog } from '@angular/material/dialog';
import { CitizenEducationDialogComponent } from '../_dialogs/education-dialog/education-dialog.component';
import { CitizenProfileComponent } from '../profile.component';
import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';

@Component({
  selector: 'app-citizen-education',
  templateUrl: './education.component.html',
  styleUrls: ['./education.component.scss']
})
export class CitizenEducationComponent implements OnInit {


  educationList: karjoEducationDto[] = [];

  loading: boolean = true;
  isSaving: boolean = false;
  userId: string;

  baseEnums: any = {};
  loadingEnums: boolean = true;

  constructor(
    private dataService: DataService,
    private route: ActivatedRoute,
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private profileComponent: CitizenProfileComponent
  ) {
    this.route.parent.params.subscribe(p => {
      this.userId = (p.id && p.id != '0') ? p.id : '';
      this.getEducationList();
    });
  }




  ngOnInit(): void {
  }



  getEducationList() {
    this.loading = true;
    this.dataService.get(ServerApis.getAllEducationByCitizen).subscribe(response => {
      this.loading = false;
      if (response && response.isSuccess) {
        this.educationList = response.data ? response.data : [];
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
      this.loading = false;
      this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
    });
  }



  deleteEducation(row: karjoEducationDto) {
    Swal.fire({
      title: 'حذف تحصیلات',
      text: 'آیا برای حذف تحصیلات اطمینان دارید؟',
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
      showConfirmButton: true,
      showCancelButton: true
    }).then(result => {
      if (result.value) {
        row.loading = true;
        this.dataService.delete(ServerApis.deleteCitizenEducation,
          {
            id: +row.id
          }).subscribe(response => {
            row.loading = false;
            if (response.isSuccess) {
              this.toastrService.success("حذف تحصیلات با موفقیت انجام شد.");
              for (var i = 0; i < this.educationList.length; i++) {
                if (this.educationList[i].id == row.id) {
                  this.educationList.splice(i, 1);
                }
              }
              this.profileComponent.getPersonalInfo();

            } else {
              let msg = response.messages ? response.messages : 'متاسفانه سرور با خطا مواجه شده است.';
              this.toastrService.error(msg);
            }
          }, error => {
            row.loading = false;
            this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');

          });
      }

    });
  }


  openEducationDialog(item) {
    this.matDialog.open(CitizenEducationDialogComponent, {
      data: {
        userId: this.userId,
        education: item
      },
      panelClass: 'custom-dialog',
      // width: '600px'
    }).afterClosed().subscribe(result => {

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
