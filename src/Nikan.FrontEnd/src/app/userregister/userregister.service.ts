import { BehaviorSubject } from 'rxjs';
import { DataService } from '../core/services/data-service.service';
import { Injectable } from '@angular/core';
import { LocalStorageService } from '../core/services/localstorage.service';
import { ServerApis } from '../core/server-apis';

@Injectable({
  providedIn: 'root',
})
export class UserRegisterService {
  private UserPreRegisterData: BehaviorSubject<{
    mobileNumber: string;
    nationCode: string;
    nationality: number;
  }> = new BehaviorSubject(null);
  constructor(
    private dataService: DataService,
    private localStorageService: LocalStorageService,
  ) {}
  setUserPreRegisterData(data: { mobileNumber: string; nationCode: string; nationality: number }) {
    this.localStorageService.set('UserPreRegisterData', data);
    this.UserPreRegisterData.next(data);
  }

  getUserPreRegisterData() {
    if (this.UserPreRegisterData.value) return this.UserPreRegisterData.value;
    else return this.localStorageService.get('UserPreRegisterData');
  }
}
