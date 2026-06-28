import { Component, OnInit, Inject ,Input} from '@angular/core'; 
import { ToastrService } from 'ngx-toastr';
 import * as moment from 'jalali-moment';
import { ServerApis } from '../../../../../core/server-apis';
import { DataService } from '../../../../../core/services/data-service.service';
 
@Component({
  selector: 'admin-view-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.scss']
})
            
export class AdminViewEventDetailsComponent implements OnInit {
  @Input('id') id:string=''; 
  loadingData: boolean = true;  
  event: any = {}; 
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
    this.dataService.get(ServerApis.getEvent, { id: this.id }).subscribe(response => {
      this.loadingData = false;
      if (response.isSuccess) {
        this.event = response.data ? response.data : {};
       
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }

    }, error => {
      this.loadingData = false; 

    });
  }






}
