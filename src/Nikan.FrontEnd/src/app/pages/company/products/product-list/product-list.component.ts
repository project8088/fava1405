import { Component, OnInit, AfterViewInit, ViewChild, Input } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
 import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router'; 
import { FormBuilder, FormGroup } from '@angular/forms';
import Swal from 'sweetalert2'; 
import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
@Component({
  selector: 'company-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class CompanyProductListComponent implements OnInit, AfterViewInit {
  loading: boolean;
  displayedColumns: string[] = ['row', 'imageUrl','code', 'title', 'description', 'price', 'isActive', 'operation'];

  baseUrl: string = ServerApis.baseUrl;


  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;
  events: any[] = [];
  constructor(
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private router: Router,
    private dataService: DataService,
    private fb: FormBuilder,
  ) {
    this.searchForm = this.fb.group({
      title: [''],
    });


  }

  ngOnInit(): void {

  }

  ngAfterViewInit() {
    this.getList();
  }





  getList() {
    this.isLoadingResults = true;
    this.data = [];
    this.dataService.get(ServerApis.getAllCompanyProducts, {}).subscribe(response => {
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
    this.dataSource.filter = this.searchForm.get('title').value;

  }


 


  delete(row) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا می خواهید "' + row.title + '" را حذف کنید؟',
      showConfirmButton: true,
      confirmButtonText: 'بله',
      showCancelButton: true,
      cancelButtonText: 'خیر'
    }).then(result => {
      if (result.value) {
        this.dataService.get(ServerApis.removeCompanyProduct, { id: row.id }).subscribe(response => {
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
