import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CustomFormValidators } from '../../../core/custom-validator/form-validation';
import { DataService } from '../../../core/services/data-service.service';
import { ServerApis } from '../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table'; 
import { CitizenProfileDialogComponent } from '../../../shared/_dialog/citizen-profile/citizen-profile.component';
@Component({
  selector: 'adm-citizens-authentication',
  templateUrl: './citizens-authentication.component.html',
  styleUrls: ['./citizens-authentication.component.scss'],
})
export class AdminCitizenAuthenticationComponent implements AfterViewInit {
  

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  loading: boolean = true;
  citizen: any = {};
 
  searchForm: FormGroup;
  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private router: Router,
    private fb: FormBuilder 
     
  ) {
    this.searchForm = this.fb.group({ 
      birthDate: [null], 
      nationCode: [''],
    });
  }

  ngAfterViewInit() {
    
  }

  getinfo() {
    this.loading = true;

    var param: any = this.searchForm.value; 
    if (param.birthDate)
      param.birthDate = this.dataService.formatDate(param.birthDate);
   
    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.loading = false;
          return this.dataService.get(ServerApis.citizenForAuthenticationByAdmin, param);
        }),
        map((response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.citizen = response.data ? response.data  : {};
          } else {
            let msg = response.messages
              ? response.messages
              : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        }),
        catchError((err) => {
          this.loading = false;
          return observableOf([]);
        })
      )
      .subscribe((data) => {
        
      });
  }

  openCitizenProfile(citizenId) {
    this.matDialog.open(CitizenProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        id: citizenId
      },
      width: '85%',
      maxWidth: '1800px'
    });
  } 
  




 

  

  
}
