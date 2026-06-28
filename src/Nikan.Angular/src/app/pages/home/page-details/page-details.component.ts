import { Component, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { DataService } from '../../../core/services/data-service.service';
import { AuthUser } from '../../../core/authentication/user.model';
import { AuthService } from '../../../core/authentication/auth.service';
import { ServerApis } from '../../../core/server-apis'; 
import { Meta, Title } from '@angular/platform-browser';

@Component({
  selector: 'home-page-details',
  templateUrl: './page-details.component.html',
  styleUrls: ['./page-details.component.scss']
})
export class MainPageDetailsComponent implements OnInit {
  slug: string;
  user: AuthUser;
  loadingData: boolean;
  page: any;
  tags: string[] = [];


  baseUrl: string = ServerApis.baseUrl;

  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private titleService: Title,
    private metaService: Meta
  ) {

    this.route.params.subscribe(p => {
      this.slug = p.slug;
      this.getDetailsInfo();
    });
    this.user = this.authService.currentUserValue;
  }



  ngOnInit() {

  }




  getDetailsInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getPageWithSlug + this.slug ).subscribe(response => {
      this.loadingData = false;
      if (response.isSuccess) {

        this.page = response.data;
        this.tags = this.page.seoTags.split(',');
        if (this.page.seoTags) {
          this.titleService.setTitle(this.page.title);
          this.metaService.addTags([
            { name: 'keywords', content: this.page.seoTags },
            { name: 'description', content: this.page.seoDescription },
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


