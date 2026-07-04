import { Component, OnInit, Output } from '@angular/core';

import { EventEmitter } from 'events';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-timer',
  templateUrl: './timer.component.html',
  styleUrls: ['./timer.component.scss'],
  standalone: false,
})
export class TimerComponent extends AppBase implements OnInit {
  timerCounter: number = 120;
  lastTimerCounter: number = 120;
  timerCounterString='';
  resendTimerInterval: any;
  @Output() onEnd = new EventEmitter();

  constructor() {
    super();
  }

  ngOnInit(): void {}

  /**
   * تایمر برای ارسال مجدد کد تائید
   * */
  startTimer() {
    return new Promise((resolve) => {
      this.resendTimerInterval = setInterval(() => {
        this.timerCounter--;
        this.timerCounterString = this.convertSecondstoTime(this.timerCounter);
        if (this.timerCounter <= 0) {
          clearInterval(this.resendTimerInterval);
          this.timerCounter = 0;
          resolve(true);
        }
      }, 1000);
    });
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
