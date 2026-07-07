import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ControlValueAccessor, Validators, FormControl } from '@angular/forms';
import { RequireMatch } from '../../custom-validator/requireMatch';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'car-license-plate',
  templateUrl: './car-license-plate.component.html',
  styleUrls: ['./car-license-plate.component.scss'],
  //providers: [
  //  {
  //    provide: NG_VALUE_ACCESSOR,
  //    useExisting: forwardRef(() => InputAutoCompleteComponent),
  //    multi: true
  //  }
  //]
  standalone: false,
})
export class CarLicensePlateComponent extends AppBase implements ControlValueAccessor, OnInit {
  @Input() disabled: boolean = false;
  @Input('required') required: boolean = false;
  @Output('change') change = new EventEmitter<any>();

  letters: string[] = [
    'الف',
    'ب',
    'پ',
    'ت',
    'ث',
    'ج',
    'د',
    'ز',
    'ژ',
    'س',
    'ش',
    'ص',
    'ط',
    'ع',
    'ف',
    'ق',
    'ک',
    'ل',
    'م',
    'ن',
    'و',
    'ه',
    'ی',
  ];

  s2: string = '';
  s1: number = 0;
  s3: number = 0;
  s4: number = 0;
  myControl = new FormControl();
  constructor() {
    super();
  }

  ngOnInit() {
    this.init();
  }

  init() {
    if (this.myControl) {
      if (this.required) {
        this.myControl.setValidators([Validators.required, RequireMatch]);
        this.myControl.updateValueAndValidity();
      }
    }
  }

  private propagateChange = (_: any) => {};

  writeValue(value: any): void {
    if (value !== undefined && value !== null) {
      let plateNumber = value.split(' ');
      this.s1 = plateNumber[0] ? plateNumber[0] : null;
      this.s2 = plateNumber[1] ? plateNumber[1] : null;
      this.s3 = plateNumber[2] ? plateNumber[2] : null;
      this.s4 = plateNumber[3] ? plateNumber[3] : null;
    }
  }
  registerOnChange(fn: any): void {
    // this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    // this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onChange() {
    if (this.change) this.change.emit(this.myControl?.value);
  }

  changeNum() {
    if (this.s1 && this.s2 && this.s3 && this.s4) {
      let plateNumber =
        this.s1.toString() + ' ' + this.s2 + ' ' + this.s3.toString() + ' ' + this.s4.toString();
      this.myControl?.setValue(plateNumber);
    }

    //else {
    //  this.myControl.setValue('');
    //}
  }
}
