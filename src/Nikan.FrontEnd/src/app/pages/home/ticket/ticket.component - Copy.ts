import { Component, OnInit, Inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CustomFormValidators } from 'src/app/core/custom-validator/form-validation';
import Swal from 'sweetalert2';
import { DataService } from '../../../core/services/data-service.service';
import { ServerApis } from '../../../core/server-apis';
import { AuthUser } from '../../../core/authentication/user.model';
import { AuthService } from 'src/app/core/authentication/auth.service';
import { EventEmitter } from 'events';

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.scss'],
})
export class TicketComponent implements OnInit {
  ticketForm: FormGroup;
  isSaving: boolean;
  loadingData: boolean = true;
  periorityList: any[] = [];

  organizationList: any = ([] = []);
  unitList: any = ([] = []);
  loadingUnit: boolean;
  user: AuthUser;
  sendNewTicket: boolean = false;
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
  ) {
    this.ticketForm = this.fb.group({
      subject: [null, []],
      ticketMessage: [null, [Validators.required, Validators.maxLength(10000)]],
      priority: [null, [Validators.required]],
      ticketSubject: [null, []],
      name: [null, [Validators.required]],
      mobileNumber: [null, [this.customValidator.checkMobileNumber]],
      email: [null, [this.customValidator.checkEmail]],
      organizationId: [null, [Validators.required]],
      organizationalUnitId: [null, [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.user = this.authService.getAuthUser();
    if (this.user) {
      this.ticketForm.get('name').setValue(this.user.displayName);
    }

    this.dataService.getEnums().subscribe(
      (response) => {
        if (response) {
          this.periorityList = response.ticketPriority ? response.ticketPriority : [];
        } else {
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        }
      },
      (error) => {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );

    this.getOrganizations();
  }

  getOrganizations() {
    this.loadingData = true;

    this.dataService.get(ServerApis.getAllOrganizational, {}).subscribe(
      (response) => {
        this.loadingData = false;
        if (response.isSuccess) {
          this.organizationList = response.data ? response.data : [];
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loadingData = false;
      },
    );
  }

  getUnitsOfOrganization(ev) {
    if (!this.ticketForm.get('organizationId').value) return;
    this.loadingUnit = true;

    this.dataService
      .get(ServerApis.getAllOrganizationalUnitByOrganId, {
        organId: this.ticketForm.get('organizationId').value.key,
      })
      .subscribe(
        (response) => {
          this.loadingUnit = false;
          if (response.isSuccess) {
            this.unitList = response.data ? response.data : [];
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loadingUnit = false;
        },
      );
  }

  saveInfo() {
    if (this.ticketForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.ticketForm.markAllAsTouched();
      return false;
    }
    this.isSaving = true;
    let formData = this.ticketForm.value;
    this.dataService
      .post(ServerApis.sendUserTicket, {
        Id: '',
        Subject: formData.subject,
        TicketMessage: formData.ticketMessage,
        organizationalUnitId: formData.organizationalUnitId.key,
        name: formData.name ? formData.name : '',
        mobileNumber: formData.mobileNumber ? formData.mobileNumber : '',
        email: formData.email ? formData.email : '',
        ticketSubjectId: formData.ticketSubject ? formData.ticketSubject.key : null,
        UserId: this.user ? this.user.userId : '',
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response.isSuccess) {
            this.toastrService.success('تیکت شما با موفقیت ارسال شد.');
            Swal.fire({
              showConfirmButton: true,
              showCancelButton: false,
              title: 'تیکت شما با موفقیت ارسال شد',
              text: 'کد رهگیری: ' + response.data.code,
              confirmButtonText: 'تائید',
            });
            this.ticketForm.reset();
            this.sendNewTicket = false;
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.isSaving = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
  }

  /*----------------------------------------------------------------*/
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
