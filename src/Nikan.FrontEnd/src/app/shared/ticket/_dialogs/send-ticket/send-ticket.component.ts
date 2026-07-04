import { Component, OnInit, Inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/core/authentication/auth.service';
import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';
import { AuthUser } from '../../../../core/authentication/user.model';

@Component({
  selector: 'app-send-ticket-dialog',
  templateUrl: './send-ticket.component.html',
  styleUrls: ['./send-ticket.component.scss'],
})
export class SendTicketDialogComponent implements OnInit {
  ticketForm: FormGroup;
  isSaving: boolean;
  loadingData: boolean = true;
  loadingSubject: boolean = false;
  organizationList: any = ([] = []);
  unitList: any = ([] = []);
  periorityList: any[] = [];
  loadingUnit: boolean;

  user: AuthUser;
  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<SendTicketDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private dataService: DataService,
  ) {
    this.user = this.authService.currentUserValue;
    this.ticketForm = this.fb.group({
      subject: [null, []],
      ticketMessage: [null, [Validators.required, Validators.maxLength(10000)]],
      priority: [null, [Validators.required]],
      organizationId: [null, [Validators.required]],
      organizationalUnitId: [null, [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.getOrganizations();

    this.dataService.getEnums().subscribe(
      (response) => {
        if (response) {
          this.periorityList = response.ticketPriority ? response.ticketPriority : [];
        } else {
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
          this.matDialogRef.close();
        }
      },
      (error) => {
        this.matDialogRef.close();
      },
    );
  }

  getOrganizations() {
    this.loadingData = true;

    this.dataService.get(ServerApis.getAllSupportCenter, {}).subscribe(
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

  getUnitsOfOrganization() {
    this.loadingUnit = true;

    this.dataService
      .get(ServerApis.getAllOrganizationalUnitByOrganId, {
        organId: this.ticketForm.get('organizationId').value,
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
        Priority: formData.priority,
        organizationalUnitId: formData.organizationalUnitId,
        fileUrl: '',
        name: this.user.displayName,
        email: null,
        mobileNumber: null,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response.isSuccess) {
            this.toastrService.success('پیام شما با موفقیت ارسال شد.');
            if (this.authService.currentUserValue)
              this.router.navigate([
                '/' + this.user.rootModule + '/ticket-details/' + response.data.ticketId,
              ]);

            this.matDialogRef.close(true);
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
}
