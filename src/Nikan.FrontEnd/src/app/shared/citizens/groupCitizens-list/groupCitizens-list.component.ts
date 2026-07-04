import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { merge, of as observableOf } from 'rxjs';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ServerApis } from '../../../core/server-apis';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-groupCitizens-list',
  templateUrl: './groupCitizens-list.component.html',
  styleUrls: ['./groupCitizens-list.component.scss'],
    standalone: false
})
export class AppGroupCitizensListComponent extends AppBase implements AfterViewInit, OnInit {
  displayedColumns: string[] = ['row', 'group', 'citizen', 'creationDate', 'addByUser'];
  userCode: string;
  searchForm: FormGroup;

  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
) {
      super();
    this.route.params.subscribe((p) => {
      this.userCode = p.id ? p.id : null;
    });

    this.searchForm = this.fb.group({});
  }

  ngOnInit() {}
  ngAfterViewInit() {
    this.getList();
  }

  getList() {
    var param: any = this.searchForm.value;
    param.offset = this.paginator ? this.paginator.pageIndex : 0;
    param.count = this.paginator ? this.paginator.pageSize : 10;
    param.userCode = this.userCode;

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ServerApis.getPagedGroupsCitizensInfo, param);
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
        this.data = data;
      });
  }

  pageEvent(event: PageEvent) {
    this.getList();
  }
}
