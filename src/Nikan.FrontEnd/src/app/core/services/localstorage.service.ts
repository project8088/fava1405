import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageService {
  get(key: string) {
    var data: any = window.localStorage.getItem(key);
    try {
      return JSON.parse(data);
    } catch {
      return null;
    }
  }

  set(key: string, data: any) {
    window.localStorage.setItem(key, JSON.stringify(data));
  }

  delete(key: string) {
    window.localStorage.removeItem(key);
  }
}
