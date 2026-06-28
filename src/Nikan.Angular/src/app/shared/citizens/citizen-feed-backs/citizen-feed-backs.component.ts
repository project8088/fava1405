import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ToastrService } from 'ngx-toastr'; 
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms'; 
import { MatTableDataSource } from '@angular/material/table';
import Swal from 'sweetalert2';
import { DataService } from '../../../core/services/data-service.service';
import { ServerApis } from '../../../core/server-apis';
@Component({
    selector: 'app-citizen-feed-back-list',
    templateUrl: './citizen-feed-backs.component.html',
    styleUrls: ['./citizen-feed-backs.component.scss']
})
export class AppCitizenFeedBackListComponent implements AfterViewInit {
 
  listfeedback: any[] = [];
  isSavingfeedback: boolean;
  groupfeedbackList: any[] = []; 
    userCode: string;
    loadingfeedback: boolean;
    data: any[] = [];
    dataSource = new MatTableDataSource();
    listCount: number = 0;
    isLoadingResults: boolean = true;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
  feedbackfrm: FormGroup;
     constructor(
      private toastrService: ToastrService,
      private matDialog: MatDialog,
      private router: Router,
      private dataService: DataService,
       private fb: FormBuilder,
       private route: ActivatedRoute
    ) {
       this.feedbackfrm = this.fb.group(
         {
           feedbackDescription: [null, [Validators.required]],
           feedbackId: [null, [Validators.required]],
         });

       this.getBaseListFeedbacke();

       this.route.params.subscribe(p => {
         this.userCode = p.id ? p.id:null;
       });

    }

    ngOnInit(): void {

    }

    ngAfterViewInit() {
      this.getListFeedbacks();
    }



  getListFeedbacks() {
    this.loadingfeedback = true;
    this.listfeedback = [];
    this.dataService.get(ServerApis.getAllCitizenFeedbacks, { userCode: this.userCode }).subscribe(response => {
      this.loadingfeedback = false;
      if (response.isSuccess) {
        this.listfeedback = response.data ? response.data : [];

      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
        this.loadingfeedback = false;

    });
  }
  savefeedback() {
    if (this.feedbackfrm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.feedbackfrm.markAllAsTouched();
      return false;
    }

    var formValue = this.feedbackfrm.value;
    this.isSavingfeedback = true;
    this.dataService.post(ServerApis.addFeedbacke, {
      userCode: this.userCode,
      feedbackId: formValue.feedbackId,
      feedbackDescription: formValue.feedbackDescription ? formValue.feedbackDescription : ''
    }).subscribe(response => {
      this.isSavingfeedback = false;
      if (response.isSuccess) {
        this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
        this.getListFeedbacks();
      } else {
        var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
        this.toastrService.error(msg);
      }
    }, error => {
        this.isSavingfeedback = false;
    });
  }
  getBaseListFeedbacke() {

    this.dataService.get(ServerApis.getBaseListFeedbacke).subscribe(response => {
      if (response.isSuccess)
        this.groupfeedbackList = response.data ? response.data : [];
      else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }

    });
  }

 
    pageEvent(event: PageEvent) {
      this.getListFeedbacks();
    }

   
     


}


