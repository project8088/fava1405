import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '@core/server-apis';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-show-image',
  templateUrl: './show-image.component.html',
  styleUrls: ['./show-image.component.scss'],
    standalone: false
})
export class ShowImageDialogComponent extends AppBase implements OnInit {
  data;
  baseUrl = ServerApis.baseUrl;

  constructor(@Inject(MAT_DIALOG_DATA) private _data: any) {
      super();
    this.data = _data;
  }

  ngOnInit(): void {}
}
