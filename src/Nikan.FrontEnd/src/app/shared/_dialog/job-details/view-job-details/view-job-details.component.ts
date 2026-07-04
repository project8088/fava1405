import { Component, OnInit, Input } from '@angular/core';
import * as moment from 'jalali-moment';
import { ServerApis } from '../../../../core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-view-job-details',
  templateUrl: './view-job-details.component.html',
  styleUrls: ['./view-job-details.component.scss'],
  standalone: false,
})
export class ViewJobDetailsComponent extends AppBase implements OnInit {
  @Input('jobId') jobId: string = '';

  loadingData: boolean = true;
  similarJabs: any[] = [];

  jobOpportunity: any = {};

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.getJobInfo();
  }

  getJobInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getJobForView, { id: this.jobId }).subscribe(
      (response) => {
        this.loadingData = false;
        if (response.isSuccess) {
          this.similarJabs = response.data.similarJobTitle
            ? response.data.similarJobTitle.split(',')
            : [];

          this.jobOpportunity = response.data;

          if (this.jobOpportunity.expireDate)
            this.jobOpportunity.expireDate = moment(new Date(this.jobOpportunity.expireDate))
              .locale('fa')
              .format('YYYY/MM/DD');
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
