import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { CardAddUserDialogComponent } from '../dialogs/add-user/add-user.component';
import Swal from 'sweetalert2';
@Component({
  selector: 'adm-card-users',
  templateUrl: './card-users.component.html',
  styleUrls: ['./card-users.component.scss']
})
export class CardUsersComponent implements  OnInit {
 
  displayedColumns: string[] = ['row', 'userName', 'displayName', 'emailAddress','userAccountState', 'createdOnDate', 'lastLoggedIn', 'operation'];
 

  roleList: any[] = [];
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;

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

    this.searchForm = this.fb.group({
      fromDate: [null],
      toDate: [null],
      username: [''],
      roleId: [null],

    });
     

     

  }



  ngOnInit() { 
    this.getList();

  }

 







  getList() {
    var param: any = this.searchForm.value;
    param.offset = (this.paginator) ? this.paginator.pageIndex : 0;
    param.count = (this.paginator) ? this.paginator.pageSize : 10;
    if (param.fromDate)
      param.fromDate = this.dataService.formatDate(param.fromDate);
    if (param.toDate)
      param.toDate = this.dataService.formatDate(param.toDate);

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.searchCardUser, param);
        }),
        map(response => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.items ? response.data.items : [];
            this.listCount = response.data.totalItems ? response.data.totalItems : 0;
            // debugger; 
            return items;
          } else {
            let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
            this.toastrService.error(msg);
          }
        }),
        catchError((err) => {
          this.isLoadingResults = false;
          return observableOf([]);
        })
      ).subscribe(data => {
        this.data = data;
      });



  }


  pageEvent(event: PageEvent) {
    this.getList();
  }

  applyFilter() {
    if (this.paginator) {
      this.paginator.firstPage();
    }
    this.getList();
  }



  openAddUserDialog() {
    this.matDialog.open(CardAddUserDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
      }
    }).afterClosed().subscribe(result => {
      if (result)
        this.getList();
    });
  }

  delete(row) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا می خواهید "' + row.displayName + '" را حذف کنید؟',
      showConfirmButton: true,
      confirmButtonText: 'بله',
      showCancelButton: true,
      cancelButtonText: 'خیر'
    }).then(result => {
      if (result.value) {
        this.dataService.get(ServerApis.deleteCardUser, { userId: row.userId }).subscribe(response => {
          if (response.isSuccess) {
            this.toastrService.success('حذف اطلاعات با موفقیت انجام شد.');
            this.getList();
          } else {
            let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
            this.toastrService.error(msg);
          }
        }, error => {
          this.toastrService.error('حذف اطلاعات با خطا مواجه شده است!');
        });
      }
    });
  }
  
}
