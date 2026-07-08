import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { OWL_OPTIONS } from '../../owal.config';

@Component({
  selector: 'home-managers',
  templateUrl: './managers.component.html',
  styleUrls: ['./managers.component.scss'],
  standalone: false,
})
export class HomeManagersListComponent extends AppBase implements OnInit {
  loadingManagers: boolean = false;
  managerList: any[] = [];
  baseUrl: string = ServerApis.baseUrl;

  owlOptions = OWL_OPTIONS;
  constructor() {
    super();
  }

  ngOnInit() {
    this.getManagerList();
  }

  getManagerList() {
    this.loadingManagers = true;
    this.dataService.get(ServerApis.getManagersList, {}).subscribe(
      (response) => {
        this.loadingManagers = false;
        this.managerList = response.data ? response.data : [];
    
      },
      (error:any) => {
        this.loadingManagers = false;
      },
    );
  }


}
