import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { ServerApis } from 'src/app/core/server-apis';
import { DataService } from 'src/app/core/services/data-service.service';

@Component({
  selector: 'app-citizen-reject-image',
  templateUrl: './citizen-reject-image.component.html',
  styleUrls: ['./citizen-reject-image.component.scss'],
})
export class AdminCitizenRejectImageDialogComponent implements OnInit {
  form: FormGroup = new FormGroup({
    reason: new FormControl(''),
    reasonRejectCitizenPicture: new FormControl(''),
    sendSms: new FormControl(false),
  });

  isSaving: boolean;
  citizen: any;

  rejectCitizenPictureList: [];

  constructor(
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private matDialogRef: MatDialogRef<AdminCitizenRejectImageDialogComponent>,
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
      .post(ServerApis.rejectCitizenPicture, {
        citizenId: this.citizen.citizenId,
        ...formValues,
      })
      .subscribe((data) => {
        this.toastService.success(data.messages);
        this.matDialogRef.close(true);
      });
  }
}
