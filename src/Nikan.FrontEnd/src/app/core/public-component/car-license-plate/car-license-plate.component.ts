import { Component, OnInit, Input, Self, Output, EventEmitter } from '@angular/core';
import { ControlValueAccessor, Validators, NgControl } from '@angular/forms';
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
  @Input() disabled: boolean;
  @Input('required') required: boolean = false;
  @Output('change') change: EventEmitter<any>;

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
  s1: number;
  s3: number;
  s4: number;

  constructor(@Self() public ngControl: NgControl) {
    super();
    ngControl.valueAccessor = this;
    //this.ngControl = new FormControl(null, [RequireMatch]);
  }

  ngOnInit() {
    this.init();
  }

  init() {
    if (this.ngControl.control) {
      if (this.required) {
        this.ngControl.control.setValidators([Validators.required, RequireMatch]);
        this.ngControl.control.updateValueAndValidity();
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
    if (this.change) this.change.emit(this.ngControl.control.value);
  }

  changeNum() {
    if (this.s1 && this.s2 && this.s3 && this.s4) {
      let plateNumber =
        this.s1.toString() + ' ' + this.s2 + ' ' + this.s3.toString() + ' ' + this.s4.toString();
      this.ngControl.control.setValue(plateNumber);
    }

    //else {
    //  this.ngControl.control.setValue('');
    //}
  }
}
