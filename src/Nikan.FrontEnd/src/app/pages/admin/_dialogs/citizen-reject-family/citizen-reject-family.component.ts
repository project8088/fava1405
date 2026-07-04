import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { ServerApis } from '@core/server-apis';
import { DataService } from '@core/services/data-service.service';

@Component({
  selector: 'app-citizen-reject-family',
  templateUrl: './citizen-reject-family.component.html',
  styleUrls: ['./citizen-reject-family.component.scss'],
})
export class AdminCitizenRejectFamilyComponent implements OnInit {
  form: FormGroup = new FormGroup({
    reason: new FormControl(''),
    description: new FormControl(''),
    sendSms: new FormControl(false),
  });

  isSaving: boolean;
  citizen: any;

  rejectCitizenPictureList: [];

  constructor(
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private matDialogRef: MatDialogRef<AdminCitizenRejectFamilyComponent>,
    private dataService: DataService,
    private toastService: ToastrService,
  ) {
    this.citizen = _data;
  }

  ngOnInit(): void {
    this.dataService.getEnums().subscribe((data) => {
      this.rejectCitizenPictureList = data.rejectCitizenPicture;
    });
  }

  saveInfo() {
    const formValues = this.form.value;

    this.dataService
      .post(ServerApis.removeFamily, {
        userCode: this.citizen.userCode,
        famillyId: this.citizen.family.familyCitizenId,
        description: formValues.description,
      })
      .subscribe(
        (response) => {
          if (response.isSuccess) {
            this.toastService.success(response.messages);
            this.matDialogRef.close(true);
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastService.error(msg);
          }
        },
        (error) => {},
      );
  }
}
