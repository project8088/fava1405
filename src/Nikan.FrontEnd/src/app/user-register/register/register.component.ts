import { ApiResult } from '@core/models/models';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { HelperService } from '@core/services/helper.service';
import { Observable } from 'rxjs';
import { ServerApis } from '@core/server-apis';
import { UserRegisterService } from '../userregister.service';
import { AppBase } from '@app/app.base';
import { TimerComponent } from '@app/shared/timer/timer.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  standalone: false,
})
export class RegisterComponent extends AppBase implements OnInit {
  serviceId: number = 0;
  //form 1
  firstFormGroup: FormGroup;
  maritalStatus: any[] = [];

  educationStatues: any[] = [];
  educationGroups: any[] = [];
  educationLevel: any[] = [];
  jobGroup: any[] = [];

  states: any[] = [];
  cities = new Observable<any>();
  isfahanCities: any[] = [];

  // form4
  forthFormGroup: FormGroup;
  passwordQuestion: any[] = [];

  // form 5
  fifthFormGroup: FormGroup;

  loading?: boolean;

  today = new Date();
  codeSent: boolean = false;
  @ViewChild('codeTimer', { static: false }) codeTimer!: TimerComponent;
  mobileNumber;

  constructor(
    private AccountService: UserRegisterService,
    private helperService: HelperService,
    private customValidator: CustomFormValidators,
  ) {
    super();
    this.dataService.getEnums().subscribe((data) => {
      this.maritalStatus = this.getListOptions(data['maritalStatus']);
      //this.educationStatues = this.getListOptions(data['educationStatues']);
      //this.educationLevel = this.getListOptions(data['educationLevel']);
      //this.jobGroup = this.getListOptions(data['jobGroup']);
      // this.passwordQuestion = this.getListOptions(data['passwordQuestion']);
    });
    const userPreRegisterData = this.AccountService.getUserPreRegisterData();

    this.mobileNumber = userPreRegisterData.mobileNumber;

    this.getEducationGroups();
    this.firstFormGroup = this.fb.group({
      serviceId: [null, [Validators.required]],
      firstName: [null, [Validators.required, this.customValidator.checkPersianCharacters]],
      lastName: [null, [Validators.required, this.customValidator.checkPersianCharacters]],
      mariageStatus: [null, [Validators.required]],
      fatherName: [null, [Validators.required, this.customValidator.checkPersianCharacters]],
      birthDate: [null, [Validators.required]],
      gender: [null, [Validators.required]],
      cityId: [null],
    });

    this.route.queryParams.subscribe((params) => {
      if (params['serviceId']) {
        this.serviceId = params['serviceId'];

        this.firstFormGroup.patchValue({ serviceId: params['serviceId'] });
      } else this.router.navigate(['/']);
    });

    this.forthFormGroup = this.fb.group({
      // passwordQuestion: [null, [Validators.required]],
      //  passwordAnswer: [null, [Validators.required]],
      password: [null, [Validators.required, Validators.minLength(6)]],
      confirmPassword: [null],
    });

    // this.forthFormGroup.get('password')?.setValidators([Validators.required, Validators.minLength(6),()=>{
    //   if(this.forthFormGroup)
    // }]);
    this.forthFormGroup.get('confirmPassword')?.setValidators([
      Validators.required,
      () => {
        if (
          this.forthFormGroup.get('password')?.value !==
          this.forthFormGroup.get('confirmPassword')?.value
        )
          return {
            invalid: true,
          };
        else return null;
      },
    ]);

    this.fifthFormGroup = this.fb.group({
      verifyCode: [null, [Validators.required]],
    });

    this.helperService.getIsfahanCities().subscribe((data) => {
      this.isfahanCities = data;
    });
  }

  getEducationGroups() {
    this.dataService.get(ServerApis.getEducationGroups).subscribe(
      (response) => {
        if (response) {
          this.educationGroups = response.data ?? [];
        }
      },
      (error:any) => {
        this.toastrService.error('خطا در ارتباط با سرور!');
      },
    );
  }

  confirmPassValidator(predicate: any) {
    return (formControl: any) => {
      if (!formControl.parent) {
        return null;
      }
      if (predicate()) {
        return Validators.required(formControl);
      }
      return null;
    };
  }

  ngOnInit(): void {}
  finalSubmit(): void {
    const form1 = this.firstFormGroup.getRawValue();
    const form4 = this.forthFormGroup.getRawValue();
    const form5 = this.fifthFormGroup.getRawValue();
    const userPreRegisterData = this.AccountService.getUserPreRegisterData();

    if (form1.birthDate) form1.birthDate = this.dataService.formatDate(form1.birthDate);

    const finalData = {
      ...form1,

      ...form4,
      ...form5,
      ...userPreRegisterData,
    };

    this.dataService.post(ServerApis.citizenRegister, finalData).subscribe(
      (data: ApiResult<any>) => {
        if (data.isSuccess) {
          this.toastrService.success(data.messages);

          this.authService.storeToken(data.data.access_token, data.data.refresh_token);
          debugger;
          this.router.navigateByUrl('/redirect?serviceId=' + this.serviceId);
        } else this.toastrService.error(data.messages);
      },
      (error:any) => {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  sendVerificationCode(e: any) {
    this.codeSent = true;
    this.codeTimer.startTimer().then(() => {
      this.codeSent = false;
    });

    e.preventDefault();
    const userPreRegisterData = this.AccountService.getUserPreRegisterData();
    this.dataService.post(ServerApis.reSendVerfiyCode, userPreRegisterData).subscribe(
      (data: ApiResult<any>) => {
        if (data.isSuccess) {
          this.toastrService.success(data.messages);
        } else {
          this.toastrService.error(data.messages);
          this.codeSent = false;
        }
      },
      (error:any) => {
        this.codeSent = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  getListOptions(options: { key: number; text: string }[]) {
    return options.map((el: { key: number; text: string }) => {
      return { value: String(el.key), text: el.text };
    });
  }
}
