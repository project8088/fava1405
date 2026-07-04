import { Component, Inject, Input, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';

import { ActivatedRoute } from '@angular/router';
import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-view-jobseeker-profile',
  templateUrl: './view-jobseeker-profile.component.html',
  styleUrls: ['./view-jobseeker-profile.component.scss'],
})
export class ViewJobseekerProfileComponent implements OnInit {
  @Input('jobseekerId') jobseekerId: string = '';

  jobseeker: any = {};
  loadingData: boolean;
  maritalStatus: boolean;

  baseUrl: string = ServerApis.baseUrl;

  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
  ) {}

  ngOnInit(): void {
    this.getJobseekerInfo();
    this.dataService.getEnums().subscribe((data) => {
      this.maritalStatus = data.maritalStatus.map((el) => el.text);
    });
  }

  getJobseekerInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getCitizenBaseInfo).subscribe(
      (response) => {
        this.loadingData = false;
        if (response.isSuccess) {
          this.jobseeker = response.data ? response.data : {};
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loadingData = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }
}
