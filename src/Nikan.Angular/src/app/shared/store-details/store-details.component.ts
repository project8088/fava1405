import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router'; 
import { ToastrService } from 'ngx-toastr'; 
import { DataService } from '../../core/services/data-service.service';
import { ServerApis } from '../../core/server-apis';
import { AuthUser } from '../../core/authentication/user.model';
import { AuthService } from '../../core/authentication/auth.service';
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { MatPaginator, PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-store-details',
  templateUrl: './store-details.component.html',
  styleUrls: ['./store-details.component.scss']
})
export class StoreDetailsComponent implements OnInit {
  id: string; 
  loading: boolean;
  info: any;
   user: AuthUser;
    
  constructor(
    private route: ActivatedRoute,
    private dataService: DataService,
    private toastrService: ToastrService,
    private router: Router,
    private authService: AuthService
  ) {
    this.user = this.authService.currentUserValue;
    this.route.params.subscribe(p => {
      this.id = p.id;
      this.getInfo(); 
    });
  }

  ngOnInit(): void {
  }


 





  getInfo() { 
    this.loading = true;
    return this.dataService.get(ServerApis.getStoreSaleItems, { id: this.id }).subscribe(response => {
      this.loading = false;
      if (response.isSuccess && response.data) {
        this.info = response.data ? response.data : {};
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
      this.loading = false;
    });

  }

     


  buy() {
    this.toastrService.info('در حال حاضر امکان ارتباط با درگاه پرداخت برقرار نیست.');
  }



}
