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
  selector: 'home-help-service',
  templateUrl: './help-service.component.html',
  styleUrls: ['./help-service.component.scss']
})
export class WebUserHelpServiceDetailsComponent implements OnInit {
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

  

    this.route.params.subscribe(p => {
      this.newsId = p.id;
      this.getDetailsInfo(); 
    });
    this.user = this.authService.currentUserValue;
  }



  ngOnInit() {
  
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



 




 


 

}


