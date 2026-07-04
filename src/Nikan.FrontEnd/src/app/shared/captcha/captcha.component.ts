import { Component, OnInit } from '@angular/core';

import { DataService } from 'src/app/core/services/data-service.service';

@Component({
  selector: 'app-captcha',
  templateUrl: './captcha.component.html',
  styleUrls: ['./captcha.component.scss'],
})
export class CaptchaComponent implements OnInit {
  loadingCaptcha: boolean = false;
  captchaImage: string = '';

  constructor(private dataService: DataService) {}

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
      }
    );
  }
}
