import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, finalize, map } from 'rxjs/operators';

import { ApiResult } from '../models/response';
import { AuthUser } from './user.model';
import { LocalStorageService } from '../services/localstorage.service';
import { Router } from '@angular/router';
import { ServerApis } from '../server-apis';
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<AuthUser | null>;
  public currentUser: Observable<AuthUser | null>;

  constructor(
    private http: HttpClient,
    private router: Router,
    private storageService: LocalStorageService,
  ) {
    var user = this.getAuthUser();
    this.currentUserSubject = new BehaviorSubject<AuthUser | null>(user);
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): AuthUser | null {
    return this.currentUserSubject.value;
  }

  /**
   * ورود به حساب کابری
   * @param credentials
   */
  login(credentials: any): Observable<ApiResult<any>> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };

    return this.http.post<ApiResult<any>>(ServerApis.login, credentials, httpOptions).pipe(
      map((resp) => {
        if (resp.isSuccess) {
          this.storeToken(resp.data.access_token, resp.data.refresh_token);
        }
        this.storePermissions(resp.data.permissions);
        return resp;
      }),
      //, catchError(error => {
      //  //return of(error as any);
      //  return throwError(error);
      //})
    );
  }

  storePermissions(permissions: string[]) {
    let p = btoa(btoa(JSON.stringify(permissions ?? [])));
    this.storageService.set('p', p);
  }

  getPermissions() {
    let p = this.storageService.get('p');
    if (p) {
      p = JSON.parse(atob(atob(p)));
      return p || [];
    } else return [];
  }

  checkPermission(permission?: string) {
    if (!permission) return true;
    var userPermissions = this.getPermissions();
    if (userPermissions.indexOf(permission) > -1) return true;
    else return false;
  }

  storeToken(access_token: string, refresh_token: string) {
    this.storageService.set('access_token', access_token);
    this.storageService.set('refresh_token', refresh_token);

    this.currentUserSubject.next(this.getAuthUser());
  }

  refreshToken(): Observable<string> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const refreshToken = this.storageService.get('refresh_token');
    return this.http
      .post(ServerApis.refreshToken, { refreshToken: refreshToken }, { headers: headers })
      .pipe(
        map((response: any) => {
          this.storeToken(response.access_token, response.refresh_token);
          return response.access_token;
        }),
      );
  }

  /**
   * خروج از حساب کاربری
   */
  logout(navigateToHome: boolean = true) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const refreshToken = encodeURIComponent(this.storageService.get('refresh_token'));
    this.http
      .get(ServerApis.logout + `?refreshToken=${refreshToken}`, { headers: headers })
      .pipe(
        map((response) => response || {}),
        catchError((error: HttpErrorResponse) => throwError(error)),
        finalize(() => {
          this.storageService.delete('access_token');
          this.storageService.delete('refresh_token');
          this.currentUserSubject.next(null);

          if (navigateToHome) {
            this.router.navigate(['/']);
          }
        }),
      )
      .subscribe((result) => {
        console.log('logout', result);
      });
  }

  /**
   * دریافت اطلاعات کاربر لاگین شده
   * */
  getAuthUser(): AuthUser | null {
    let token = this.getRawAuthToken();
    if (!token) {
      return null;
    }
    const decodedToken = jwt_decode.jwtDecode<any>(token);
    const roles = this.getDecodedTokenRoles(decodedToken);
    let rootModule = '';
    let admin = false;
    let company = false;
    let jobSeeker = false;
    let citizen = false;
    let card = false;
    let webuser = false;

    if (roles.indexOf('admin') > -1) {
      rootModule = 'admin';
      admin = true;
    } else if (roles.indexOf('company') > -1) {
      rootModule = 'company';
      company = true;
    } else if (roles.indexOf('card') > -1) {
      rootModule = 'card';
      card = true;
    } else if (roles.indexOf('webapiuser') > -1) {
      rootModule = 'webuser';
      webuser = true;
    } else if (roles.indexOf('citizen') > -1) {
      rootModule = 'citizen';
      citizen = true;
    }
    return Object.freeze({
      userId: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
      userName: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
      displayName: decodedToken['DisplayName'],
      roles: roles,
      rootModule: rootModule,
      isAdmin: admin,
      isCompany: company,
      isJobseeker: jobSeeker,
      isCitizen: citizen,
      isCardUser: card,
      userCompanyStatus: +decodedToken['UserCompanyAccountStatusId'],
      rejectDescription: decodedToken['rejectDesription'],
    });
  }

  isAuthUserInRoles(requiredRoles: string[]): boolean {
    const user = this.getAuthUser();
    if (!user || !user.roles) {
      return false;
    }

    if (user.roles.indexOf('admin') >= 0) {
      return true; // The `Admin` role has full access to every pages.
    }

    return requiredRoles.some((requiredRole) => {
      if (user.roles) {
        return user.roles.indexOf(requiredRole.toLowerCase()) >= 0;
      } else {
        return false;
      }
    });
  }

  isAuthUserInRole(requiredRole: string): boolean {
    return this.isAuthUserInRoles([requiredRole]);
  }

  getRawAuthToken() {
    return this.storageService.get('access_token');
  }

  getDecodedTokenRoles(decodedToken: any): string[] {
    const roles = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    if (!roles) {
      return [];
    }

    if (Array.isArray(roles)) {
      return roles.map((role) => role.toLowerCase());
    } else {
      return [roles.toLowerCase()];
    }
  }

  updateUserData(user: AuthUser) {
    this.currentUserSubject.next(user);
  }

  goToDashboard() {
    let user = this.getAuthUser();
    if (!user || !user.roles) {
      this.router.navigate(['/']);
      return;
    }
    if (user.roles.indexOf('admin') > -1) this.router.navigate(['/admin']);
    else if (user.roles.indexOf('company') > -1) this.router.navigate(['/company']);
    else if (user.roles.indexOf('card') > -1) this.router.navigate(['/card']);
    else if (user.roles.indexOf('citizen') > -1) this.router.navigate(['/citizen']);
    else if (user.roles.indexOf('webapiuser') > -1) this.router.navigate(['/webuser']);
    else this.router.navigate(['/']);
  }
}
