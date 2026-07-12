import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AuthUser } from '@core/authentication/user.model';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-transaction-details',
  templateUrl: './transaction-details.component.html',
  styleUrls: ['./transaction-details.component.scss'],
  standalone: false,
})
export class TransactionDetailsComponent extends AppBase implements OnInit, AfterViewInit {
  id: string = '';
  transactionInfo: any;
  user?: AuthUser | null;
  isLoadingResults: boolean = true;

  constructor() {
    super();
    this.user = this.authService.currentUserValue;
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.route.params.subscribe((p) => {
      this.id = p['id'];
      this.getInfo();
    });
  }

  getInfo() {
    this.isLoadingResults = true;
    return this.dataService
      .get(ServerApis.getTransaction, { id: this.id })
      .pipe(
        finalize(() => {
          this.isLoadingResults = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response.isSuccess && response.data) {
            this.transactionInfo = response.data ? response.data : {};
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
  }

  back() {
    window.history.back();
  }
}
