import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { TimerComponent } from 'src/app/shared/timer/timer.component';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-edit-mobile',
  templateUrl: './edit-mobile.component.html',
  styleUrls: ['./edit-mobile.component.scss'],
  standalone: false,
})
export class CitizenEditMobileComponent extends AppBase implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  userId?: string;
  editForm: FormGroup;

  confirmForm: FormGroup;
  loadingState: boolean=false;

  emailForm: FormGroup;

  sendingSMS: boolean = false;
  showConfirmCode: boolean = false;

  timerCounter: number = 120;
  lastTimerCounter: number = 120;
  timerCounterString='';
  resendTimerInterval: any;
  isConfirm = false;

  oldMobileNumber = false;
  @ViewChild('phoneTimer', { static: false }) phoneTimer: TimerComponent;

  constructor() {
    super();
    this.editForm = this.fb.group({
      newmobileNumber: [null, [Validators.required]],
    });
    this.confirmForm = this.fb.group({
      smsActiveCode: [null, [Validators.required]],
    });

    this.getCitizenMobileNumber();
  }

  ngOnInit(): void {}

  getCitizenMobileNumber() {
    this.dataService.get(ServerApis.getCitizenMobileNumber).subscribe((data) => {
      this.loading = false;
      this.oldMobileNumber = data.data.mobileNumber;
      this.isConfirm = data.data.isConfirm;
      this.editForm.patchValue({
        citizenId: data.data.citizenId,
      });
    });
  }

  sendCode() {
    if (this.editForm.get('newmobileNumber')?.invalid) {
      this.toastrService.warning('شماره موبایل جدید  خود را وارد کنید.');
      this.editForm.get('newmobileNumber')?.markAsTouched();
        return ;
    }

    this.sendingSMS = true;
    this.dataService
      .post(ServerApis.checkValidMobileNumberAndGetVerfiyCodeForChangeMobileNumber, {
        NewMobileNumber: this.editForm.get('newmobileNumber')?.value,
      })
      .subscribe(
        (response) => {
          this.sendingSMS = false;
          if (response.isSuccess) {
            this.toastrService.success('کد تائید شماره موبایل با موفقیت ارسال شد.');
            this.lastTimerCounter = this.lastTimerCounter + 60;
            this.timerCounter = this.lastTimerCounter;
            this.startTimer();
            this.showConfirmCode = true;
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error:any) => {
          this.sendingSMS = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
  }

  saveForm() {
    if (this.confirmForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.confirmForm.markAllAsTouched();
        return ;
    }
    this.isSaving = true;

    var formValue = this.editForm.value;
    var confirmFormValue = this.confirmForm.value;
    this.dataService
      .post(ServerApis.updteCitizenMobileNumber, {
        MobileNumber: formValue.newmobileNumber,
        SmsActiveCode: confirmFormValue.smsActiveCode,
      })
      .subscribe(
        (response) => {
          if (response && response.isSuccess) {
            this.showForm();
            this.isSaving = false;
            this.getCitizenMobileNumber();
            this.showConfirmCode = false;

            this.toastrService.success(response.messages);
          } else {
            this.isSaving = false;
            let msg = response.messages ? response.messages : 'شماره موبایل معتبر نیست';
            this.toastrService.error(msg);
          }
        },
        (error:any) => {
          this.isSaving = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
  }

  toggleEditMode() {}
  cancelEdit() {
    this.showForm();
    this.getCitizenMobileNumber();
  }

  showForm() {}
  /**
   * تایمر برای ارسال مجدد کد تائید
   * */
  startTimer() {
    this.resendTimerInterval = setInterval(() => {
      this.timerCounter--;
      this.timerCounterString = this.convertSecondstoTime(this.timerCounter);
      if (this.timerCounter <= 0) {
        clearInterval(this.resendTimerInterval);
        this.timerCounter = 0;
      }
    }, 1000);
  }

  /**
   * convert 300s to 5:00
   * @param {any} given_seconds
   */
  convertSecondstoTime(given_seconds:number) {
    var dateObj = new Date(given_seconds * 1000);
    var hours = dateObj.getUTCHours();
    var minutes = dateObj.getUTCMinutes();
    var seconds = dateObj.getSeconds();

    var timeString =
      hours.toString().padStart(2, '0') +
      ':' +
      minutes.toString().padStart(2, '0') +
      ':' +
      seconds.toString().padStart(2, '0');

    return timeString;
  }
}
