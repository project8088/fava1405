import { Injectable } from '@angular/core';
import {
  Router,
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivateChild,
} from '@angular/router';
import { AuthService } from './auth.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate, CanActivateChild {
  constructor(
    private router: Router,
    private authService: AuthService,
    private toastrService: ToastrService,
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    var rootUrl = route.url[0].path;
    return this.chechGuard(rootUrl, state);
  }

  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.chechGuardChild(state, route);
  }

  chechGuard(rootUrl: string, state: RouterStateSnapshot) {
    var user = this.authService.getAuthUser();
    if (!user) return false;
    if (user.rootModule == rootUrl || user.isAdmin) {
      return true;
    }

    this.toastrService.error('شما اجازه دسترسی به این صفحه را ندارید!');
    //// not logged in so redirect to login page with the return url
    this.router.navigate(['/503'], { queryParams: { returnUrl: state.url } });
    return false;
  }

  chechGuardChild(state: RouterStateSnapshot, route: ActivatedRouteSnapshot) {
    var user = this.authService.getAuthUser();
    if (!user) return false;
    if (user.isCompany && user.userCompanyStatus != 1) {
      var accessUrls = [
        'dashboard',
        'company-profile',
        'tickets',
        'ticket-details',
        'contact-us',
        'change-password',
      ];
      if (accessUrls.indexOf(route.url[0].path) > -1) return true;
    } else if (user.isCompany && user.userCompanyStatus == 1) {
      return true;
    }

    this.toastrService.error('شما اجازه دسترسی به این صفحه را ندارید!');
    //// not logged in so redirect to login page with the return url
    this.router.navigate(['/503'], { queryParams: { returnUrl: state.url } });

    return false;
  }
}
