import { Component, OnInit, Inject ,Input} from '@angular/core'; 
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'jalali-moment';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';

@Component({
  selector: 'app-view-job-details',
  templateUrl: './view-job-details.component.html',
  styleUrls: ['./view-job-details.component.scss']
})
export class ViewJobDetailsComponent implements OnInit {
  @Input('jobId') jobId:string='';

  
  loadingData: boolean = true;
  similarJabs: any[] = []; 

  jobOpportunity: any = {};



  constructor(
    private dataService: DataService,
    private toastrService: ToastrService
  ) {
  
  }

  ngOnInit(): void {
    this.getJobInfo();
  }



  getJobInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getJobForView, { id: this.jobId }).subscribe(response => {
      this.loadingData = false;
      if (response.isSuccess) {

        this.similarJabs = (response.data.similarJobTitle) ? response.data.similarJobTitle.split(',') : [];
        


        this.jobOpportunity = response.data;

        if(this.jobOpportunity.expireDate)
        this.jobOpportunity.expireDate= moment(new Date( this.jobOpportunity.expireDate)).locale('fa').format('YYYY/MM/DD') ;

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
