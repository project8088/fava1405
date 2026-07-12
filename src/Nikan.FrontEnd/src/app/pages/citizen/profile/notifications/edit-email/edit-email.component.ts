import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { TimerComponent } from '@app/shared/timer/timer.component';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'app-citizen-edit-email',
  templateUrl: './edit-email.component.html',
  styleUrls: ['./edit-email.component.scss'],
  standalone: false,
})
export class CitizenEditEmailComponent extends AppBase implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  userId?: string;
  cardForm: FormGroup;

  confirmForm: FormGroup;
  loadingState: boolean = false;
  codeSent: boolean = false;

  timerCounter: number = 120;
  lastTimerCounter: number = 120;
  timerCounterString = '';
  resendTimerInterval: any;
  editMode: boolean = false;
  formIsShowing: boolean = false;

  @ViewChild('timer', { static: false }) timer!: TimerComponent;

  constructor() {
    super();
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
  showForm() {
    this.formIsShowing = true;
  }

  getCardInfo() {
    this.dataService.get(ServerApis.getCitizenEmail).subscribe(
      (data) => {
        this.loading = false;
        this.formIsShowing = data.data.Email;
        this.cardForm.patchValue({
          citizenId: data.data.citizenId,
          email: data.data.email,
        });
      },
      (error: any) => {
        this.loading = false;
      },
    );
  }

  sendCode() {
    const form = this.cardForm.getRawValue();
    this.dataService.post(ServerApis.checkValidMobileNumberForCitzenRegister, form).subscribe(
      (response) => {
        if (response && response.isSuccess) {
          this.codeSent = true;
          this.timer.startTimer().then(() => {
            this.codeSent = false;
          });

          this.dataService.post(ServerApis.getVerfiCodeByCitizen, form)
            .pipe(
              finalize(() => {
                this.codeSent = false;
                this.chdr.detectChanges();
              }),
            )
            .subscribe((res) => {
                        if (res.isSuccess) {
                          this.toastrService.success('کد تایید به ایمیل شما ارسال شد');
                        }
                      });
        } else {
          let msg = response.messages ? response.messages : 'ایمیل معتبر نیست';
          this.toastrService.error(msg);
        }
      },
      (error: any) => {
        this.isSaving = false;
        
      },
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
      (error: any) => {
        this.isSaving = false;
        
      },
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
  convertSecondstoTime(given_seconds: number) {
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
