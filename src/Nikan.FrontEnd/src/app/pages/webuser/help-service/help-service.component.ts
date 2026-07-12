import { Component, OnInit } from '@angular/core';
import { AuthUser } from '@core/authentication/user.model';
import { ServerApis } from '@core/server-apis';
import { NewsDto, NewsCommentDto } from '@core/models/news';
import { Meta, Title } from '@angular/platform-browser';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'home-help-service',
  templateUrl: './help-service.component.html',
  styleUrls: ['./help-service.component.scss'],
  standalone: false,
})
export class WebUserHelpServiceDetailsComponent extends AppBase implements OnInit {
  newsId: string = '';
  user?: AuthUser | null;
  loadingData?: boolean;
  news = new NewsDto();
  tags: string[] = [];

  comments: NewsCommentDto[] = [];
  loadingComments: boolean = false;
  sendingComment: boolean = false;

  baseUrl: string = ServerApis.baseUrl;

  lastNewsList: NewsDto[] = [];
  loadingLastNews: boolean = false;

  mostVisitedList: NewsDto[] = [];
  loadingVisited: boolean = false;

  constructor(
    private titleService: Title,
    private metaService: Meta,
  ) {
    super();
    this.route.params.subscribe((p) => {
      this.newsId = p['id'];
      this.getDetailsInfo();
    });
    this.user = this.authService.currentUserValue;
  }

  ngOnInit() {}

  getDetailsInfo() {
    this.loadingData = true;
    this.dataService
      .get(ServerApis.getNews, { id: this.newsId, forEdit: false })
      .pipe(
        finalize(() => {
          this.loadingData = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response.isSuccess) {
            this.news = response.data ?? new NewsDto();
            this.tags = this.news.seoTags.split(',');
            if (this.news.seoTags) {
              this.titleService.setTitle(this.news.title);
              this.metaService.addTags([
                { name: 'keywords', content: this.news.seoTags },
                { name: 'description', content: this.news.seoDescription },
                { name: 'robots', content: 'index, follow' },
              ]);
            }
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
  }
}
