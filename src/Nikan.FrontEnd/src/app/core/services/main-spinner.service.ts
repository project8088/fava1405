import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MainSpinnerService {
  show(text?: string) {
    let el: any = document.querySelector('#MainRootLoading');
    if (el) {
      el.style.display = 'flex';
    }
  }

  hide(text?: string) {
    setTimeout(() => {
      let el: any = document.querySelector('#MainRootLoading');
      if (el) {
        el.style.display = 'none';
      }
    }, 100);
  }
}
