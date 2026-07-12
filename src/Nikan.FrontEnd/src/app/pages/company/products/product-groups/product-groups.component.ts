import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { FormGroup } from '@angular/forms';
import Swal from 'sweetalert2';
import { ServerApis } from '@core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { CompanyAddUpdateProductGroupDialogComponent } from '../_dialogs/add-update-product-group/add-update-product-group.component';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'company-product-groups',
  templateUrl: './product-groups.component.html',
  styleUrls: ['./product-groups.component.scss'],
  standalone: false,
})
export class CompanyProductGroupsListComponent extends AppBase implements OnInit, AfterViewInit {
  loading?: boolean;
  displayedColumns: string[] = ['row', 'name', 'parent', 'createdBy', 'isActive', 'operation'];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  searchForm: FormGroup;
  events: any[] = [];
  constructor() {
    super();
    this.searchForm = this.fb.group({
      title: [''],
    });
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.getList();
  }

  getList() {
    this.isLoadingResults = true;
    this.data = [];
    this.dataService
      .get(ServerApis.getAllCompanyProductGroups, {})
      .pipe(
        finalize(() => {
          this.isLoadingResults = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response.isSuccess) {
          this.data = response.data ? response.data : [];
          this.dataSource.data = this.data;
          this.listCount = this.data.length;
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      });
  }

  pageEvent(event: PageEvent) {
    this.getList();
  }

  applyFilter() {
    this.dataSource.filter = this.searchForm.get('title')?.value;
  }

  delete(row: any) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا می خواهید "' + row.name + '" را حذف کنید؟',
      showConfirmButton: true,
      confirmButtonText: 'بله',
      showCancelButton: true,
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService.get(ServerApis.removeProductGroup, { id: row.id }).subscribe(
          (response) => {
            if (response.isSuccess) {
              this.toastrService.success('حذف اطلاعات با موفقیت انجام شد.');
              this.getList();
            } else {
              let msg = response.messages
                ? response.messages
                : 'متاسفانه خطایی در سرور رخ داده است!';
              this.toastrService.error(msg);
            }
          },
          (error: any) => {
            this.toastrService.error('حذف اطلاعات با خطا مواجه شده است!');
          },
        );
      }
    });
  }

  openProductGroupDialog(row: any) {
    this.matDialog
      .open(CompanyAddUpdateProductGroupDialogComponent, {
        panelClass: 'custom-dialog',
        data: {
          item: row,
        },
      })
      .afterClosed()
      .subscribe((response) => {
        if (response) this.getList();
      });
  }
}
