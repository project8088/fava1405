import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router'; 
import { ToastrService } from 'ngx-toastr'; 
import { DataService } from '../../core/services/data-service.service';
import { ServerApis } from '../../core/server-apis';
import { AuthUser } from '../../core/authentication/user.model';
import { AuthService } from '../../core/authentication/auth.service';
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-transaction-details',
  templateUrl: './transaction-details.component.html',
  styleUrls: ['./transaction-details.component.scss']
})
export class TransactionDetailsComponent implements OnInit,AfterViewInit {
  id: string; 
  transactionInfo: any;
  user: AuthUser; 
   isLoadingResults: boolean = true;
 
  constructor(
    private route: ActivatedRoute,
    private dataService: DataService,
    private toastrService: ToastrService,
    private router: Router,
    private authService: AuthService
  ) {
    this.user = this.authService.currentUserValue;
 
  }

  ngOnInit(): void {
  }


  ngAfterViewInit() {
    this.route.params.subscribe(p => {
      this.id = p.id;
      this.getInfo();
    });
  }





  getInfo() {
   
    this.isLoadingResults = true;
    return this.dataService.get(ServerApis.getTransaction, { id: this.id }).subscribe(response => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) { 
            this.transactionInfo = response.data ? response.data : {};
           } else {
            let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
            this.toastrService.error(msg);
          }
        },error=> {
          this.isLoadingResults = false; 
        });
  }


 




 

  back() {
    window.history.back();
  }

   

}
