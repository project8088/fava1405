import {
  Component,
  OnInit,
  Input,
  forwardRef,
  Optional,
  Self,
  Output,
  EventEmitter,
} from '@angular/core';
import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  Validators,
  FormControl,
  NgControl,
} from '@angular/forms';
import { RequireMatch } from '../../custom-validator/requireMatch';
import { Observable } from 'rxjs';
import { ServerApis } from '../../server-apis';
import { startWith, map } from 'rxjs/operators';
import { MatAutocompleteTrigger, MatAutocomplete } from '@angular/material/autocomplete';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'input-company',
  templateUrl: './input-company.component.html',
  styleUrls: ['./input-company.component.scss'],,
  //providers: [
  //  {
  //    provide: NG_VALUE_ACCESSOR,
  //    useExisting: forwardRef(() => InputAutoCompleteComponent),
  //    multi: true
  //  }
  //]
    standalone: false
})
export class InputCompanyAutoCompleteComponent extends AppBase implements ControlValueAccessor, OnInit {
  @Input() disabled: boolean;

  @Input('label') label: string = 'شرکت';
  @Input('required') required: boolean = false;

  List: any[] = [];

  @Output('optionSelected') optionSelected: EventEmitter<any>;

  loading: boolean;
  filteredList: Observable<any[]>;

  constructor(
    @Self() public ngControl: NgControl,
  ) {
      super();
    ngControl.valueAccessor = this;
    //this.ngControl = new FormControl(null, [RequireMatch]);
  }

  ngOnInit() {
    this.getCompanyList();

    if (this.ngControl.control) {
      if (this.required) {
        this.ngControl.control.setValidators([Validators.required, RequireMatch]);
        this.ngControl.control.updateValueAndValidity();
      }
    }
  }

  private propagateChange = (_: any) => {};

  writeValue(value: any): void {
    //if (value !== undefined) {
    //  this.ngControl.control.setValue(value.toString());
    //}
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

  getCompanyList() {
    if (this.ngControl.control) {
      this.ngControl.control.reset('');
      this.List = [];
    }

    this.loading = true;
    this.dataService.get(ServerApis.getListCompany, {}).subscribe(
      (response) => {
        this.loading = false;
        this.List = response.data ? response.data : [];
        this.filteredList = this.ngControl.control.valueChanges.pipe(
          startWith(''),
          map((value) => {
            if (value === null || value === undefined) return '';
            else if (typeof value === 'string') return value;
            else if (value.text) return value.text;
            else return '';
          }),
          map((value) => (value ? this._filter(value, this.List) : this.List)),
        );
      },
      (error) => {
        this.loading = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  /**
   * فیلتر بر روی لیست ها برای اتوکامپلیت
   * @param name  عبارت جستجو
   * @param list   لیست
   */
  private _filter(name: string, list): any[] {
    const filterValue = name.toLowerCase();
    return list.filter((option) => option.text.toLowerCase().indexOf(filterValue) === 0);
  }

  clearItem(trigger: MatAutocompleteTrigger, auto: MatAutocomplete) {
    setTimeout((_) => {
      auto.options.forEach((item) => {
        item.deselect();
      });
      this.ngControl.control.reset('');
      trigger.openPanel();
    }, 100);
  }

  /**
   * for bind object in autocomplete
   * @param item
   */
  displayFn(item): string {
    return item && item.text ? item.text : '';
  }

  onChange() {
    if (this.optionSelected) this.optionSelected.emit(this.ngControl.control.value);
  }
}
