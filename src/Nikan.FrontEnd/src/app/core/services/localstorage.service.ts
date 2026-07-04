import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageService {
  get(key) {
    var data = window.localStorage.getItem(key);
    try {
      return JSON.parse(data);
    } catch {
      return null;
    }
  }

  set(key, data) {
    window.localStorage.setItem(key, JSON.stringify(data));
  }

  delete(key) {
    window.localStorage.removeItem(key);
  }
}
