import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { map, shareReplay } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ServerApis } from '../../../core/server-apis';
import { ShortKarjoProfile } from '@core/models/citizen/global-information';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-citizen-refund-status',
  templateUrl: './citizen-refund-status.component.html',
  styleUrls: ['./citizen-refund-status.component.scss'],
    standalone: false
})
export class CitizenRefundStatusComponent extends AppBase implements OnInit {
  userId: string;
  loading: boolean;
  personInfo: ShortKarjoProfile;
  baseUrl: string = ServerApis.baseUrl;

  constructor(
    private breakpointObserver: BreakpointObserver
  ) {
      super();}
  ngOnInit(): void {}
}
