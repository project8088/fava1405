import { Component, DOCUMENT, inject } from '@angular/core';
import {
  NavigationCancel,
  NavigationEnd,
  NavigationError,
  NavigationStart,
  Router,
  RouterOutlet,
} from '@angular/router';
import { MainSpinnerService } from '@core/services/main-spinner.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected router = inject(Router);
  protected mainSpinnerService = inject(MainSpinnerService);
  protected doc = inject(DOCUMENT);
  constructor() {
    this.router.events.subscribe((event): void => {
      if (event instanceof NavigationStart) {
        this.mainSpinnerService.show();
      }
      if (event instanceof NavigationCancel || event instanceof NavigationError) {
        this.mainSpinnerService.hide();
      }
      if (event instanceof NavigationEnd) {
        window.scrollTo(0, 0);
        this.mainSpinnerService.hide();
        this.doc
          .querySelector('meta[property=og\\:url]')
          ?.setAttribute('content', window.location.href);
      }
    });
  }
}
