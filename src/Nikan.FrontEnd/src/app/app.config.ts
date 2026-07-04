import {
  ApplicationConfig,
  importProvidersFrom,
  provideBrowserGlobalErrorListeners,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { ToastrModule } from 'ngx-toastr';
import { provideHttpClient } from '@angular/common/http';
import { MaterialModule } from '@core/material/material.module';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(),
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
  ],
};
