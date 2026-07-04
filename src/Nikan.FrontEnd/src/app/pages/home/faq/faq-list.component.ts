import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { DataService } from '../../../core/services/data-service.service';
import { ServerApis } from '../../../core/server-apis';

@Component({
  selector: 'app-faq-list',
  templateUrl: './faq-list.component.html',
  styleUrls: ['./faq-list.component.scss'],
})
export class FaqListComponent implements OnInit {
  loadingGroup: boolean;
  loadingList: boolean;

  faqGroups: any[] = [];
  selectedFaq: any;

  faqList: any[] = [];

  selectedFaqGroupId: number;

  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
  ) {
    this.getFaqGroup();
  }

  ngOnInit(): void {}

  getFaqGroup() {
    this.loadingGroup = true;
    this.dataService.get(ServerApis.getFaqGroups, {}).subscribe(
      (response) => {
        this.loadingGroup = false;
        if (response && response.isSuccess) {
          this.faqGroups = response.data ? response.data : [];
          if (this.faqGroups.length > 0) this.getFaqList(this.faqGroups[0]);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loadingGroup = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  getFaqList(item) {
    this.selectedFaqGroupId = item.key;

    this.faqList = [];
    this.loadingList = true;
    this.selectedFaq = item;

    this.dataService.get(ServerApis.getFaqList, { groupId: item.key }).subscribe(
      (response) => {
        this.loadingList = false;
        if (response && response.isSuccess) {
          this.faqList = response.data ? response.data : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loadingList = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  getDetails(item: any) {
    if (item.showDetails === true) {
      item.showDetails = false;
      return false;
    } else if (item.showDetails === false) {
      item.showDetails = true;
      return true;
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
        (error) => {
          item.loading = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
    }
  }
}
