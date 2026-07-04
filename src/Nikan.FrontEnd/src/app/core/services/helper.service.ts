import { ApiResult } from '../models/models';
import { DataService } from './data-service.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ServerApis } from '../server-apis';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class HelperService {
  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
  ) {}

  /**
   *دریافت لیست استان ها
   * */
  getProvinces(): Observable<Object> {
    return this.dataService.get(ServerApis.getProvinces).pipe(
      map((response: ApiResult<Object>) => {
        if (response.isSuccess) {
          var result = response.data;
          return result;
        } else {
          return null;
        }
      }),
    );
  }
  /**
   *دریافت لیست شهرهای اصفهان
   * */
  getIsfahanCities(): Observable<Array<Object>> {
    return this.dataService.get(ServerApis.getIsFahanCites).pipe(
      map((response: ApiResult<Array<Object>>) => {
        if (response.isSuccess) {
          var result = response.data;
          return result;
        } else {
          return null;
        }
      }),
    );
  }

  /**
   *دریافت لیست شهر ها
   * */
  getCities() {
    return this.dataService.get(ServerApis.getAllCites).pipe(
      map((response: ApiResult<[]>) => {
        if (response.isSuccess) {
          var result = response.data;
          return result;
        } else {
          return null;
        }
      }),
    );
  }

  /**
   * دریافت لیست شهرستان ها
   * */

  getCitesByParent(value: string) {
    const filterValue = value.toLowerCase();

    return this.dataService
      .get(ServerApis.getCitesByParent, {
        parentId: filterValue,
      })
      .pipe(
        map(
          (response) => {
            if (response.isSuccess) return response.data;
            else {
              let msg = response.messages
                ? response.messages
                : 'در یافت اطلاعات از سرور با خطا مواجه شده است.';
              this.toastrService.error(msg);
            }
          },
          (error) => {
            this.toastrService.error('خطا در ارتباط با سرور!');
          },
        ),
      );
  }
}
