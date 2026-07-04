import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import { DataService } from 'src/app/core/services/data-service.service';
import { ServerApis } from 'src/app/core/server-apis';

@Component({
  selector: 'app-terms',
  templateUrl: './terms.component.html',
  styleUrls: ['./terms.component.scss'],
})
export class TermsComponent implements OnInit {
  regLink: string = '';
  service = { serviceName: null };
  serviceId;
  loadingData: boolean;
    toastrService: any;

  constructor(
    private activeRoute: ActivatedRoute,
    private dataService: DataService,
    private router: Router
  ) {
    this.activeRoute.queryParams.subscribe((params) => {
      this.serviceId = params.serviceId; 
    });
  }

  ngOnInit(): void { 
    this.getServiceInfo();
  }

  getServiceInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getAppInfo, {
      id: this.serviceId,
    }).subscribe(response => {
      this.loadingData = false;
      if (response.isSuccess) {
        debugger;
        this.service = response.data;
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }

    }, error => {
      this.loadingData = false;

    });
  }


 
}
