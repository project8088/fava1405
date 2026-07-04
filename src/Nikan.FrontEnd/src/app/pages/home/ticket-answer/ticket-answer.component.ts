import { Component, OnInit, Inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CustomFormValidators } from 'src/app/core/custom-validator/form-validation';
import Swal from 'sweetalert2';
import { DataService } from '../../../core/services/data-service.service';
import { ServerApis } from '../../../core/server-apis';
import { AuthUser } from '../../../core/authentication/user.model';
import { AuthService } from 'src/app/core/authentication/auth.service';

@Component({
  selector: 'app-ticket-answer',
  templateUrl: './ticket-answer.component.html',
  styleUrls: ['./ticket-answer.component.scss'],
})
export class TicketAnswerComponent implements OnInit {
  ticketForm: FormGroup;
  isSaving: boolean;
  loadingData: boolean = true;

  loadingUnit: boolean;

  searching: boolean;
  trackingCode: string = '';
  ticketAnswer: any;
  loadingSubject: boolean;
  constructor(
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private dataService: DataService,
    private customValidator: CustomFormValidators,
    private authService: AuthService,
  ) {}

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
