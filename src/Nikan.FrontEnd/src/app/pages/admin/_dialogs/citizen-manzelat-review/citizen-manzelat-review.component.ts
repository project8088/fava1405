import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { ServerApis } from '@core/server-apis';
import { DataService } from '@core/services/data-service.service';

@Component({
  selector: 'app-citizen-manzelat-review',
  templateUrl: './citizen-manzelat-review.component.html',
  styleUrls: ['./citizen-manzelat-review.component.scss'],
})
export class AdminCitizenManzelatReviewComponent implements OnInit {
  form: FormGroup = new FormGroup({
    formResult: new FormControl(true),
    formResultDescription: new FormControl(''),
    sendSms: new FormControl(false),
  });

  isSaving: boolean;
  data: any;

  rejectCitizenPictureList: [];

  constructor(
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private matDialogRef: MatDialogRef<AdminCitizenManzelatReviewComponent>,
    private dataService: DataService,
    private toastService: ToastrService,
  ) {
    this.data = _data;

    if (this.data.command === 'Remove') this.data.title = 'حذف فرم ' + this.data.manzelatForm.title;
    else this.data.title = 'بررسی فرم ' + this.data.manzelatForm.title;
  }

  ngOnInit(): void {
    this.dataService.getEnums().subscribe((data) => {
      this.rejectCitizenPictureList = data.rejectCitizenPicture;
    });
  }

  saveInfo() {
    const formValues = this.form.value;

    if (this.data.command === 'Remove') {
      this.dataService
        .get(ServerApis.removeManzalatForm, {
          id: this.data.manzelatForm.manzalatRegisterId,
        })
        .subscribe(
          (response) => {
            if (response.isSuccess) {
              this.toastService.success(response.messages);
              this.matDialogRef.close(true);
            } else {
              let msg = response.messages
                ? response.messages
                : 'متاسفانه خطایی در سرور رخ داده است!';
              this.toastService.error(msg);
            }
          },
          (error) => {},
        );
    } else {
      this.dataService
        .post(ServerApis.confirmManzaltByAdmin, {
          userCode: this.data.userCode,
          manzalatFormType: this.data.manzelatForm.manzalatFormType,
          ...formValues,
        })
        .subscribe(
          (response) => {
            if (response.isSuccess) {
              this.toastService.success(response.message);
              this.matDialogRef.close(true);
            } else {
              let msg = response.messages
                ? response.messages
                : 'متاسفانه خطایی در سرور رخ داده است!';
              this.toastService.error(msg);
            }
          },
          (error) => {},
        );
    }
  }
}
