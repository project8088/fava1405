import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '../../../../core/server-apis';
import Swal from 'sweetalert2';
import { CitizenProfileDialogComponent } from '../../../../shared/_dialog/citizen-profile/citizen-profile.component';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-refund-excel-batch-file-details',
  templateUrl: './refund-excel-batch-file-details.component.html',
  styleUrls: ['./refund-excel-batch-file-details.component.scss'],
    standalone: false
})
export class AdminRefundExcelBatchFileDetailsComponent extends AppBase implements AfterViewInit {
  loading: boolean;
  isSaving: boolean;

  displayedColumns: string[] = [
    'row',
    'citizen',
    'nationalCode',
    'orderId',
    'saleReferenceId',
    'refundAmount',
    'totalRefundAmount',
    'refundCardNumber',
  ];
  showAddPanel: boolean;
  importId: string;
  info: any = {};
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchForm: FormGroup;
  frm: FormGroup;
  events: any[] = [];
  constructor(
    private customValidator: CustomFormValidators,
  ) {
      super();
    this.frm = fb.group({
      unitName: [null, [Validators.required]],
      className: ['', [Validators.required]],
      /* nationalCode: [null,   this.customValidator.checkNationalCode],*/
      letterNumber: [''],
      description: [null],
      citizenAccess: [true],
      importId: [0],
      citizenId: [null],
    });

    this.searchForm = this.fb.group({
      title: [''],
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe((p) => {
      this.importId = p.importId;
      this.getList();
    });
  }

  ngAfterViewInit() {}

  getList() {
    this.isLoadingResults = true;
    this.data = [];
    this.dataService.get(ServerApis.refundImportFileDetails, { importId: this.importId }).subscribe(
      (response) => {
        this.isLoadingResults = false;
        if (response.isSuccess && response.data) {
          this.info = response.data;
          this.data = response.data.refundList ? response.data.refundList : [];
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
    this.dataSource.filter = this.searchForm.get('title').value;
  }

  getCardNumber() {
    Swal.fire({
      title: 'تائید',
      text: 'آیا برای استعلام  فایل اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService
          .get(ServerApis.getCardsNumber, {
            importId: this.importId,
          })
          .subscribe(
            (response) => {
              if (response.isSuccess) {
                this.toastrService.success('با موفقیت استعلام شد.');
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

  save() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return false;
    }

    this.frm.get('importId').setValue(+this.importId);
    let form = this.frm.value;

    form.citizenId = +form.citizenId.key;

    this.isSaving = true;
    this.dataService.post(ServerApis.addRefundAccess, form).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.showAddPanel = false;
          this.frm.reset();
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

  delete() {
    Swal.fire({
      title: 'حذف',
      text: 'آیا می خواهید "' + this.importId + '" را حذف کنید؟',
      showConfirmButton: true,
      confirmButtonText: 'بله',
      showCancelButton: true,
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.dataService
          .get(ServerApis.removeRefundImportFile, { importId: this.importId })
          .subscribe(
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

  openCitizenProfile(row) {
    this.matDialog.open(CitizenProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        userCode: row.userCode,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }
}
