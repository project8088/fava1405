import {
  ApplicationConfig,
  importProvidersFrom,
  provideBrowserGlobalErrorListeners,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { ToastrModule } from 'ngx-toastr';
import { HTTP_INTERCEPTORS, provideHttpClient,  withInterceptorsFromDi } from '@angular/common/http';
import { MaterialModule } from '@core/material/material.module';
import { AuthInterceptor } from '@core/authentication/auth.interceptor';
import { ErrorInterceptor } from '@core/authentication/error.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    importProvidersFrom([
      MaterialModule,
      ToastrModule.forRoot({
        closeButton: true,
        progressBar: true,
        timeOut: 10000,
        positionClass: 'toast-bottom-center',
        preventDuplicates: true,
      }),
    ]),

    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
};
