import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr'; 
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ServerApis } from '../../../core/server-apis';
import { DataService } from '../../../core/services/data-service.service';



@Component({
    selector: 'app-show-citizen',
    templateUrl: './show-citizen.component.html',
    styleUrls: ['./show-citizen.component.scss']
})
  export class AppShowCitizenComponent implements AfterViewInit {
    userCode: string;
    info: any = {};
    loading: boolean=true;
  imageUrl: string;
  baseUrl: string = ServerApis.baseUrl;
 
      constructor(
      private toastrService: ToastrService,
      private matDialog: MatDialog,
      private router: Router,
      private dataService: DataService,
       private route: ActivatedRoute
      ) {

        this.route.params.subscribe(p => {
          this.userCode = p.id;
        });
    }

    ngOnInit(): void {
    
    }

    ngAfterViewInit() {
      this.getInfo();
    }

 


    getInfo() {
      this.loading = true; 
      this.dataService.get(ServerApis.getCitizenFullInfo, {
        userCode: this.userCode
      }).subscribe(response => {
        this.loading = false;
        if (response.isSuccess) {
          this.info = response.data ? response.data : {};
        } else {
          let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
          this.toastrService.error(msg);
        }
      }, error => {
        this.loading = false;

      });


    }

    back() {
      window.history.back();
    }
 

  sendcitizenForAuthentication(citizenId) {
    this.dataService.get(ServerApis.citizenForAuthenticationByCitizenId, { citizenId: citizenId }).subscribe(response => {
      if (response.isSuccess && response.data) {
        this.toastrService.success(response.messages);
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {

    });
  }






}


