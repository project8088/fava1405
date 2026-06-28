import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { AuthService } from 'src/app/core/authentication/auth.service';
import Swal from 'sweetalert2';
import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { CitizenProfileDialogComponent } from '../../../../shared/_dialog/citizen-profile/citizen-profile.component';
@Component({
  selector: 'app-sabtAhval-citizen-list',
  templateUrl: './get-sabtAhval-citizen-list.component.html',
  styleUrls: ['./get-sabtAhval-citizen-list.component.scss']
})
export class AdminSabtAhvalCitizensListComponent implements AfterViewInit, OnInit {
  id: number;

  displayedColumns: string[] = ['row', 'checkbox', 'firstName', 'lastName', 'fatherName', 'nationCode', 'birthDate', 'verifyDate','sabtStatus'];


  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  searchForm: FormGroup;


  sendingSms: boolean = false;
  selectAll: boolean = false;
  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private customValidator: CustomFormValidators
  ) {
    this.route.params.subscribe(p => {
      this.id = p.id;
    });
    this.searchForm = this.fb.group({
      title: [null],
    });



  }


  ngOnInit() {

  }

  ngAfterViewInit() {
    this.getList();
  }
   


  getList() {
    this.isLoadingResults = true;
    var param: any = {
      offset: 0,
      count: 10000,
      exportId: this.id
    };

    this.data = [];
    this.dataService.get(ServerApis.getAllCitizenExported, param).subscribe(response => {
      this.isLoadingResults = false;
      if (response.isSuccess && response.data) {
        this.data = response.data.items ? response.data.items : []; 
        this.dataSource.data = this.data;
        this.listCount = response.data.totalItems ? response.data.totalItems : 0;

      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
      this.isLoadingResults = false;

    });


  }




  applyFilter() {
    this.dataSource.filter = this.searchForm.get('title').value;
  }



  sendcitizenForAuthentication(citizenId) {  
    this.dataService.get(ServerApis.citizenForAuthenticationByCitizenId, { citizenId: citizenId}).subscribe(response => {
      if (response.isSuccess && response.data) {
        this.toastrService.success(response.messages); 
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
      
    }); 
  }



  selectUnselectAll() {
    for (var i = 0; i < this.data.length; i++) {
      this.data[i].selected = this.selectAll;
    }
  }


  sendSms() {
    var selectedIds = [];
    for (var i = 0; i < this.data.length; i++) { 
      if (this.data[i].selected && this.data[i].sabtStatus==0 )
        selectedIds.push(this.data[i].citizenId);
    }

    if (selectedIds.length == 0) {
      this.toastrService.warning('رکورد هایی که می خواهید برای آن ها پیامک ارسال شود را انتخاب کنید.');
      return;
    }

    Swal.fire({
      title: 'ارسال پیامک',
      text: 'شما ' + selectedIds.length + ' مورد برای ارسال پیامک انتخاب کرده اید. برای ارسال پیامک اطمینان دارید؟',
      showCancelButton: true,
      showConfirmButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر'
    }).then(result => {
      if (result.isConfirmed) {


        this.sendingSms = true;
        this.dataService.post(ServerApis.sendSabtAhvalCitizensSms, {
          ExportId: +this.id,
          Ids: selectedIds
        }).subscribe(response => {
          this.sendingSms = false;
          if (response.isSuccess) {
            Swal.fire({
              title: 'پیامک با موفقیت ارسال شد',
              text: response.messages
            });
            this.toastrService.success('پیامک با موفقیت ارسال شد.');
          } else {
            let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
            this.toastrService.error(msg);
          }
        }, error => {
          this.sendingSms = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است!');
        });
      }
    }
    );
  }


  openCitizenProfile(row) {
    this.matDialog.open(CitizenProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        userCode: row.userCode
      },
      width: '85%',
      maxWidth: '1800px'
    });
  }






}


