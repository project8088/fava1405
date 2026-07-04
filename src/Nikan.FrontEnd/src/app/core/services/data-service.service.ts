import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { ApiResult } from '../models/response';
import { BaseDataModel } from '../models/base-data-model';
import { Injectable } from '@angular/core';
import { LocalStorageService } from './localstorage.service';
import { Router } from '@angular/router';
import { ServerApis } from '../server-apis';
import { SiteSettingViewModel } from '../models/setting';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};
@Injectable({
  providedIn: 'root',
})
export class DataService {
  constructor(
    private http: HttpClient,
    private localStorageService: LocalStorageService,
    private router: Router,
    private toastrService: ToastrService,
  ) {
    this.getBaseEnums().subscribe();
    this.getSiteSettingFromServer().subscribe();
  }

  /**
   * post data to api Rest
   * @param url
   * @param dataToPost
   */
  post(url: string, dataToPost: any = ''): Observable<ApiResult<any>> {
    return this.http.post<ApiResult<any>>(url, dataToPost, httpOptions).pipe(
      map((response) => {
        return response;
      }),
    );
  }

  /**
   * post form with multipart/form data
   * auto convert json to form data
   * @param url
   * @param formData as json
   */
  postFormData(url: string, formData: any = ''): Observable<ApiResult<any>> {
    var postForm = new FormData();
    for (var prop in formData) {
      if (formData[prop].name) postForm.append(prop, formData[prop], formData[prop].name);
      else postForm.append(prop, formData[prop]);
    }
    return this.http.post<ApiResult<any>>(url, postForm).pipe(
      map((response) => {
        return response;
      }),
    );
  }

  /**
   * get data from api Rest
   * @param url
   * @param params
   */
  get(url: string, params: any = ''): Observable<ApiResult<any>> {
    return this.http.get<ApiResult<any>>(url, { params: params }).pipe(
      //retry(3), // retry a failed request up to 3 times
      map((response) => {
        return response;
      }),
      //  tap(data => console.log('fetched http successfully')),
      //  catchError(this.handleError)
    );
  }

  /**
   * delete from server
   * @param url
   * @param params
   */
  delete(url: string, params: any = ''): Observable<ApiResult<any>> {
    //const urlParam = url + JSON.stringify(params);
    return this.http.delete<ApiResult<any>>(url, { params: params }).pipe(
      // retry(3), // retry a failed request up to 3 times
      map((response) => {
        return response;
      }),
    );
  }

  downloadFile(
    url: string,
    params: any,
    type = 'application/vnd.ms-excel',
    fileName = 'export.xls',
  ) {
    this.http
      .get<ApiResult<any>>(url, {
        params,
        responseType: 'arraybuffer' as any,
      })
      .subscribe(
        (response) => {
          this.downLoadFile(response, type, fileName);
        },
        (error) => this.handleError(error),
      );
  }

  private handleError(error: HttpErrorResponse) {
    debugger;
    if (error.status === 0) {
      this.toastrService.error('عدم دسترسی');
    } else if (error.status === 405) {
      this.toastrService.error('عدم دسترسی');
    } else {
      this.toastrService.error('عدم دسترسی');
    }
    // Return an observable with a user-facing error message.
    //return throwError(() => new Error('Something bad happened; please try again later.'));
  }

  /**
   * Method is use to download file.
   * @param data - Array Buffer data
   * @param type - type of the document.
   */
  private downLoadFile(data: any, type: string, fileName: string) {
    debugger;
    this.toastrService.info('در حال دریافت فایل لطفا منتظر بمانید');
    let blob = new File([data], fileName, { type: type, endings: 'native' });
    let url = window.URL.createObjectURL(blob);

    var a: any = document.createElement('a');
    document.body.appendChild(a);
    a.style = 'display: none';
    a.href = url;
    a.download = fileName;
    a.click();
    window.URL.revokeObjectURL(url);
    a.remove();
  }

  /**
   * دریافت تاریخ جاری از سرور
   * worldtimeapi
   * */
  getCurrentDateTime(): Observable<any> {
    return this.http.get<any>(ServerApis.getCurrentDateTime).pipe(
      // retry(3), // retry a failed request up to 3 times
      map((response) => {
        return response;
      }),
    );
  }

  getCaptchaImage(param: any): Observable<any> {
    return this.http.get(ServerApis.captcha, { params: param, responseType: 'blob' });
  }

  /**
   * تبدیل تاریخ به فرمت
   * YYYY/MM/DD
   */
  formatDate(date) {
    var d = new Date(date),
      month = '' + (d.getMonth() + 1),
      day = '' + d.getDate(),
      year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
  }

  downloadPlacementRegisterRequestFile() {
    var url = 'assets/register-request.pdf';
    if (ServerApis.baseUrl == '/') url = '/ui/' + url;

    return this.http.get(url, { responseType: 'blob' }).pipe(
      map((res) => {
        return res;
      }),
    );
  }

  /**
   * دریافت لیست enum ها از سرور
   * */
  protected getBaseEnums(): Observable<any> {
    return this.http.get(ServerApis.getlistBase).pipe(
      map((response: any) => {
        response = this.convertKeyToInt(response);
        this.localStorageService.set('enums', response);
        return response;
      }),
    );
  }

  /**
   * دریافت اطلاعات پایه از لوکال استوریج
   * */
  getEnums(): Observable<any> {
    let enums = this.localStorageService.get('enums');
    if (enums) return of(enums);
    else return this.getBaseEnums();
  }

  getTextOfEnum(objectName, key): Observable<string> {
    return this.getEnums().pipe(
      map((response) => {
        let enums = response;
        for (let property in enums) {
          if (property == objectName) {
            return enums[property].find((v) => v.key == key).text;
          }
        }
      }),
    );
  }

  /**
   * دریافت تنظیمات سایت از سمت سرور
   * */
  protected getSiteSettingFromServer(): Observable<SiteSettingViewModel> {
    return this.http.get(ServerApis.getSiteInfo).pipe(
      map((response: ApiResult<SiteSettingViewModel>) => {
        if (response.isSuccess) {
          var result = response.data;
          this.localStorageService.set('setting', result);
          return result;
        } else {
          return null;
        }
      }),
    );
  }

  /**
   * دریافت تنظیمات سایت
   * */
  getSetting(): Observable<SiteSettingViewModel> {
    let setting = this.localStorageService.get('setting');
    if (setting) return of(setting);
    else return this.getSiteSettingFromServer();
  }

  /**
   * convert key<string> to key<int>
   *
   * @param objects
   */
  convertKeyToInt(objects) {
    for (let property in objects) {
      if (objects[property] instanceof Array) {
        objects[property].forEach((item) => {
          if (item.key == null || item.key == undefined || item.key == '') item.key = null;
          else item.key = parseInt(item.key);
        });
      }
    }
    return objects;
  }

  toCamelCase(o) {
    var newO, origKey, newKey, value;
    if (o instanceof Array) {
      return o.map((value) => {
        if (typeof value === 'object') {
          value = this.toCamelCase(value);
        }
        return value;
      });
    } else {
      newO = {};
      for (origKey in o) {
        if (o.hasOwnProperty(origKey)) {
          newKey = (origKey.charAt(0).toLowerCase() + origKey.slice(1) || origKey).toString();
          value = o[origKey];
          if (value instanceof Array || (value !== null && value.constructor === Object)) {
            value = this.toCamelCase(value);
          }
          newO[newKey] = value;
        }
      }
    }
    return newO;
  }
}
