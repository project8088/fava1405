import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import Swal from 'sweetalert2';
import { ServerApis } from '../../../core/server-apis';
import { AuthUser } from '../../../core/authentication/user.model';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-ticket-answer',
  templateUrl: './ticket-answer.component.html',
  styleUrls: ['./ticket-answer.component.scss'],
    standalone: false
})
export class TicketAnswerComponent extends AppBase implements OnInit {
  ticketForm: FormGroup;
  isSaving: boolean;
  loadingData: boolean = true;

  loadingUnit: boolean;

  searching: boolean;
  trackingCode: string = '';
  ticketAnswer: any;
  loadingSubject: boolean;
  constructor(
    private customValidator: CustomFormValidators
  ) {
      super();}

  ngOnInit(): void {}

  searchTrackingCode() {
    this.searching = true;
    this.ticketAnswer = '';
    this.dataService.get(ServerApis.getAnswerTicket, { refCode: this.trackingCode }).subscribe(
      (response) => {
        this.searching = false;
        if (response && response.isSuccess) {
          this.ticketAnswer = response.data;
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.searching = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }
}
