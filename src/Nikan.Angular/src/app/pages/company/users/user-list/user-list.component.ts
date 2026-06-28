import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
 import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
 import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import {  CompanyUpdateUserDialogComponent } from '../dialogs/update-user/update-user.component';
import {  CompanyChangePasswordDialogComponent } from '../dialogs/change-user-password/change-user-password.component';
import {  CompanyAddUserDialogComponent } from '../dialogs/add-user/add-user.component';
import { AuthUser } from '../../../../core/authentication/user.model';
import { AuthService } from '../../../../core/authentication/auth.service';
@Component({
  selector: 'company-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class CompanyUserListComponent implements  OnInit {
 
  displayedColumns: string[] = ['row', 'userName', 'displayName', 'emailAddress','mobileNumber', 'roles','userAccountState', 'createdOnDate', 'lastLoggedIn', 'operation'];
 
  companyId: string = '';


  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;

  groupList: any[] = [];
  user: AuthUser;

  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private router: Router,
    private fb: FormBuilder,
    private customValidator: CustomFormValidators,
    private route: ActivatedRoute,
    private authService: AuthService

  ) {
    this.user = this.authService.currentUserValue;
    this.route.params.subscribe(p => {
      this.companyId = p.id;
    });
    this.searchForm = this.fb.group({
      query: [null, []]
    });


     

  }



  ngOnInit() {
    this.getList();
  }

 







  getList() {
    this.isLoadingResults = true;
    this.data = [];
    var param:any = {};
    if (this.companyId && this.companyId != '0')
      param.companyId = this.companyId;
    else
      param.companyId = null;


    this.dataService.get(ServerApis.getCompanyUsers, param).subscribe(response => {
      this.isLoadingResults = false;
      if (response.isSuccess) {
        this.data = response.data ? response.data : [];
        this.dataSource.data = this.data;
        this.listCount = this.data.length;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;

      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
      this.isLoadingResults = false;

    });


  }


  pageEvent(event: PageEvent) {
    this.getList();
  }


  applyFilter() {
    this.dataSource.filter = this.searchForm.get('query').value;

  }


  openAddUserDialog() {
    this.matDialog.open(CompanyAddUserDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        companyId: this.companyId
      }
    }).afterClosed().subscribe(result => {
      if (result)
        this.getList();
    });
  }


  openUpdateUserDialog(row) {
    this.matDialog.open(CompanyUpdateUserDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        userId: row.userId,
        companyId: this.companyId

      }
    }).afterClosed().subscribe(result => {
      if (result)
        this.getList();
    });
  }

  openChangePasswordDialog(row) {
    this.matDialog.open(CompanyChangePasswordDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        userId: row.userId,
        userName: row.userName,
        displayName: row.displayName
      }
    });
  }




  back() {
    this.router.navigate(['/admin/companies']);
  }




}
