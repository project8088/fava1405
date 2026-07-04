import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, merge, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { AuthService } from '@core/authentication/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { DataService } from '../../../core/services/data-service.service';
import { ServerApis } from '../../../core/server-apis';

@Component({
  selector: 'app-citizen-event-list',
  templateUrl: './citizen-event-list.component.html',
  styleUrls: ['./citizen-event-list.component.scss'],
})
export class AppCitizenEventListComponent implements OnInit {
  search: string = '';
  paging: any = {};
  userCode: string;
  events: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  searchForm: FormGroup;
  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private router: Router,
    private matDialog: MatDialog,
    private fb: FormBuilder,
    private route: ActivatedRoute,
  ) {
    this.route.params.subscribe((p) => {
      this.userCode = p.id ? p.id : null;
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
