import { Component, OnInit } from '@angular/core';
import { CompanyInfoDto } from '../../../core/models/company/company-info';
import { ServerApis } from '../../../core/server-apis';
import { FormGroup, Validators } from '@angular/forms';
import { CustomFormValidators } from '../../../core/custom-validator/form-validation';
import { AuthUser } from '../../../core/authentication/user.model';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-company-page',
  templateUrl: './company-page.component.html',
  styleUrls: ['./company-page.component.scss'],
  standalone: false,
})
export class CompanyPageComponent extends AppBase implements OnInit {
  companyInfo: CompanyInfoDto;
    loading?: boolean;
  companyId: string = '';

  messageIsSended: boolean;
  sendedMessages: string;
  contactForm: FormGroup;
  periorityList: any[] = [];
  unitList: any = ([] = []);
  loadingUnit: boolean;
  isSaving=false;

  user: AuthUser;

  baseUrl: string = ServerApis.baseUrl;

  lat = 0;
  lng = 0;

  constructor(private customValidator: CustomFormValidators) {
    super();
    this.contactForm = this.fb.group({
      subject: [null, [Validators.required, Validators.maxLength(1000)]],
      message: [null, [Validators.required, Validators.maxLength(10000)]],
      name: [null, [Validators.required]],
      email: [null, [this.customValidator.checkEmail]],

      mobileNumber: [null, [this.customValidator.checkMobileNumber]],
    });

    this.route.params.subscribe((p) => {
      if (p['id'] != '0' && p['id']) this.companyId = p['id'];
      this.getInfo();
    });
  }

  ngOnInit(): void {
    this.user = this.authService.getAuthUser();
    if (this.user) {
      this.contactForm.get('name')?.setvalue(this.user.displayName);
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
  }

  getInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.fullCompanyInfo, {
        companyId: this.companyId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.companyInfo = response.data ? response.data : {};
            if (this.companyInfo.lat && this.companyInfo.lng) {
              this.lat = +this.companyInfo.lat;
              this.lng = +this.companyInfo.lng;
            }
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loading = false;
        },
      );
  }

  saveInfo() {
    if (this.contactForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.contactForm.markAllAsTouched();
        return ;
    }
    this.isSaving = true;
    let formData = this.contactForm.value;
    this.dataService
      .post(ServerApis.addContact, {
        Id: '',
        Subject: formData.subject,
        Message: formData.message,
        organizationalUnitId: null,
        name: formData.name ? formData.name : '',
        mobileNumber: formData.mobileNumber ? formData.mobileNumber : '',
        email: formData.email ? formData.email : '',
        CompanyId: this.companyId,
        UserId: this.user ? this.user.userId : '',
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response.isSuccess) {
            this.sendedMessages = response.messages
              ? response.messages
              : 'پیام شما با موفقیت ارسال شد.';
            this.toastrService.success(this.sendedMessages);
            this.messageIsSended = true;

            this.contactForm.reset();
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.isSaving = false;
        },
      );
  }
}
