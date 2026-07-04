import { Component, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { ServerApis } from '../../../core/server-apis';
import { DataService } from '../../../core/services/data-service.service';
 @Component({
   selector: 'home-bank-call-back',
  templateUrl: './bank-call-back.component.html',
  styleUrls: ['./bank-call-back.component.scss']
})
export class BankCallBackComponent implements OnInit {
   loading: boolean;
   id: string;

   response: any;

   constructor(
     private dataService: DataService,
    private fb: FormBuilder,
     private toastrService: ToastrService,
     private route: ActivatedRoute
   ) {
     this.route.queryParams.subscribe(p => {
       if (p.id) {
         this.id = p.id;
         this.payVerify();
       } else {
         this.toastrService.warning("شناسه پرداخت وارد نشده است.");
       }
     });

  }

  ngOnInit(): void {

  }

  
  

 


   payVerify() {
    


     this.loading = true; 
     this.dataService.post(ServerApis.showPayResult, { id:this.id }).subscribe(response => {
       this.loading = false;
       this.response = response;
         if (response.isSuccess) {
            
         
          } else {
            let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
            this.toastrService.error(msg);
          }
        }, error => {
     this.loading = false; 
        });
       
  }


}
