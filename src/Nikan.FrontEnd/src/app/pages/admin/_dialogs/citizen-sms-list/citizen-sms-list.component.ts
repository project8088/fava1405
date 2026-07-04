import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-sms-list',
  templateUrl: './citizen-sms-list.component.html',
  styleUrls: ['./citizen-sms-list.component.scss'],
  standalone: false,
})
export class AdminCitizenSmsListDialogComponent extends AppBase implements OnInit {
  citizen: any;
  data: any[] = [];

  constructor(@Inject(MAT_DIALOG_DATA) private _data: any) {
    super();
    this.citizen = _data;
  }

  ngOnInit(): void {
    this.getMessages();
  }

  getMessages() {
    this.dataService
      .get(ServerApis.getCitizenRejectImageSmsList, { citizenId: this.citizen.citizenId })
      .subscribe((data) => {
        this.data = data.data;
      });
  }
}
