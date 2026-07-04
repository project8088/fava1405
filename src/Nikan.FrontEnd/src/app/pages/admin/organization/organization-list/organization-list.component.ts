import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup, Validators } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import Swal from 'sweetalert2';
import { ServerApis } from '../../../../core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-organization-list',
  templateUrl: './organization-list.component.html',
  styleUrls: ['./organization-list.component.scss'],
    standalone: false
})
export class AdminOrganizationListComponent extends AppBase implements AfterViewInit {
  displayedColumns: string[] = [
    'row',
    'organizationName',
    'description',
    'isActive',
    'cardDistributionCenters',
    'supportCenters',
    'operation',
  ];

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
    private customValidator: CustomFormValidators
  ) {
      super();
    this.frm = fb.group({
      id: [null],
      organizationName: [null, [Validators.required]],
      description: [null],
      isActive: [true],
      cardDistributionCenters: [null],
      supportCenters: [null],
      indexOrder: [null, [Validators.required]],
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
    this.dataService.get(ServerApis.getAllOrganization).subscribe(
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

  save() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return false;
    }
    this.isSaving = true;
    this.dataService.post(ServerApis.addOrUpdateOrganization, this.frm.value).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.showAddOrUpdatePanel = false;
          this.frm.reset();
          this.frm.get('isActive').setValue(false);
          this.frm.get('cardDistributionCenters').setValue(false);
          this.frm.get('supportCenters').setValue(false);
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

  delete(row) {
    Swal.fire({
      title: 'حذف',
      text: 'آیا برای حذف "' + row.organizationName + '" اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService
          .get(ServerApis.removeOrganization, {
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
      organizationName: row.organizationName,
      description: row.description,
      indexOrder: row.indexOrder,
      isActive: row.isActive,
      cardDistributionCenters: row.cardDistributionCenters,
      supportCenters: row.supportCenters,
    });
    this.showAddOrUpdatePanel = true;
  }
}
