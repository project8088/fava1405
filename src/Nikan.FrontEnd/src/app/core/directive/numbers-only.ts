import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: 'input[numbersOnly]',
})
export class OnlyNumberDirective {
  constructor(private _el: ElementRef) {}

  @HostListener('input', ['$event']) onInputChange(event) {
    const initalValue = this._el.nativeElement.value;
    this._el.nativeElement.value = initalValue.replace(/[^0-9]*/g, '');
    if (initalValue !== this._el.nativeElement.value) {
      event.stopPropagation();
    }
  }
}

@Directive({
  selector: 'input[decimalOnly]',
})
export class OnlyDecimalDirective {
  constructor(private _el: ElementRef) {}

  @HostListener('input', ['$event']) onInputChange(event) {
    const initalValue = this._el.nativeElement.value;
    var num = initalValue.replace(/[^0-9/.]*/g, '');
    if (num.match(/\./g)) if (num.match(/\./g).length > 1) num = parseFloat(num);
    this._el.nativeElement.value = num;
    if (initalValue !== this._el.nativeElement.value) {
      event.stopPropagation();
    }
  }
}
