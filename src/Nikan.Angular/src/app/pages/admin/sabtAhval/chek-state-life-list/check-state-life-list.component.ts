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
  selector: 'app-check-state-life-list',
  templateUrl: './check-state-life-list.component.html',
  styleUrls: ['./check-state-life-list.component.scss']
})
export class AdminCheckStateLifeListComponent implements AfterViewInit, OnInit {
  id: number;

  displayedColumns: string[] = ['row',   'firstName', 'lastName', 'fatherName', 'nationCode', 'birthDate', 'verifyDate','sabtStatus'];


  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  searchForm: FormGroup;


  sendingSms: boolean = false;
   
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



  checkIsDead(item) {
    item.loading = true;
    this.dataService.get(
      ServerApis.checkIsDead, { nationCode: item.nationCode }).subscribe(response => {
        item.loading = false;
        if (response && response.isSuccess) {
          this.toastrService.success(response.messages);
          this.getList();
        } else {
          let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
          this.toastrService.error(msg);
        }
      }, error => {
        item.loading = false;
      });
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


