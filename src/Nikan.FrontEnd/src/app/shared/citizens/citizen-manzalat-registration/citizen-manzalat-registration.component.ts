import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import Swal from 'sweetalert2';
import { DataService } from '../../../core/services/data-service.service';
import { ServerApis } from '../../../core/server-apis';
@Component({
  selector: 'app-citizen-manzalat-registration',
  templateUrl: './citizen-manzalat-registration.component.html',
  styleUrls: ['./citizen-manzalat-registration.component.scss'],
})
export class AppCitizenManzalatRegistrationComponent implements AfterViewInit {
  userCode: string;
  baseUrl: string = ServerApis.baseUrl;

  loading: boolean;
  data: any[] = [];
  dataSource = new MatTableDataSource();
  listCount: number = 0;
  isLoadingResults: boolean = true;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private router: Router,
    private dataService: DataService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
  ) {
    this.route.params.subscribe((p) => {
      this.userCode = p.id ? p.id : null;
    });
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.getManzalatInfo();
  }

  getManzalatInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getCitizenInfoAndManzaltForm, {
        userCode: this.userCode,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess) {
            this.data = response.data ? response.data.manzaltForms : {};
          }
        },
        (error) => {
          this.loading = false;
        },
      );
  }
}
