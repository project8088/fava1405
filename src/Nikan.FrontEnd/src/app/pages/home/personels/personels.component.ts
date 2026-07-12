import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { OWL_OPTIONS } from '../owal.config';
import { finalize } from 'rxjs';

@Component({
  selector: 'home-personels',
  templateUrl: './personels.component.html',
  styleUrls: ['./personels.component.scss'],
  standalone: false,
})
export class HomePersonelsListComponent extends AppBase implements OnInit {
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
    this.dataService
      .get(ServerApis.getManagersList, {})
      .pipe(
        finalize(() => {
          this.loadingManagers = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        this.managerList = response.data ? response.data : [];
      });
  }
}
