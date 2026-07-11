import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import Swal from 'sweetalert2';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-ticket-subjects',
  templateUrl: './ticket-subjects.component.html',
  styleUrls: ['./ticket-subjects.component.scss'],
  standalone: false,
})
export class AdminTicketSubjectsComponent extends AppBase implements AfterViewInit, OnInit {
  displayedColumns: string[] = [
    'row',
    'organizationalUnit',
    'title',
    'description',
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

  frm: FormGroup;
  showAddOrUpdatePanel: boolean = false;
  isSaving = false;
  loadingUnit: boolean = false;
  loadingData?: boolean;
  organizationList: any[] = [];
  unitList: any[] = [];
  constructor() {
    super();
    this.frm = this.fb.group({
      id: [null],
      title: [null, [Validators.required]],
      organizationId: [null, [Validators.required]],
      organizationalUnitId: [null, [Validators.required]],
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

  ngOnInit() {
    this.getOrganizations();
  }

  getList() {
    this.isLoadingResults = true;
    this.data = [];
    this.dataService.get(ServerApis.getAllTicketSubject, {}).subscribe(
      (response) => {
        this.isLoadingResults = false;
        if (response.isSuccess) {
          this.data = response.data ? response.data : [];
          for (var i of this.data) {
            if (i.description) {
              let d = document.createElement('div');
              d.innerHTML = i.description;
              i.textDescription = d.innerText ? d.innerText.substring(0, 100) : '';
            }
          }

          this.dataSource.data = this.data;
          this.listCount = this.data.length;
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error: any) => {
        this.isLoadingResults = false;
      },
    );
  }

  getOrganizations() {
    this.loadingData = true;

    this.dataService.get(ServerApis.getAllOrganizational, {}).subscribe(
      (response) => {
        this.loadingData = false;
        if (response.isSuccess) {
          this.organizationList = response.data ? response.data : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error: any) => {
        this.loadingData = false;
      },
    );
  }

  getUnitsOfOrganization() {
    this.loadingUnit = true;

    this.dataService
      .get(ServerApis.getAllOrganizationalUnitByOrganId, {
        organId: this.frm.get('organizationId')?.value,
      })
      .subscribe(
        (response) => {
          this.loadingUnit = false;
          if (response.isSuccess) {
            this.unitList = response.data ? response.data : [];
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {
          this.loadingUnit = false;
        },
      );
  }

  pageEvent(event: PageEvent) {
    this.getList();
  }

  applyFilter() {
    this.dataSource.filter = this.searchForm.get('query')?.value;
  }

  addnewSubject() {
    this.showAddOrUpdatePanel = true;
    window.scrollTo(0, 0);
  }

  delete(row: any) {
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
          .get(ServerApis.removeTicketSubject, {
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
            (error: any) => {},
          );
      }
    });
  }

  update(row: any) {
    this.frm.setValue({
      id: row.id,
      title: row.title,
      description: row.description,
      organizationalUnitId: row.organizationalUnitId,
      organizationId: row.organizationId,
      isActive: row.isActive,
    });
    this.showAddOrUpdatePanel = true;
    window.scrollTo(0, 0);
  }

  save() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return;
    }
    this.isSaving = true;
    var params = this.frm.value;

    this.dataService.post(ServerApis.addOrUpdateTicketSubject, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.showAddOrUpdatePanel = false;
          this.frm.reset();
          this.frm.get('isActive')?.setValue(true);
          this.getList();
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error: any) => {
        this.isSaving = false;
      },
    );
  }
}
