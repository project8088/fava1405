import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-event-list',
  templateUrl: './citizen-event-list.component.html',
  styleUrls: ['./citizen-event-list.component.scss'],
  standalone: false,
})
export class AppCitizenEventListComponent extends AppBase implements OnInit {
  search: string = '';
  paging: any = {};
  userCode: string='';
  events: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  constructor() {
    super();
    this.route.params.subscribe((p) => {
      this.userCode = p['id'] ? p['id'] : null;
      this.getListevents();
    });
  }

  ngOnInit() {}

  ngAfterViewInit() {}

  getListevents() {
    this.dataService.get(ServerApis.getCitizenTopEvent, { userCode: this.userCode }).subscribe(
      (response) => {
        if (response.isSuccess) {
          this.events = response.data ? response.data : [];
        }
      },
      (error) => {},
    );
  }
}
