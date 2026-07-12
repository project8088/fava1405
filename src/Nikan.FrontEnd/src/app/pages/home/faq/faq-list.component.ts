import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'app-faq-list',
  templateUrl: './faq-list.component.html',
  styleUrls: ['./faq-list.component.scss'],
  standalone: false,
})
export class FaqListComponent extends AppBase implements OnInit {
  loadingGroup: boolean = false;
  loadingList: boolean = false;

  faqGroups: any[] = [];
  selectedFaq: any;

  faqList: any[] = [];

  selectedFaqGroupId?: number;

  constructor() {
    super();
    this.getFaqGroup();
  }

  ngOnInit(): void {}

  getFaqGroup() {
    this.loadingGroup = true;
    this.dataService.get(ServerApis.getFaqGroups, {})
      .pipe(
        finalize(() => {
          this.loadingGroup = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response && response.isSuccess) {
                this.faqGroups = response.data ? response.data : [];
                if (this.faqGroups.length > 0) this.getFaqList(this.faqGroups[0]);
              } else {
                let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            }, (error: any) => {
              
            });
  }

  getFaqList(item: any) {
    this.selectedFaqGroupId = item.key;

    this.faqList = [];
    this.loadingList = true;
    this.selectedFaq = item;

    this.dataService.get(ServerApis.getFaqList, { groupId: item.key })
      .pipe(
        finalize(() => {
          this.loadingList = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response && response.isSuccess) {
                this.faqList = response.data ? response.data : [];
              } else {
                let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            }, (error: any) => {
              
            });
  }

  getDetails(item: any) {
    if (item.showDetails === true) {
      item.showDetails = false;
      return;
    } else if (item.showDetails === false) {
      item.showDetails = true;
      return;
    } else {
      item.showDetails = true;
      item.loading = true;
      this.dataService.get(ServerApis.getFaq, { id: item.id }).subscribe(
        (response) => {
          item.loading = false;
          if (response && response.isSuccess) {
            item.details = response.data;
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {
          item.loading = false;
          
        },
      );
    }
  }
}
