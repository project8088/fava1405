import { Component, ContentChild, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { DataService } from 'src/app/core/services/data-service.service';
import { ServerApis } from 'src/app/core/server-apis';
import { TimerComponent } from 'src/app/shared/timer/timer.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-citizen-edit-email',
  templateUrl: './edit-email.component.html',
  styleUrls: ['./edit-email.component.scss'],
})
export class CitizenEditEmailComponent implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  userId: string;
  cardForm: FormGroup;

  confirmForm: FormGroup;
  loadingState: boolean;
  codeSent: boolean;

  emailForm: FormGroup;

  timerCounter: number = 120;
  lastTimerCounter: number = 120;
  timerCounterString: string;
  resendTimerInterval: any;
  editMode: boolean;
  formIsShowing: boolean = false;

  @ViewChild('timer', { static: false }) timer: TimerComponent;

  constructor(
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private dataService: DataService
  ) {
    this.cardForm = this.fb.group({
      email: [null, [Validators.required]],
    });
    this.confirmForm = this.fb.group({
      smsActiveCode: [null, [Validators.required]],
    });

    this.getCardInfo();
  }

  ngOnInit(): void {}

  toggleEditMode() {
    this.editMode = !this.editMode;
  }
  showForm(){
    this.formIsShowing = true;
  }

  getCardInfo() {
    this.dataService.get(ServerApis.getCitizenEmail).subscribe(
      (data) => {
        this.loading = false;
        this.formIsShowing = data.data.Email;
        this.cardForm.patchValue({
          citizenId:data.data.citizenId,
          email: data.data.email,
        });
      },
      (error) => {
        this.loading = false;
      }
    );
  }

  sendCode() {
    const form = this.cardForm.getRawValue();
    this.dataService
      .post(ServerApis.checkValidMobileNumberForCitzenRegister, form)
      .subscribe(
        (response) => {
          if (response && response.isSuccess) {
            this.codeSent = true;
            this.timer.startTimer().then(() => {
              this.codeSent = false;
            });

            this.dataService
              .post(ServerApis.getVerfiCodeByCitizen, form)
              .subscribe((res) => {
                if (res.isSuccess) {
                  this.toastrService.success('کد تایید به ایمیل شما ارسال شد');
                }
              });
          } else {
            let msg = response.messages
              ? response.messages
              : 'ایمیل معتبر نیست';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.isSaving = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        }
      );
  }

  saveForm() {
    const form = {
      ...this.confirmForm.getRawValue(),
      ...this.cardForm.getRawValue(),
    };
    this.dataService.post(ServerApis.updteCitizenEmailAddress, form).subscribe(
      (response) => {
        if (response && response.isSuccess) {
          this.toastrService.success(response.message);
        } else {
          let msg = response.messages ? response.messages : 'ایمیل معتبر نیست';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.isSaving = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      }
    );
  }

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
  convertSecondstoTime(given_seconds) {
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
