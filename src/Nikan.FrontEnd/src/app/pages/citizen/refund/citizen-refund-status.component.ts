import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { map, shareReplay } from 'rxjs/operators';

import { ActivatedRoute } from '@angular/router';
import { DataService } from '../../../core/services/data-service.service';
import { Observable } from 'rxjs';
import { ServerApis } from '../../../core/server-apis';
import { ShortKarjoProfile } from '@core/models/citizen/global-information';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-citizen-refund-status',
  templateUrl: './citizen-refund-status.component.html',
  styleUrls: ['./citizen-refund-status.component.scss'],
})
export class CitizenRefundStatusComponent implements OnInit {
  userId: string;
  loading: boolean;
  personInfo: ShortKarjoProfile;
  baseUrl: string = ServerApis.baseUrl;

  constructor(
    private breakpointObserver: BreakpointObserver,
    private route: ActivatedRoute,
    private dataService: DataService,
    private toastrService: ToastrService,
  ) {}
  ngOnInit(): void {}
}
