import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '@core/server-apis';

@Component({
  selector: 'app-citizen-image',
  templateUrl: './citizen-image.component.html',
  styleUrls: ['./citizen-image.component.scss'],
})
export class AdminCitizenImageDialogComponent implements OnInit {
  citizen;
  baseUrl = ServerApis.baseUrl;

  constructor(@Inject(MAT_DIALOG_DATA) private _data: any) {
    this.citizen = _data;
  }

  ngOnInit(): void {}
}
