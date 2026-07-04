import { Component, OnInit, Inject ,Input} from '@angular/core'; 
import { ToastrService } from 'ngx-toastr';
 import * as moment from 'jalali-moment';
 import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';

@Component({
  selector: 'app-view-notification-details',
  templateUrl: './notification-details.component.html',
  styleUrls: ['./notification-details.component.scss']
})
export class ViewNotificationDetailsComponent implements OnInit {
  @Input('id') id:string='';

  
  loadingData: boolean = true; 

  notification: any = {};

  baseUrl: string = ServerApis.baseUrl;


  constructor(
    private dataService: DataService,
    private toastrService: ToastrService
  ) {
  
  }

  ngOnInit(): void {
    this.getInfo();
  }



  getInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getNotification, { id: this.id , forEdit:false}).subscribe(response => {
      this.loadingData = false;
      if (response.isSuccess) {
        this.notification = response.data ? response.data : {};
       
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }

    }, error => {
      this.loadingData = false; 

    });
  }






}
