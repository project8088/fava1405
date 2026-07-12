import { Component, OnInit, Input } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-view-jobseeker-profile',
  templateUrl: './view-jobseeker-profile.component.html',
  styleUrls: ['./view-jobseeker-profile.component.scss'],
  standalone: false,
})
export class ViewJobseekerProfileComponent extends AppBase implements OnInit {
  @Input('jobseekerId') jobseekerId: string = '';

  jobseeker: any = {};
  loadingData?: boolean;

  baseUrl: string = ServerApis.baseUrl;

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.getJobseekerInfo();
  }

  getJobseekerInfo() {
    this.loadingData = true;
    this.dataService
      .get(ServerApis.getJobseekerProfile, { id: this.jobseekerId })
      .pipe(
        finalize(() => {
          this.loadingData = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response.isSuccess) {
            this.jobseeker = response.data ? response.data : {};
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
  }
}
