import { Component, OnInit, AfterViewInit, ViewChild, Input } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import * as moment from 'jalali-moment';
import { SendTicketDialogComponent } from '../_dialogs/send-ticket/send-ticket.component';
import { AuthUser } from '../../../core/authentication/user.model';
import { ServerApis } from '../../../core/server-apis';
import { FormGroup } from '@angular/forms';

import { merge, of } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.scss'],
    standalone: false
})
export class TicketListComponent extends AppBase implements OnInit, AfterViewInit {
  introductionInfo: any = {};
  displayedColumns: string[];
  data: any[] = [];
  dataSource = new MatTableDataSource();
  resultsLength: number = 0;
  isLoadingResults: boolean = true;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  currentUser: AuthUser;
  rootModule: string;

  searchForm: FormGroup;
  ticketStatus: any[] = [];

  constructor(
) {
      super();
    this.searchForm = this.fb.group({
      ticketStatus: [null],
      iscolsed: [null],
      isSolved: [null],
      fromDate: [null],
      toDate: [null],
      ownerName: [''],
      title: [''],
    });

    if (this.authService.currentUserValue) {
      this.currentUser = this.authService.currentUserValue;
    }
  }

  ngOnInit(): void {
    this.dataService.getEnums().subscribe((response) => {
      this.ticketStatus = response.ticketStatus;
    });

    this.rootModule = this.authService.currentUserValue.rootModule;

    if (this.currentUser.isAdmin)
      this.displayedColumns = [
        'row',
        'code',
        'organName',
        'fullName',
        'subject',
        'priority',
        'organizationalUnit',
        'createdOn',
        'ticketStatus',
        'operation',
      ];
    else
      this.displayedColumns = [
        'row',
        'code',
        'fullName',
        'subject',
        'priority',
        'organizationalUnit',
        'createdOn',
        'ticketStatus',
        'operation',
      ];
  }

  ngAfterViewInit() {
    this.getList();
  }

  getList() {
    var param: any = this.searchForm.value;
    param.offset = this.paginator ? this.paginator.pageIndex : 0;
    param.count = this.paginator ? this.paginator.pageSize : 10;
    if (param.fromDate) param.fromDate = this.dataService.formatDate(param.fromDate);
    if (param.toDate) param.toDate = this.dataService.formatDate(param.toDate);
    console.log('CONSOLOG: M:paginateVar & O: this.var : ', this.currentUser);
    let ticketUrl = ServerApis.getUserTicketsList;
    if (this.currentUser.isAdmin) ticketUrl = ServerApis.getAdminTicketsList;
    else if (this.currentUser.isCompany) ticketUrl = ServerApis.getCompanyTicketsList;
    else if (this.currentUser.isCardUser) ticketUrl = ServerApis.getCardTicketsList;

    merge()
      .pipe(
        startWith(param),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.get(ticketUrl, param);
        }),
        map((response) => {
          this.isLoadingResults = false;
          if (response.isSuccess && response.data) {
            var items = response.data.tickets ? response.data.tickets : [];
            this.resultsLength = response.data.totalItems ? response.data.totalItems : 0;

            return items;
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        }),
        catchError((err) => {
          this.isLoadingResults = false;
          this.toastrService.error('دريافت اطلاعات از سرور با خطا مواجه شده است!');
          return of([]);
        }),
      )
      .subscribe((data) => {
        this.data = data;
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

  viewTicket(row) {
    this.router.navigate(['/' + this.currentUser.rootModule + '/ticket-details/' + row.id]);
  }

  openNewTicketDialog() {
    this.matDialog
      .open(SendTicketDialogComponent, {
        panelClass: 'custom-dialog',
        minWidth: '70%',
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) this.getList();
      });
  }
}
