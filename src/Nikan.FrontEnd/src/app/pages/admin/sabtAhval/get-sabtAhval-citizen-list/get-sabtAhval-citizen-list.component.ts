import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import Swal from 'sweetalert2';
import { ServerApis } from '@core/server-apis';
import { MatTableDataSource } from '@angular/material/table';
import { CitizenProfileDialogComponent } from '../../../../shared/_dialog/citizen-profile/citizen-profile.component';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-sabtAhval-citizen-list',
  templateUrl: './get-sabtAhval-citizen-list.component.html',
  styleUrls: ['./get-sabtAhval-citizen-list.component.scss'],
  standalone: false,
})
export class AdminSabtAhvalCitizensListComponent extends AppBase implements AfterViewInit, OnInit {
  id: number;

  displayedColumns: string[] = [
    'row',
    'checkbox',
    'firstName',
    'lastName',
    'fatherName',
    'nationCode',
    'birthDate',
    'verifyDate',
    'sabtStatus',
  ];

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  searchForm: FormGroup;

  sendingSms: boolean = false;
  selectAll: boolean = false;
  constructor(private customValidator: CustomFormValidators) {
    super();
    this.route.params.subscribe((p) => {
      this.id = p['id'];
    });
    this.searchForm = this.fb.group({
      title: [null],
    });
  }

  ngOnInit() {}

  ngAfterViewInit() {
    this.getList();
  }

  getList() {
    this.isLoadingResults = true;
    var param: any = {
      offset: 0,
      count: 10000,
      exportId: this.id,
    };

    this.data = [];
    this.dataService.get(ServerApis.getAllCitizenExported, param).subscribe(
      (response) => {
        this.isLoadingResults = false;
        if (response.isSuccess && response.data) {
          this.data = response.data.items ? response.data.items : [];
          this.dataSource.data = this.data;
          this.listCount = response.data.totalItems ? response.data.totalItems : 0;
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

  applyFilter() {
    this.dataSource.filter = this.searchForm.get('title')?.value;
  }

  sendcitizenForAuthentication(citizenId) {
    this.dataService
      .get(ServerApis.citizenForAuthenticationByCitizenId, { citizenId: citizenId })
      .subscribe(
        (response) => {
          if (response.isSuccess && response.data) {
            this.toastrService.success(response.messages);
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {},
      );
  }

  selectUnselectAll() {
    for (var i = 0; i < this.data.length; i++) {
      this.data[i].selected = this.selectAll;
    }
  }

  sendSms() {
    var selectedIds = [];
    for (var i = 0; i < this.data.length; i++) {
      if (this.data[i].selected && this.data[i].sabtStatus == 0)
        selectedIds.push(this.data[i].citizenId);
    }

    if (selectedIds.length == 0) {
      this.toastrService.warning(
        'رکورد هایی که می خواهید برای آن ها پیامک ارسال شود را انتخاب کنید.',
      );
      return;
    }

    Swal.fire({
      title: 'ارسال پیامک',
      text:
        'شما ' +
        selectedIds.length +
        ' مورد برای ارسال پیامک انتخاب کرده اید. برای ارسال پیامک اطمینان دارید؟',
      showCancelButton: true,
      showConfirmButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.isConfirmed) {
        this.sendingSms = true;
        this.dataService
          .post(ServerApis.sendSabtAhvalCitizensSms, {
            ExportId: +this.id,
            Ids: selectedIds,
          })
          .subscribe(
            (response) => {
              this.sendingSms = false;
              if (response.isSuccess) {
                Swal.fire({
                  title: 'پیامک با موفقیت ارسال شد',
                  text: response.messages,
                });
                this.toastrService.success('پیامک با موفقیت ارسال شد.');
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            },
            (error) => {
              this.sendingSms = false;
              this.toastrService.error('متاسفانه خطایی در سرور رخ داده است!');
            },
          );
      }
    });
  }

  openCitizenProfile(row:any) {
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
