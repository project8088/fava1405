import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private router: Router,
    private toastrService: ToastrService,
  ) {}
  addCookie(req: HttpRequest<any>, Cookie: string): HttpRequest<any> {
    return req.clone({
      withCredentials: req.url.includes('worldtimeapi.org') == false ? true : false,
      // setHeaders: {
      //   'Cookie': Cookie
      // }
    });
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(this.addCookie(req, '')).pipe(
      catchError((error) => {
        if (error instanceof HttpErrorResponse) {
          switch ((<HttpErrorResponse>error).status) {
            case 401:
              return this.handle401Error(req, next);
            default:
              return throwError(error);
          }
        } else {
          return throwError(error);
        }
      }),
    );
  }

  handle401Error(req: HttpRequest<any>, next: HttpHandler) {
    // Route to the login page (implementation up to you)
    Swal.fire({
      title: 'خطای دسترسی',
      text: 'شما اجازه دسترسی به این صفحه را ندارید! لطفا وارد حساب کاربری خود شوید.',
      showConfirmButton: true,
      confirmButtonText: 'ورود به حساب کاربری',
      showCancelButton: false,
      allowEscapeKey: false,
      allowOutsideClick: false,
    }).then((result) => {
      if (result.value) this.router.navigate(['/account/login']);
    });
    return throwError('');
  }
}
