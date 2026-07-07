import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import Swal from 'sweetalert2';
import { ServerApis } from '../../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { AdminImportNationCodeGroupsExcelDialogComponent } from '../../import-Excel-Import/dialog/nationCode-groups-import-excel/import-nationCode-groups-excel.component';
import { AdminGroupTransferDialogComponent } from '../_dialogs/group-transfer/group-transfer.component';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.scss'],
  standalone: false,
})
export class AdminGroupListComponent extends AppBase implements AfterViewInit, OnInit {
  displayedColumns: string[] = [
    'row',
    'id',
    'groupName',
    'parent',
    'countCitizen',
    'countQueue',
    'creationDate',
    'isActive',
    'operation',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  searchForm: FormGroup;
  constructor(private customValidator: CustomFormValidators) {
    super();
    this.searchForm = this.fb.group({
      groupname: [''],
    });
  }

  ngOnInit() {}

  ngAfterViewInit() {
    this.getList();
  }

  getList() {
    var param: any = this.searchForm.value;
    param.offset = this.paginator ? this.paginator.pageIndex : 0;
    param.count = this.paginator ? this.paginator.pageSize : 10;

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.searchGroups, param);
        }),
        map((response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.items ? response.data.items : [];
            this.listCount = response.data.totalItems ? response.data.totalItems : 0;
            // debugger;
            return items;
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        }),
        catchError((err) => {
          this.isLoadingResults = false;
          return observableOf([]);
        }),
      )
      .subscribe((data) => {
       this.dataSource.data = data;
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

  delete(row:any) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا می خواهید "' + row.groupName + '" را حذف کنید؟',
      showConfirmButton: true,
      confirmButtonText: 'بله',
      showCancelButton: true,
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService.get(ServerApis.removeGroup, { id: row.id }).subscribe(
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
          (error) => {
            this.toastrService.error('حذف اطلاعات با خطا مواجه شده است!');
          },
        );
      }
    });
  }

  manageAttachment(item:any) {
    this.matDialog
      .open(AdminImportNationCodeGroupsExcelDialogComponent, {
        data: {
          groupId: item.id,
        },
        minWidth: '80%',
        panelClass: 'custom-dialog',
      })
      .afterClosed()
      .subscribe((response) => {
        this.getList();
      });
  }

  transfergroups(item:any) {
    this.matDialog
      .open(AdminGroupTransferDialogComponent, {
        data: {
          groupId: item.id,
        },
        minWidth: '80%',
        panelClass: 'custom-dialog',
      })
      .afterClosed()
      .subscribe((response) => {
        this.getList();
      });
  }

  reviewgroups(item:any) {
    item.loading = true;
    this.dataService.get(ServerApis.reviewGroups, { id: item.id }).subscribe(
      (response) => {
        item.loading = false;
        if (response && response.isSuccess) {
          this.toastrService.success('بازبینی گروه با موفقیت انجام گردید.');
          this.getList();
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        item.loading = false;
      },
    );
  }
}
