import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import { ApiResult } from 'src/app/core/models/response';
import { DataService } from 'src/app/core/services/data-service.service';
import { ServerApis } from 'src/app/core/server-apis';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-redirect',
  templateUrl: './redirect.component.html',
  styleUrls: ['./redirect.component.scss'],
})


export class RedirectComponent implements OnInit {
  serviceId;
  loading: boolean = true;
  url: string;
  returnUrl: string = '';
  constructor(
    private dataService: DataService,
    private route: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((p) => {
      if (p.returnUrl) this.returnUrl = p.returnUrl;
    });

    this.route.queryParams.subscribe((params) => {
      if (params['serviceId']) {
        this.serviceId = params['serviceId'];
        this.getUserUrl();
      } else this.router.navigate(['/']);
    });
  }
   
  getUserUrl() {
    this.dataService
      .get(ServerApis.callCitizenService + '?serviceId=' + this.serviceId + '&returnUrl=' + this.returnUrl)
      .subscribe(
        (data: ApiResult<any>) => {
          if (data.isSuccess) {
            this.url = data.data;
            if (data.data.indexOf('http') > -1)
              window.location.href = data.data;
            else this.router.navigate([data.data]);
          } else this.toastrService.error(data.messages);
        },
        (error) => {
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        }
      );
  }
}
