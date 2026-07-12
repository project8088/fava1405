import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { FormGroup, Validators } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { AuthUser } from '@core/authentication/user.model';
import { SiteSettingViewModel } from '@core/models/setting';
import { AppBase } from '@app/app.base';
import { finalize } from "rxjs";

@Component({
  selector: 'app-contact-us',
  templateUrl: './contact-us.component.html',
  styleUrls: ['./contact-us.component.scss'],
  standalone: false,
})
export class ContactUsComponent extends AppBase implements OnInit {
  contactForm: FormGroup;
  loadingData: boolean = true;
  periorityList: any[] = [];
  organizationList: any = ([] = []);
  unitList: any = ([] = []);
  loadingUnit: boolean = false;
  isSaving = false;

  user?: AuthUser | null;
  setting?: SiteSettingViewModel | null;

  constructor(private customValidator: CustomFormValidators) {
    super();
    this.contactForm = this.fb.group({
      subject: [null, [Validators.required, Validators.maxLength(1000)]],
      message: [null, [Validators.required, Validators.maxLength(10000)]],
      organizationId: [null, [Validators.required]],
      organizationalUnitId: [null, [Validators.required]],
      name: [null, [Validators.required]],
      email: [null, [this.customValidator.checkEmail]],
      mobileNumber: [null, [this.customValidator.checkMobileNumber]],
    });
  }

  ngOnInit(): void {
    this.dataService.getSetting().subscribe((response) => {
      this.setting = response;
    });

    this.user = this.authService.getAuthUser();
    if (this.user) {
      this.contactForm.get('name')?.setValue(this.user?.displayName);
    }

    this.getOrganizations();

    this.dataService.getEnums().subscribe(
      (response) => {
        if (response) {
          this.periorityList = response.ticketPriority ? response.ticketPriority : [];
        } else {
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        }
      },
      (error: any) => {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  getOrganizations() {
    this.loadingData = true;

    this.dataService.get(ServerApis.getAllOrganizational, {})
      .pipe(
        finalize(() => {
          this.loadingData = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
              if (response.isSuccess) {
                this.organizationList = response.data ? response.data : [];
              } else {
                let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            }, (error: any) => {
            });
  }

  getUnitsOfOrganization() {
    this.loadingUnit = true;

    this.dataService
            .get(ServerApis.getAllOrganizationalUnitByOrganId, {
              organId: this.contactForm.get('organizationId')?.value,
            })
      .pipe(
        finalize(() => {
          this.loadingUnit = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
                if (response.isSuccess) {
                  this.unitList = response.data ? response.data : [];
                } else {
                  let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                  this.toastrService.error(msg);
                }
              }, (error: any) => {
              });
  }

  saveInfo() {
    if (this.contactForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.contactForm.markAllAsTouched();
      return;
    }
    this.isSaving = true;
    let formData = this.contactForm.value;
    this.dataService
            .post(ServerApis.addContact, {
              Id: '',
              Subject: formData.subject,
              Message: formData.message,
              organizationalUnitId: formData.organizationalUnitId,
              name: formData.name ? formData.name : '',
              mobileNumber: formData.mobileNumber ? formData.mobileNumber : '',
              email: formData.email ? formData.email : '',
              CompanyId: '',
              UserId: this.user ? this.user.userId : '',
            })
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
                if (response.isSuccess) {
                  this.toastrService.success('پیام شما با موفقیت ارسال شد.');

                  this.contactForm.reset();
                } else {
                  let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                  this.toastrService.error(msg);
                }
              }, (error: any) => {
              });
  }
}
