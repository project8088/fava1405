import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, inject, OnInit } from '@angular/core';
import { finalize, map, shareReplay } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { RegisterServiceModel } from '@core/models/models';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-card',
  templateUrl: './citizen-card.component.html',
  styleUrls: ['./citizen-card.component.scss'],
  standalone: false,
})
export class CitizenCardComponent extends AppBase implements OnInit {
  cards: RegisterServiceModel[] = [];
  protected breakpointObserver = inject(BreakpointObserver);

  constructor() {
    super();
  }

  loading: boolean = false;
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map((result) => result.matches),
    shareReplay(),
  );

  ngOnInit(): void {
    this.getCitizenCards();
  }

  getCitizenCards() {
    this.dataService
      .get(ServerApis.getCitizenCardInfo)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        const cards: RegisterServiceModel[] = response.data ? response.data : [];
        this.cards = cards;
      });
  }

  checkCanOrderCard(card: any) {
    this.dataService
      .get(ServerApis.checkCanOrderCard, { cardInfoId: card.cardInfoId })
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        const cards: RegisterServiceModel[] = response.data ? response.data : [];
        // if (response.isSuccess)
        //   this.router.navigateByUrl('./card-details');
        // else this.toastrService.error(response.message);
      });
  }
}
