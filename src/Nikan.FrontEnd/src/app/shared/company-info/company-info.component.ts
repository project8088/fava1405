import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AuthUser } from '@core/authentication/user.model';
import { AppBase } from '@app/app.base';

declare var $: any;

@Component({
  selector: 'app-company-info',
  templateUrl: './company-info.component.html',
  styleUrls: ['./company-info.component.scss'],
  standalone: false,
})
export class CompanyInfoComponent extends AppBase implements OnInit {
  companyInfo?: any;
  loading?: boolean;
  companyId: string = '';
  user?: AuthUser | null;
  baseUrl = ServerApis.baseUrl;
  constructor() {
    super();
    this.user = this.authService.currentUserValue;
    this.route.params.subscribe((p) => {
      if (p['id'] != '0' && p['id']) this.companyId = p['id'];
      this.getInfo();
    });
  }

  ngOnInit(): void {}
  getInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getFullCompanyInfo, {
        companyId: this.companyId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.companyInfo = response.data;

            setTimeout(() => {
              $('.lightGallery').lightGallery({
                selector: 'a',
                thumbnail: false,
              });
            }, 1000);
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loading = false;
        },
      );
  }

  back() {
    this.router.navigate(['/admin/companies']);
  }
}
