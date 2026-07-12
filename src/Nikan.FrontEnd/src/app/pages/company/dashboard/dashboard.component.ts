import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { ViewNotificationDetailsDialogComponent } from '@app/shared/_dialog/notification-details/notification-details.component';
import { AuthUser } from '@core/authentication/user.model';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-company-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  standalone: false,
})
export class CompnayDashboardComponent extends AppBase implements OnInit {
  loading?: boolean;
  notifications: any[] = [];
  baseUrl: string = ServerApis.baseUrl;
  loadingStore: boolean = false;
  storeItems: any[] = [];

  user?: AuthUser | null;

  constructor() {
    super();
    this.authService.currentUser.subscribe((u) => {
      this.user = u;
    });
  }
  ngOnInit(): void {
    this.getNotificationList();
    // this.getStoreList();
  }

  getNotificationList() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getLastNotifications, {})
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response && response.isSuccess) {
          this.notifications = response.data ? response.data : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      });
  }

  getStoreList() {
    this.loadingStore = true;
    this.dataService
      .get(ServerApis.getAllStoreItemForComany, {})
      .pipe(
        finalize(() => {
          this.loadingStore = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        if (response && response.isSuccess) {
          this.storeItems = response.data ? response.data : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      });
  }

  openNotificationDetails(item: any) {
    this.matDialog.open(ViewNotificationDetailsDialogComponent, {
      data: {
        id: item.id,
      },
      panelClass: 'custom-dialog',
      minWidth: '80%',
    });
  }

  buy(item: any) {
    this.toastrService.info('در حال حاضر امکان ارتباط با درگاه پرداخت برقرار نیست.');
  }
}
