import { Component, OnInit } from '@angular/core';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-ticket-answer',
  templateUrl: './ticket-answer.component.html',
  styleUrls: ['./ticket-answer.component.scss'],
  standalone: false,
})
export class TicketAnswerComponent extends AppBase implements OnInit {
  isSaving = false;
  loadingData: boolean = true;

  loadingUnit: boolean = false;

  searching: boolean = false;
  trackingCode: string = '';
  ticketAnswer: any;
  loadingSubject: boolean = false;
  constructor(private customValidator: CustomFormValidators) {
    super();
  }

  ngOnInit(): void {}

  searchTrackingCode() {
    this.searching = true;
    this.ticketAnswer = '';
    this.dataService
      .get(ServerApis.getAnswerTicket, { refCode: this.trackingCode })
      .pipe(
        finalize(() => {
          this.searching = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe(
        (response) => {
          if (response && response.isSuccess) {
            this.ticketAnswer = response.data;
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {},
      );
  }
}
