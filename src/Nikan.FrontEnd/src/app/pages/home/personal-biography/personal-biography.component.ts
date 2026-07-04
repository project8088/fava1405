import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AuthUser } from '../../../core/authentication/user.model';
import Swal from 'sweetalert2';
import { ServerApis } from '../../../core/server-apis';
import { Meta, Title } from '@angular/platform-browser';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'home-personal-biography',
  templateUrl: './personal-biography.component.html',
  styleUrls: ['./personal-biography.component.scss'],
    standalone: false
})
export class PersonalBiographyComponent extends AppBase implements OnInit {
  id: string;
  loading: boolean;
  userInfo: any;
  baseUrl: string = ServerApis.baseUrl;

  constructor(
    private titleService: Title,
    private metaService: Meta,
  ) {
      super();
    this.route.params.subscribe((p) => {
      this.id = p.id;
      this.getUserInfo();
    });
  }

  ngOnInit() {}

  getUserInfo() {
    this.loading = true;
    this.dataService.get(ServerApis.getPersonelInfoForView, { id: this.id }).subscribe(
      (response) => {
        this.loading = false;
        if (response && response.isSuccess) {
          this.userInfo = response.data;

          if (this.userInfo.seoTags) {
            this.titleService.setTitle(this.userInfo.seoTags.seoTitle);
            this.metaService.addTags([
              { name: 'keywords', content: this.userInfo.seoTags.seokeywords },
              { name: 'description', content: this.userInfo.seoTags.seoDescription },
              { name: 'robots', content: 'index, follow' },
            ]);
          }
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
          this.router.navigate(['/home']);
        }
      },
      (error) => {
        this.loading = false;
      },
    );
  }
}
