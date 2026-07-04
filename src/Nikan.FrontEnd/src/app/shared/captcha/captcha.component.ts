import { Component, OnInit } from '@angular/core';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-captcha',
  templateUrl: './captcha.component.html',
  styleUrls: ['./captcha.component.scss'],
})
export class CaptchaComponent extends AppBase implements OnInit {
  loadingCaptcha: boolean = false;
  captchaImage: string = '';

  constructor() {
      super();}

  ngOnInit(): void {
    this.getCaptcha();
  }

  getCaptcha() {
    this.loadingCaptcha = true;

    this.dataService.getCaptchaImage({}).subscribe(
      (response) => {
        this.loadingCaptcha = false;
        let reader = new FileReader();
        let photo = new File([response], 'captcha.png', { type: 'image/png' });
        reader.readAsDataURL(photo);
        reader.onload = (event: any) => {
          this.captchaImage = event.target.result;
        };
      },
      (error) => {
        this.loadingCaptcha = false;
      },
    );
  }
}
