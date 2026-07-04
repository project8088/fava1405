import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-reject-family',
  templateUrl: './citizen-reject-family.component.html',
  styleUrls: ['./citizen-reject-family.component.scss'],
  standalone: false,
})
export class AdminCitizenRejectFamilyComponent extends AppBase implements OnInit {
  form: FormGroup = new FormGroup({
    reason: new FormControl(''),
    description: new FormControl(''),
    sendSms: new FormControl(false),
  });

  isSaving=false;
  citizen: any;

  rejectCitizenPictureList: [];

  constructor(
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private matDialogRef: MatDialogRef<AdminCitizenRejectFamilyComponent>,
  ) {
    super();
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
            this.toastrService.success(response.messages);
            this.matDialogRef.close(true);
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {},
      );
  }
}
