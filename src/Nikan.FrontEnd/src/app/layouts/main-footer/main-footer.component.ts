import { Component, OnInit } from '@angular/core';
import { ServerApis } from '../../core/server-apis';
import { SiteSettingViewModel } from '../../core/models/setting';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-main-footer',
  templateUrl: './main-footer.component.html',
  styleUrls: ['./main-footer.component.scss'],
    standalone: false
})
export class MainFooterComponent extends AppBase implements OnInit {
  baseUrl: string = ServerApis.baseUrl;
  setting: SiteSettingViewModel;
  constructor() {
      super();
    this.dataService.getSetting().subscribe((response) => {
      this.setting = response;
    });
  }

  ngOnInit(): void {}
}
