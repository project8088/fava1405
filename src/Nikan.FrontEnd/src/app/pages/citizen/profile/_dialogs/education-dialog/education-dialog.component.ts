import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { startWith, map, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { CustomFormValidators } from '@core/custom-validator/form-validation';
import { karjoEducationDto } from '@core/models/citizen/education';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-education-dialog',
  templateUrl: './education-dialog.component.html',
  styleUrls: ['./education-dialog.component.scss'],
  standalone: false,
})
export class CitizenEducationDialogComponent extends AppBase implements OnInit {
  isSaving = false;
  educationForm: FormGroup;
  id: string = '';
  userId?: string;
  loading: boolean = true;

  loadingEnums: boolean = true;
  baseEnums: any = {};

  education?: karjoEducationDto;

  loadingFieldStudies: boolean = false;
  filteredFieldStudies = new Observable<any[]>();
  selectedFieldStudies: any[] = [];

  constructor(
    private matDialogRef: MatDialogRef<CitizenEducationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private customValidator: CustomFormValidators,
  ) {
    super();
    if (_data) {
      this.education = _data.education ? _data.education : '';
      this.userId = _data.userId ? _data.userId : '';
    }

    this.educationForm = this.fb.group({
      grade: ['', [Validators.required]],
      major: [''],
      university: [null],
      dateOfStart: [null],
      dateOfEnd: [null],
    });
  }

  ngOnInit() {
    this.getBaseEnums();

    if (this.education) {
      this.educationForm.setValue({
        grade: {
          key: this.education.gradeId,
          value: this.education.grade,
        },
        major: {
          text: this.education.major ? this.education.major : '',
          value: this.education.majorId ? this.education.majorId : '',
        },
        university: this.education.university,
        dateOfStart: this.education.dateOfStart ? new Date(this.education.dateOfStart) : '',
        dateOfEnd: this.education.dateOfEnd ? new Date(this.education.dateOfEnd) : '',
      });
    }
    //رشته های تحصیلی
    this.filteredFieldStudies = this.educationForm.get('major')!.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((value) => {
        return this._filterFieldStudies(value);
      }),
    );
  }

  /**
   * دریافت اطلاعات  پایه
   *
   * */
  getBaseEnums() {
    this.loadingEnums = true;
    this.dataService.getEnums().subscribe(
      (response) => {
        this.loadingEnums = false;
        if (response) {
          this.baseEnums.educationLevel = response.educationLevel ? response.educationLevel : [];

          var grade: any[] = [];
          this.baseEnums.educationLevel.forEach((item: any) => {
            if (+item.key > 19) grade.push(item);
          });
          this.baseEnums.educationLevel = grade;
        } else {
          let msg = response.messages
            ? response.messages
            : 'در یافت اطلاعات از سرور با خطا مواجه شده است.';
          this.toastrService.error(msg);
        }
      },
      (error: any) => {
        this.toastrService.error('خطا در ارتباط با سرور!');
        this.loadingEnums = false;
      },
    );
  }

  /**
   * جستجو رشته های تحصیلی
   * @param value
   */
  private _filterFieldStudies(value: string) {
    if (!value || typeof value !== 'string') return this.filteredFieldStudies;

    const filterValue = value.toLowerCase();

    this.loadingFieldStudies = true;
    return this.dataService
      .get(ServerApis.getMajors, {
        query: filterValue,
        offset: 0,
        count: 20,
      })
      .pipe(
        map(
          (response) => {
            this.loadingFieldStudies = false;
            if (response.isSuccess) return response.data;
            else {
              let msg = response.messages
                ? response.messages
                : 'در یافت اطلاعات از سرور با خطا مواجه شده است.';
              this.toastrService.error(msg);
            }
          },
          (error: any) => {
            this.toastrService.error('خطا در ارتباط با سرور!');
            this.loadingFieldStudies = false;
          },
        ),
      );
  }

  displayFn(item: any): string {
    return item && item.text ? item.text : '';
  }

  changeGrade() {
    if (this.educationForm.get('grade')?.value.key > 0) {
      this.educationForm.get('major')?.setValidators([Validators.required]);
      this.educationForm.get('major')?.updateValueAndValidity();
    } else {
      this.educationForm.get('major')?.clearValidators();
      this.educationForm.get('major')?.setValue(null);
      this.educationForm.get('major')?.updateValueAndValidity();

      this.educationForm.get('university')?.setValue(null);
    }
  }

  saveInfo() {
    if (this.educationForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.educationForm.markAllAsTouched();
      return;
    }

    var formValue = this.educationForm.value;

    this.isSaving = true;
    this.dataService
      .post(ServerApis.saveCitizenEducation, {
        gradeId: +formValue.grade.key,
        grade: formValue.grade.text,
        major: formValue.major ? formValue.major.text : null,
        majorId: formValue.major ? +formValue.major.key : null,

        university: formValue.university ? formValue.university : '',
        dateOfStart: formValue.dateOfStart
          ? this.dataService.formatDate(formValue.dateOfStart)
          : '',
        dateOfEnd: formValue.dateOfEnd ? this.dataService.formatDate(formValue.dateOfEnd) : '',
        userId: this.userId,
        id: this.education ? +this.education?.id! : null,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response && response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
            this.matDialogRef.close(response.data);
          } else {
            let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
            this.toastrService.error(msg);
          }
        },
        (error: any) => {
          this.isSaving = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        },
      );
  }

  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.key === c2.key : c1 === c2;
  }
}
