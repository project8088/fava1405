import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: 'input[persianOnly]',
})
export class OnlyPersianCharacterDirective {
  constructor(private _el: ElementRef) {}

  @HostListener('input', ['$event']) onInputChange(event) {
    const initalValue = this._el.nativeElement.value;
    this._el.nativeElement.value = initalValue.replace(/^([\u0600-\u06FF]+\s*)+$/g, '');
    if (initalValue !== this._el.nativeElement.value) {
      event.stopPropagation();
    }
  }
}
