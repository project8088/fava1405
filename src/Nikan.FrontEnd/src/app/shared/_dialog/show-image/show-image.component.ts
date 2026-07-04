import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from 'src/app/core/server-apis';

@Component({
  selector: 'app-show-image',
  templateUrl: './show-image.component.html',
  styleUrls: ['./show-image.component.scss'],
})
export class ShowImageDialogComponent implements OnInit {
  data;
  baseUrl = ServerApis.baseUrl;

  constructor(@Inject(MAT_DIALOG_DATA) private _data: any) {
    this.data = _data;
  }

  ngOnInit(): void {}
}
