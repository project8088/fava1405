import { Component, OnInit, Inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import {
  MatAutocompleteSelectedEvent,
  MatAutocompleteTrigger,
  MatAutocomplete,
} from '@angular/material/autocomplete';
import { Observable } from 'rxjs';
import { startWith, map, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { RequireMatch } from '@core/custom-validator/requireMatch';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { BaseDataModel } from '../../../../core/models/base-data-model';
import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';

@Component({
  selector: 'admin-register-company',
  templateUrl: './register-company.component.html',
  styleUrls: ['./register-company.component.scss'],
})
export class AdminRegisterCompanyComponent implements OnInit {
  isSaving: boolean;
  registerForm: FormGroup;
  id: string;
  loading: boolean = true;

  captchaImage: any;
  loadingCaptcha: boolean = true;

  loadingState: boolean;
  stateList: BaseDataModel[] = [];
  filteredState: Observable<any[]>;

  constructor(
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private dataService: DataService,
    private customValidator: CustomFormValidators,
    private router: Router,
  ) {
    this.registerForm = this.fb.group(
      {
        companyName: [null, [Validators.required, Validators.maxLength(500)]],
        englishName: [
          null,
          [
            Validators.required,
            Validators.maxLength(40),
            this.customValidator.checkEnglishCharacters,
          ],
        ],
        companyRepresentative: [
          null,
          [
            Validators.required,
            Validators.maxLength(100),
            this.customValidator.checkPersianCharacters,
          ],
        ],
        establishedYear: [
          null,
          [Validators.required, Validators.maxLength(4), Validators.minLength(4)],
        ],
        mobileNumber: [null, [Validators.required, this.customValidator.checkMobileNumber]],
        email: [null, [Validators.required, this.customValidator.checkEmail]],
        txtTinNo: [null, [Validators.required]],
        txtRegNO: [null, [Validators.required]],
        userName: [null, [Validators.required, this.customValidator.checkEnglishCharacters]],
        password: [null, [Validators.required]],
        confirmPassword: [null, [Validators.required]],
      },
      { validator: this.checkPasswords },
    );
  }

  /**
   * بررسی یکی بودن کلمه عبور و تائید آن
   */
  checkPasswords(group: FormGroup) {
    let pass = group.controls.password.value;
    let confirmPassword = group.controls.confirmPassword.value;

    return pass === confirmPassword ? null : { notSame: true };
  }

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

  displayFn(item): string {
    return item && item.text ? item.text : '';
  }

  saveInfo() {
    if (this.registerForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.registerForm.markAllAsTouched();
      return false;
    }

    var formValue = this.registerForm.value;

    this.isSaving = true;
    this.dataService
      .post(ServerApis.companyRegisterAsync, {
        CompanyName: formValue.companyName,
        EnglishName: formValue.englishName,
        CompanyRepresentative: formValue.companyRepresentative,
        EstablishedYear: formValue.establishedYear,
        TxtTinNo: formValue.txtTinNo,
        TxtRegNO: formValue.txtRegNO,
        MobileNumber: formValue.mobileNumber,
        Email: formValue.email,
        UserName: formValue.userName,
        Password: formValue.password,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response && response.isSuccess) {
            this.toastrService.success('ثبت نام شما با موفقیت انجام شد.');
            this.router.navigate(['/admin/companies']);
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.isSaving = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
  }

  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.key === c2.key : c1 === c2;
  }
}
