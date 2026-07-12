import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { HelperService } from '@core/services/helper.service';
import { Observable, finalize } from 'rxjs';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'citizen-address-info',
  templateUrl: './address-info.component.html',
  styleUrls: ['./address-info.component.scss'],
  standalone: false,
})
export class AdminCitizenAddressInfoComponent extends AppBase implements OnInit {
  editMode: boolean = false;
  editModeWork: boolean = false;
  loading?: boolean;

  isfahanCities: any[] = [];
  userCode: string = '';
  homeForm: FormGroup;
  isSavingHome: boolean = false;
  loadingHome: boolean = false;

  workForm: FormGroup;
  isSavingWork: boolean = false;
  loadingWork: boolean = false;
  states: any[] = [];
  cities = new Observable<any[]>();
  workCityText: string = '';

  constructor(private helperService: HelperService) {
    super();
    this.homeForm = this.fb.group({
      phone: ['', [Validators.required]],
      region: [null],
      id: [null],
      street: [null, [Validators.required]],
      alley: [null],
      plaque: [null, [Validators.required]],
      cityId: [null, [Validators.required]],
      postalCode: [null, [Validators.required]],
    });
    this.workForm = this.fb.group({
      phone: ['', [Validators.required]],
      region: [null],
      id: [null],
      street: [null, [Validators.required]],
      alley: [null],
      plaque: [null],
      stateId: [null, [Validators.required]],
      cityId: [null, [Validators.required]],
      postalCode: [null, [Validators.required]],
    });

    this.route.params.subscribe((p) => {
      if (p['id'] != '0' && p['id'])
        this.route.params.subscribe((p) => {
          if (p['id'] != '0' && p['id']) this.userCode = p['id'];

          this.helperService.getIsfahanCities().subscribe((data) => {
            this.isfahanCities = data;
          });

          this.loadHomeData();
          this.loadWorkData();
        });
    });
  }
  ngOnInit(): void {}

  toggleHomeEditMode() {
    this.editMode = !this.editMode;
  }
  toggleWorkEditMode() {
    this.editModeWork = !this.editModeWork;
  }

  haveHomeAddress() {
    let form = this.homeForm.value;
    const values = Object.values(form).filter((el) => el);

    return !!values.length;
  }

  haveWorkAddress() {
    let form = this.workForm.value;
    const values = Object.values(form).filter((el) => el);

    return !!values.length;
  }

  loadHomeData() {
    this.loadingHome = true;
    this.dataService
            .get(ServerApis.getCitizenHomeAddressByAdmin, {
              userCode: this.userCode,
            })
      .pipe(
        finalize(() => {
          this.loadingHome = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((data) => {
              if (data.data) {
                this.homeForm.patchValue({
                  id: data.data.id,
                  region: data.data.region,
                  phone: data.data.phone,
                  street: data.data.street,
                  alley: data.data.alley,
                  plaque: data.data.plaque,
                  cityId: String(data.data.cityId),
                  postalCode: data.data.postalCode,
                });
              }
            });
  }

  loadWorkData() {
    this.helperService.getProvinces().subscribe((data) => {
      this.states = data as [];
    });

    this.cities = this.workForm.get('stateId')!.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((value) => {
        return this.helperService.getCitesByParent(value).pipe(map((data) => data));
      }),
    );

    this.loadingWork = true;
    this.dataService
            .get(ServerApis.getCitizenOfficeAddressByAdmin, {
              userCode: this.userCode,
            })
      .pipe(
        finalize(() => {
          this.loadingWork = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((data) => {
              if (data.data) {
                this.workForm.patchValue({
                  id: data.data.id,
                  region: data.data.region,
                  phone: data.data.phone,
                  street: data.data.street,
                  alley: data.data.alley,
                  plaque: data.data.plaque,
                  cityId: String(data.data.cityId),
                  postalCode: data.data.postalCode,
                  stateId: data.data.city.parentValue,
                });
              }

              this.cities = this.workForm.get('stateId')!.valueChanges.pipe(
                startWith(data.data.city.parentValue),
                debounceTime(400),
                distinctUntilChanged(),
                switchMap((value) => {
                  return this.helperService.getCitesByParent(value);
                }),
              );

              this.cities.subscribe((data: any[]) => {
                if (data.length) {
                  this.workCityText = data.find(
                    (el: any) => el.key === this.workForm.controls['cityId'].value,
                  ).text;
                }
              });
            });
  }

  saveHomeAddress() {
    const form = this.homeForm.getRawValue();
    this.saveAddress(form, 2);
  }
  saveOfficeAddress() {
    const form = this.workForm.getRawValue();
    this.saveAddress(form, 1);
  }

  saveAddress(form: any, addressType: number) {
    if (addressType === 1) this.isSavingHome = true;
    else this.isSavingWork = true;
    form.userCode = this.userCode;
    return this.dataService
          .post(ServerApis.addOrUpdteCitizenAddress, {
            ...form,
            addressType,
          })
    .pipe(
      finalize(() => {
        this.isSavingHome = false;
        this.chdr.detectChanges();
      }),
    )
    .subscribe((response) => {
              if (response && response.isSuccess) {
                this.editMode = false;
                this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
              } else {
                let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
              this.isSavingWork = false;
            }, (error: any) => {
              this.isSavingWork = false;
              
            });
  }

  getHomeCityText() {
    if (this.isfahanCities && this.isfahanCities.length) {
      return this.isfahanCities.find(
        (el: any) => +el.key === this.homeForm.controls['cityId'].value,
      ).text;
    } else return [];
  }

  getWorkState() {
    if (this.states?.length) {
      return this.states.find((el: any) => el.key === this.workForm.controls['stateId'].value).text;
    }
  }

  getWorkCity() {
    return this.workCityText;
  }
}
