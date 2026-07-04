 
  
import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { userGroupsDto } from 'src/app/core/models/users/userGroups';
import Swal from 'sweetalert2';
 
import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';



@Component({
  selector: 'adm-manzalat-base-list',
    templateUrl: './manzalat-base-list.component.html',
    styleUrls: ['./manzalat-base-list.component.scss']
})


export class AdminManzalatBaseFromListComponent implements OnInit {
 
  userGroupList: userGroupsDto[] = [];
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  loading: boolean = true;

  groupList: any[] = [];

  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private router: Router,
    private fb: FormBuilder,
    private customValidator: CustomFormValidators,
    private route: ActivatedRoute,

  ) {

   

  }



  ngOnInit() {
    this.getList();
  }

   
 

  getList() {
    this.loading = true;
      this.dataService.get(ServerApis.getManzalatBaseForms, { }).subscribe(response => {
      this.loading = false;
      if (response && response.isSuccess) {
        this.userGroupList = response.data ? response.data : [];
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
      this.loading = false;
      this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
    });
  }
   
 
  

 

}
