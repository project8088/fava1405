import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from 'src/app/core/server-apis';
import { DataService } from 'src/app/core/services/data-service.service';

@Component({
  selector: 'app-citizen-sms-list',
  templateUrl: './citizen-sms-list.component.html',
  styleUrls: ['./citizen-sms-list.component.scss'],
})
export class AdminCitizenSmsListDialogComponent implements OnInit {
  citizen: any;
  data: [];

  constructor(
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private dataService: DataService
  ) {
    this.citizen = _data;
  }

  ngOnInit(): void {
    this.getMessages();
  }

  getMessages() {
    this.dataService
      .get(ServerApis.getCitizenRejectImageSmsList, { citizenId: this.citizen.citizenId})
      .subscribe((data) => {
        this.data = data.data;
      });
  }
}
