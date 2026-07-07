import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { Validators, FormGroup } from '@angular/forms';
import Swal from 'sweetalert2';
import { AuthUser } from '@core/authentication/user.model';
import { ServerApis } from '@core/server-apis';
import { UploaderComponent } from '../../../../uploader/uploader.component';
import { CitizenProfileDialogComponent } from '../../../../_dialog/citizen-profile/citizen-profile.component';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-ticket-response',
  templateUrl: './ticket-response.component.html',
  styleUrls: ['./ticket-response.component.scss'],
  standalone: false,
})
export class TicketResponseComponent extends AppBase implements OnInit, AfterViewInit {
  id: string = '';
  loading: boolean = true;
  ticket: any;
  resultsLength: number = 0;

  attachmentGuid: any;
  uploadUrl: string = ServerApis.uploadAttachment;
  uploadData: any = { Caption: '' };
  answerForm: FormGroup;
  isSending: boolean = false;

  currentUser?: AuthUser | null;

  rootModule: string = '';
  @ViewChild(UploaderComponent) uploader!: UploaderComponent;

  constructor() {
    super();
    this.uploadData.Guid = this.newGuid();

    this.answerForm = this.fb.group({
      responseText: [null, [Validators.required, Validators.maxLength(10000)]],
      sendSms: [false],
      review: [false],
      resolved: [false],
    });
  }

  ngOnInit(): void {
    if (this.authService.currentUserValue) {
      this.currentUser = this.authService.currentUserValue;
      this.rootModule = this.authService.currentUserValue.rootModule ?? '';
    }
  }

  ngAfterViewInit() {
    this.route.params.subscribe((p) => {
      this.id = p['id'];
      this.getTicketInfo();
    });
  }

  getTicketInfo() {
    this.loading = true;
    this.dataService.get(ServerApis.getTicketById, { id: this.id }).subscribe(
      (response) => {
        this.loading = false;
        if (response.isSuccess) {
          this.ticket = response.data ? response.data : {};
          if (!this.ticket.responseTickets) {
            this.ticket.responseTickets = [];
          }
          // this.ticket.responseTickets.unshift({
          //   responseText:this.ticket.ticketMessage,
          //   ownerDisplayName:this.ticket.fullName,
          //   responseTextOnDate:this.ticket.createdOn,
          //   ownerId:this.ticket.ownerId
          // });

          this.answerForm.get('resolved')?.setValue(this.ticket.isSolved);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loading = false;
      },
    );
  }

  closeTicket(isClosed: boolean) {
    Swal.fire({
      text: isClosed
        ? 'آیا برای بستن تیکت اطمینان دارید؟'
        : 'آیا برای باز کردن تیکت اطمینان دارید؟',
      title: isClosed ? 'بستن تیکت' : 'باز کردن تیکت',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then((result) => {
      if (result.value) {
        this.loading = true;
        this.dataService
          .post(ServerApis.updateTicketsStatues, {
            TicketId: this.id,
            isClosed: isClosed,
          })
          .subscribe(
            (response) => {
              this.loading = false;
              if (response.isSuccess) {
                if (isClosed) {
                  this.ticket.colsedById = this.currentUser?.userId;
                  this.toastrService.success('تیکت با موفقیت بسته شد.');
                } else {
                  this.ticket.colsedById = null;
                  this.toastrService.success('تیکت با موفقیت باز شد.');
                }
              } else {
                let msg = response.messages
                  ? response.messages
                  : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            },
            (error) => {
              this.loading = false;
              this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
            },
          );
      }
    });
  }

  back() {
    if (window.history.length > 1) {
      window.history.go(-1);
    } else {
      window.close();
    }
  }

  openCitizenProfile(ownerId: number) {
    this.matDialog.open(CitizenProfileDialogComponent, {
      panelClass: 'custom-dialog',
      data: {
        userCode: ownerId,
      },
      width: '85%',
      maxWidth: '1800px',
    });
  }

  sendAnswer() {
    if (this.answerForm.invalid) {
      this.toastrService.warning('متن پیام را وارد کنید.');
      this.answerForm.markAllAsTouched();
      return;
    }
    this.isSending = true;
    this.dataService
      .post(ServerApis.sendAnswerTicket, {
        TicketId: this.id,
        ResponseText: this.answerForm.get('responseText')?.value,
        mobileNumber: this.ticket.mobileNumber,
        Description: '',
        SendSms: this.answerForm.get('sendSms')?.value,
        Review: this.answerForm.get('review')?.value,
        Solved: this.answerForm.get('resolved')?.value,
        AttachmentGuid: this.attachmentGuid ? this.attachmentGuid : '',
      })
      .subscribe(
        (response) => {
          this.isSending = false;
          if (response.isSuccess) {
            this.toastrService.success('پیام شما با موفقیت ارسال شد.');

            this.answerForm.get('responseText')?.setValue('');
            this.getTicketInfo();
            if (this.attachmentGuid) {
              this.uploader.removeAllFiles(this.uploader.uploaderInput);
              this.uploader.files = [];
              this.attachmentGuid = '';
            }
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.isSending = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
  }

  newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = (Math.random() * 16) | 0,
        v = c == 'x' ? r : (r & 0x3) | 0x8;
      return v.toString(16);
    });
  }
  getAttachmentId(ev: string) {
    this.attachmentGuid = ev;
  }
}
