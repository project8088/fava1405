import { Component, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { DataService } from '../../../core/services/data-service.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthUser } from '../../../core/authentication/user.model';
import { AuthService } from '../../../core/authentication/auth.service';
import { ServerApis } from '../../../core/server-apis';
import { NewsDto, NewsCommentDto } from '../../../core/models/news';
import { CustomFormValidators } from '../../../core/custom-validator/form-validation';
import { Meta, Title } from '@angular/platform-browser';

@Component({
  selector: 'home-news-details',
  templateUrl: './news-details.component.html',
  styleUrls: ['./news-details.component.scss']
})
export class MainNewsDetailsComponent implements OnInit {
  newsId: string;
  user: AuthUser;
  loadingData: boolean;
  news: NewsDto;
  tags: string[] = [];

  comments: NewsCommentDto[] = [];
  loadingComments: boolean;
  sendingComment: boolean;

  frm: FormGroup;
  baseUrl: string = ServerApis.baseUrl;



  lastNewsList: NewsDto[] = [];
  loadingLastNews: boolean;


  mostVisitedList: NewsDto[] = [];
  loadingVisited: boolean;

  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private router: Router,
    private matDialog: MatDialog,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authService: AuthService,
    private customValidator: CustomFormValidators,
    private titleService: Title,
    private metaService: Meta
  ) {

    this.frm = this.fb.group({
      commentMessage: [null, [Validators.required]],
      emailAddress: [null, [Validators.required, this.customValidator.checkEmail]],
      fullName: [null, [Validators.required]],
    });

    this.route.params.subscribe(p => {
      this.newsId = p.id;
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
    this.dataService.get(ServerApis.getNews, { id: this.newsId, forEdit: false }).subscribe(response => {
      this.loadingData = false;
      if (response.isSuccess) {

        this.news = response.data;
        this.tags = this.news.seoTags.split(',');
        if (this.news.seoTags) {
          this.titleService.setTitle(this.news.title);
          this.metaService.addTags([
            { name: 'keywords', content: this.news.seoTags },
            { name: 'description', content: this.news.seoDescription },
            { name: 'robots', content: 'index, follow' }
          ]);
        }

      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }

    }, error => {
      this.loadingData = false;

    });
  }



  getComments() {
    this.loadingComments = true;
    this.dataService.get(ServerApis.getNewsPublishComments, { id: this.newsId }).subscribe(response => {
      this.loadingComments = false;
      if (response.isSuccess) {
        this.comments = response.data ? response.data:[];
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }

    }, error => {
      this.loadingComments = false;

    });
  }



  sendComment() {
    if (this.frm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.frm.markAllAsTouched();
      return false;
    }
    let form = this.frm.value;
    let param: NewsCommentDto = {
      commentMessage: form.commentMessage,
      emailAddress: form.emailAddress,
      fullName: form.fullName,
      newsItemId: +this.newsId
    };
    this.sendingComment = true;
    this.dataService.post(ServerApis.addNewsComments, param).subscribe(response => {
      this.sendingComment = false;
      if (response.isSuccess) {
        this.frm.reset();
        this.toastrService.success('با تشکر، پیام شما بعد از بررسی منتشر خواهد شد.', 'پیام شما با موفقیت ارسال شد.');
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
      this.sendingComment = false;
    });

  }




  getLastNews() {
    this.loadingLastNews = true;
    this.dataService.get(ServerApis.getLastNews, {top:10}).subscribe(response => {
      this.loadingLastNews = false;
      if (response.isSuccess) {

        this.lastNewsList = response.data ? response.data:[]; 


      }  

    }, error => {
        this.loadingLastNews = false;

    });
  }




  getMostVisitedNews() {
    this.loadingVisited = true;
    this.dataService.get(ServerApis.getMostVisitedNews, { top: 10 }).subscribe(response => {
      this.loadingVisited = false;
      if (response.isSuccess) {

        this.mostVisitedList = response.data ? response.data : [];


      }

    }, error => {
        this.loadingVisited = false;

    });
  }



}


