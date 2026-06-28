import { Component, OnInit, Inject, Input } from '@angular/core'; 
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';

@Component({
  selector: 'app-view-jobseeker-profile',
  templateUrl: './view-jobseeker-profile.component.html',
  styleUrls: ['./view-jobseeker-profile.component.scss']
})
export class ViewJobseekerProfileComponent implements OnInit {
  @Input('jobseekerId') jobseekerId:string=''; 
    
  jobseeker: any = {};
  loadingData: boolean;

  baseUrl: string = ServerApis.baseUrl;


  constructor(
    private dataService: DataService,
    private toastrService: ToastrService
  ) {
 
  }

  ngOnInit(): void {
    this.getJobseekerInfo();
  }



  getJobseekerInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getJobseekerProfile,{ id: this.jobseekerId }).subscribe(response => {
      this.loadingData = false;
      if (response.isSuccess) {
        this.jobseeker = response.data ? response.data : {}; 
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
      this.loadingData = false;
      this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
    });
  }






}
