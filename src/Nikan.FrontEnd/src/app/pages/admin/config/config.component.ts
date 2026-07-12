import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'adm-config',
  templateUrl: './config.component.html',
  styleUrls: ['./config.component.scss'],
  standalone: false,
})
export class AdminConfigComponent extends AppBase implements OnInit {
  id: string = '';

  isSaving = false;
  loading?: boolean;

  configList: any[] = [];

  constructor() {
    super();
  }

  ngOnInit(): void {}

  config(): void {
    this.isSaving = true;
    this.dataService.get(ServerApis.configPortal)
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
            if (response.isSuccess) {
              this.isSaving = false;
              this.configList = response.data ? response.data : [];
            } else {
              this.isSaving = false;
              const msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
              this.toastrService.error(msg);
            }
          });
  }
}
