import { Component, OnInit } from '@angular/core';
import { AuthUser } from '@core/authentication/user.model';
import { ServerApis } from '@core/server-apis';
import { Meta, Title } from '@angular/platform-browser';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'home-page-details',
  templateUrl: './page-details.component.html',
  styleUrls: ['./page-details.component.scss'],
  standalone: false,
})
export class MainPageDetailsComponent extends AppBase implements OnInit {
  slug: string = '';
  user?: AuthUser | null;
  loadingData?: boolean;
  page: any;
  tags: string[] = [];

  baseUrl: string = ServerApis.baseUrl;

  constructor(
    private titleService: Title,
    private metaService: Meta,
  ) {
    super();
    this.route.params.subscribe((p) => {
      this.slug = p['slug'];
      this.getDetailsInfo();
    });
    this.user = this.authService.currentUserValue;
  }

  ngOnInit() {}

  getDetailsInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getPageWithSlug + this.slug).subscribe(
      (response) => {
        this.loadingData = false;
        if (response.isSuccess) {
          this.page = response.data;
          this.tags = this.page.seoTags.split(',');
          if (this.page.seoTags) {
            this.titleService.setTitle(this.page.title);
            this.metaService.addTags([
              { name: 'keywords', content: this.page.seoTags },
              { name: 'description', content: this.page.seoDescription },
              { name: 'robots', content: 'index, follow' },
            ]);
          }
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
        this.chdr.detectChanges();
      },
      (error: any) => {
        this.loadingData = false;
      },
    );
  }
}
