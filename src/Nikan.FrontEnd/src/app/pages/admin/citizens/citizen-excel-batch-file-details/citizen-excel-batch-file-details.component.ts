import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-citizen-excel-batch-file-details',
  templateUrl: './citizen-excel-batch-file-details.component.html',
  styleUrls: ['./citizen-excel-batch-file-details.component.scss'],
  standalone: false,
})
export class AdminCitizenExcelBatchFileDetailsComponent extends AppBase implements AfterViewInit {
    loading?: boolean;
  isSaving=false;

  displayedColumns: string[] = [
    'row',
    'gender',
    'nationCode',
    'firstName',
    'lastName',
    'birthDate',
    'mobile',
    'serviceId',
    'groupId',
    'isValidRow',
    'description',
  ];
  showAddPanel: boolean;
  importId: string;
  info: any = {};
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  searchForm: FormGroup;
  frm: FormGroup;
  events: any[] = [];
  constructor(private customValidator: CustomFormValidators) {
    super();
    this.searchForm = this.fb.group({
      isValidRow: [null],
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
    var param: any = this.searchForm.value;
    param.offset = this.paginator ? this.paginator.pageIndex : 0;
    param.count = this.paginator ? this.paginator.pageSize : 10;
    param.importId = this.importId;

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.citizenImportFileDetails, param);
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
}
