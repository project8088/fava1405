import { Component, OnInit } from '@angular/core';

import { DataService } from '../../core/services/data-service.service';
import { ServerApis } from '../../core/server-apis';
import { SiteSettingViewModel } from '../../core/models/setting';

@Component({
  selector: 'app-main-footer',
  templateUrl: './main-footer.component.html',
  styleUrls: ['./main-footer.component.scss'],
})
export class MainFooterComponent implements OnInit {
  baseUrl: string = ServerApis.baseUrl;
  setting: SiteSettingViewModel;
  constructor(private dataService: DataService) {
    this.dataService.getSetting().subscribe((response) => {
      this.setting = response;
    });
  }

  ngOnInit(): void {}
}
