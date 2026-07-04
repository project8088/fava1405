import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { catchError, take, filter, switchMap } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Router, RouterStateSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LocalStorageService } from '../services/localstorage.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  isRefreshingToken: boolean = false;
  tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null);

  constructor(
    private authService: AuthService,
    private router: Router,
    private toastrService: ToastrService,
    private storageService: LocalStorageService
  ) {}

  addToken(req: HttpRequest<any>, token: string): HttpRequest<any> {
    return req.clone({ setHeaders: { Authorization: 'Bearer ' + token } });
  }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const accessToken = this.authService.getRawAuthToken();
    if (
      !accessToken ||
      req.url.toLowerCase().includes('worldtimeapi') ||
      req.url.toLowerCase().includes('login') ||
      req.url.toLowerCase().includes('refreshtoken') ||
      req.url.toLowerCase().includes('logout')
    )
      return next.handle(req).pipe(
        catchError((error) => {
          return this.handleError(req, next, error);
        })
      );

    return next.handle(this.addToken(req, accessToken)).pipe(
      catchError((error) => {
        return this.handleErrorWithToken(req, next, error);
      })
    );
  }

  handleError(
    req: HttpRequest<any>,
    next: HttpHandler,
    error: HttpErrorResponse
  ) {
    if (req.url.toLowerCase().includes('worldtimeapi'))
      return throwError(error);

    if (error instanceof HttpErrorResponse) {
      switch ((<HttpErrorResponse>error).status) {
        case 400:
          return this.handle400Error(error);
        case 0:
        case 404:
          return this.handle404Error(req, next, error);
        case 401:
        case 403:
        case 405:
          return this.handleErrorAccessDenied(req, next, error);
        case 500:
          return this.handle500Error(req, next, error);
        default:
          return this.handleReciveDataError(req, next, error);
        //return throwError(error);
      }
    } else {
      return throwError(error);
    }
  }
  handleErrorWithToken(
    req: HttpRequest<any>,
    next: HttpHandler,
    error: HttpErrorResponse
  ) {
    if (error instanceof HttpErrorResponse) {
      switch ((<HttpErrorResponse>error).status) {
        case 400:
          return this.handle400Error(error);
        case 0:
        case 404:
          return this.handle404Error(req, next, error);
        case 401:
        case 403:
          return this.handle401Error(req, next);
        case 405:
          return this.handleErrorAccessDenied(req, next, error);
        case 500:
          return this.handle500Error(req, next, error);
        default:
          return this.handleReciveDataError(req, next, error);
        // return throwError(error);
      }
    } else {
      return throwError(error);
    }
  }

  handleErrorAccessDenied(
    req: HttpRequest<any>,
    next: HttpHandler,
    error: HttpErrorResponse
  ) {
    this.toastrService.error('لطفا وارد حساب کاربری خود شوید!', 'دسترسی ممنوع');
    this.logoutUser();
    const eror = error.error?.message || error.statusText;
    return throwError(eror);
  }

  handle404Error(
    req: HttpRequest<any>,
    next: HttpHandler,
    error: HttpErrorResponse
  ) {
    this.toastrService.error('دسترسی به سرور امکان پذیر نیست!', '404');

    const eror = error.error?.message || error.statusText;
    return throwError(eror);
  }

  handle500Error(
    req: HttpRequest<any>,
    next: HttpHandler,
    error: HttpErrorResponse
  ) {
    this.toastrService.error('متاسفانه خطایی در سرور رخ داده است', 'خطا!');
    const eror = error.error?.message || error.statusText;
    //console.log(eror);
    //  this.startTimer();
    //return of(t as any);
    return throwError(eror);
  }

  handleReciveDataError(
    req: HttpRequest<any>,
    next: HttpHandler,
    error: HttpErrorResponse
  ) {
    this.toastrService.error(
      'متاسفانه دریافت اطلاعات از سرور با خطا مواجه شده است.'
    );
    const eror = error.error?.message || error.statusText;
    //console.log(eror);
    //  this.startTimer();
    //return of(t as any);
    return throwError(eror);
  }

  handle401Error(req: HttpRequest<any>, next: HttpHandler) {
    if (!this.storageService.get('refresh_token')) this.logoutUser();

    if (!this.isRefreshingToken) {
      this.isRefreshingToken = true;

      // Reset here so that the following requests wait until the token
      // comes back from the refreshToken call.
      this.tokenSubject.next(null);

      return this.authService.refreshToken().pipe(
        switchMap((newToken: string) => {
          this.isRefreshingToken = false;
          if (newToken) {
            this.tokenSubject.next(newToken);
            return next.handle(this.addToken(req, newToken));
          }

          // If we don't get a new token, we are in trouble so logout.
          return this.logoutUser();
        }),
        catchError((error) => {
          // If there is an exception calling 'refreshToken', bad news so logout.
          this.isRefreshingToken = false;
          return this.logoutUser();
        })
      );
    } else {
      return this.tokenSubject.pipe(
        filter((token) => token != null),
        take(1),
        switchMap((token) => {
          return next.handle(this.addToken(req, token));
        }),
        catchError((error) => {
          // If there is an exception calling 'refreshToken', bad news so logout.
          this.isRefreshingToken = false;
          return this.logoutUser();
        })
      );
    }
  }

  handle400Error(error) {
    if (
      error &&
      error.status === 400 &&
      error.error &&
      error.error.error === 'invalid_grant'
    ) {
      // If we get a 400 and the error message is 'invalid_grant', the token is no longer valid so logout.
      return this.logoutUser();
    }
    return throwError(error);
  }

  logoutUser() {
    // Route to the login page (implementation up to you)
    this.authService.logout(false);
    this.router.navigate(['/account/login']);
    return throwError('');
  }
}
