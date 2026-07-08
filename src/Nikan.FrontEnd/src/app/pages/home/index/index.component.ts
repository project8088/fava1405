import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { NewsDto } from '@core/models/news';
import { ServerApis } from '@core/server-apis';
import { SiteSettingViewModel } from '@core/models/setting';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs/operators';
import { OWL_OPTIONS } from '../owal.config';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss'],
  standalone: false,
})
export class IndexComponent extends AppBase implements OnInit, AfterViewInit, OnDestroy {
  lastNews: NewsDto[] = [];
  loadingNews: boolean = false;
  baseUrl: string = ServerApis.baseUrl;

  setting: SiteSettingViewModel | null = null;

  owlOptions = OWL_OPTIONS;
  constructor() {
    super();
  }

  ngOnInit(): void {
    this.getLastNews();

    this.dataService.getSetting().subscribe((response) => {
      this.setting = response;
    });
  }
  ngOnDestroy() {}

  ngAfterViewInit() {}

  getLastNews() {
    this.loadingNews = true;
    this.dataService
      .get(ServerApis.getLastNews, {})
      .pipe(
        finalize(() => {
          this.loadingNews = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        this.loadingNews = false;
        this.lastNews = response.data ? response.data : [];
       
      });
  }


}
