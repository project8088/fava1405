import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '../../../../core/server-apis';
import Swal from 'sweetalert2';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-news-groups',
  templateUrl: './news-groups.component.html',
  styleUrls: ['./news-groups.component.scss'],
    standalone: false
})
export class AdminNewsGroupsComponent extends AppBase implements AfterViewInit {
  displayedColumns: string[] = ['row', 'title', 'description', 'isActive', 'operation'];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;

  frm: FormGroup;
  showAddOrUpdatePanel: boolean;
  isSaving: boolean;

  constructor(
) {
      super();
    this.frm = fb.group({
      id: [null],
      title: [null, [Validators.required]],
      description: [null],
      isActive: [true],
    });

    this.searchForm = this.fb.group({
      query: [null, []],
    });
  }

  ngAfterViewInit() {
    this.getList();
  }

  getList() {
    this.isLoadingResults = true;
    this.data = [];
    this.dataService.get(ServerApis.getAllNewGroups, {}).subscribe(
      (response) => {
        this.isLoadingResults = false;
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
      },
      (error) => {
        this.isLoadingResults = false;
      },
    );
  }

  pageEvent(event: PageEvent) {
    this.getList();
  }

  applyFilter() {
    this.dataSource.filter = this.searchForm.get('query').value;
  }

  delete(row) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا برای حذف "' + row.title + '" اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService
          .get(ServerApis.removeNewsGroups, {
            id: row.id,
          })
          .subscribe(
            (response) => {
              if (response.isSuccess) {
                this.toastrService.success('با موفقیت حذف شد.');
                this.getList();
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            },
            (error) => {},
          );
      }
    });
  }

  update(row) {
    this.frm.setValue({
      id: row.id,
      title: row.title,
      description: row.description,
      isActive: row.isActive,
    });
    this.showAddOrUpdatePanel = true;
    window.scrollTo(0, 0);
  }

  save() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return false;
    }
    this.isSaving = true;
    this.dataService.post(ServerApis.addOrUpdateNewsGroup, this.frm.value).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.showAddOrUpdatePanel = false;
          this.frm.reset();
          this.frm.get('isActive').setValue(true);

          this.getList();
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.isSaving = false;
      },
    );
  }

  /**
   *  افزودن کارفرمای جدید
   * */
  //openNewEmployementDialog(item) {
  //  this.matDialog.open(PlacementAddNewEmployerDialogComponent, {
  //    panelClass: 'custom-dialog',
  //    minWidth: '600px',
  //    data: {
  //      id: (item) ? item.id : ''
  //    }
  //  }).afterClosed().subscribe(result => {
  //    if (result) {
  //      //this.router.navigate(['/placement/employer-list']);
  //      this.applyFilter();
  //    }
  //  });
  //}
}
