import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { AuthUser } from '@core/authentication/user.model';
import { ServerApis } from '@core/server-apis';
import { NewsDto, NewsCommentDto } from '@core/models/news';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { Meta, Title } from '@angular/platform-browser';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'home-news-details',
  templateUrl: './news-details.component.html',
  styleUrls: ['./news-details.component.scss'],
  standalone: false,
})
export class MainNewsDetailsComponent extends AppBase implements OnInit {
  newsid: string = '';
  user?: AuthUser | null;
  loadingData?: boolean;
  news?: NewsDto;
  tags: string[] = [];

  comments: NewsCommentDto[] = [];
  loadingComments: boolean = false;
  sendingComment: boolean = false;

  frm: FormGroup;
  baseUrl: string = ServerApis.baseUrl;

  lastNewsList: NewsDto[] = [];
  loadingLastNews: boolean = false;

  mostVisitedList: NewsDto[] = [];
  loadingVisited: boolean = false;
  newsId: string='';
  constructor(
    private customValidator: CustomFormValidators,
    private titleService: Title,
    private metaService: Meta,
  ) {
    super();
    this.frm = this.fb.group({
      commentMessage: [null, [Validators.required]],
      emailAddress: [null, [Validators.required, this.customValidator.checkEmail]],
      fullName: [null, [Validators.required]],
    });

    this.route.params.subscribe((p) => {
      this.newsId = p['id'];
      this.getDetailsInfo();
      this.getComments();
    });
    this.user = this.authService.currentUserValue;
  }

  ngOnInit() {
    this.getLastNews();
    this.getMostVisitedNews();
  }

  getDetailsInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getNews, { id: this.newsId, forEdit: false }).subscribe(
      (response) => {
        this.loadingData = false;
        if (response.isSuccess) {
          this.news = response.data;
          this.tags = this.news?.seoTags?.split(',') ?? [];
          if (this.news?.seoTags) {
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
      (error:any) => {
        this.loadingData = false;
      },
    );
  }

  getComments() {
    this.loadingComments = true;
    this.dataService.get(ServerApis.getNewsPublishComments, { id: this.newsId }).subscribe(
      (response) => {
        this.loadingComments = false;
        if (response.isSuccess) {
          this.comments = response.data ? response.data : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error:any) => {
        this.loadingComments = false;
      },
    );
  }

  sendComment() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return;
    }
    let form = this.frm.value;
    let param: NewsCommentDto = {
      commentMessage: form.commentMessage,
      emailAddress: form.emailAddress,
      fullName: form.fullName,
      newsItemId: +this.newsId,
    };
    this.sendingComment = true;
    this.dataService.post(ServerApis.addNewsComments, param).subscribe(
      (response) => {
        this.sendingComment = false;
        if (response.isSuccess) {
          this.frm.reset();
          this.toastrService.success(
            'با تشکر، پیام شما بعد از بررسی منتشر خواهد شد.',
            'پیام شما با موفقیت ارسال شد.',
          );
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error:any) => {
        this.sendingComment = false;
      },
    );
  }

  getLastNews() {
    this.loadingLastNews = true;
    this.dataService.get(ServerApis.getLastNews, { top: 10 }).subscribe(
      (response) => {
        this.loadingLastNews = false;
        if (response.isSuccess) {
          this.lastNewsList = response.data ? response.data : [];
        }
      },
      (error:any) => {
        this.loadingLastNews = false;
      },
    );
  }

  getMostVisitedNews() {
    this.loadingVisited = true;
    this.dataService.get(ServerApis.getMostVisitedNews, { top: 10 }).subscribe(
      (response) => {
        this.loadingVisited = false;
        if (response.isSuccess) {
          this.mostVisitedList = response.data ? response.data : [];
        }
      },
      (error:any) => {
        this.loadingVisited = false;
      },
    );
  }
}
