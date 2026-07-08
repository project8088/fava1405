import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-reject-image',
  templateUrl: './citizen-reject-image.component.html',
  styleUrls: ['./citizen-reject-image.component.scss'],
  standalone: false,
})
export class AdminCitizenRejectImageDialogComponent extends AppBase implements OnInit {
  form: FormGroup = new FormGroup({
    reason: new FormControl(''),
    reasonRejectCitizenPicture: new FormControl(''),
    sendSms: new FormControl(false),
  });

  isSaving=false;
  citizen: any;

  rejectCitizenPictureList: any[]=[];

  constructor(
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private matDialogRef: MatDialogRef<AdminCitizenRejectImageDialogComponent>,
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
      .post(ServerApis.rejectCitizenPicture, {
        citizenId: this.citizen.citizenId,
        ...formValues,
      })
      .subscribe((data) => {
        this.toastrService.success(data.messages);
        this.matDialogRef.close(true);
      });
  }
}
