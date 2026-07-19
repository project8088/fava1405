import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, inject, OnInit } from '@angular/core';
import { finalize, map, shareReplay } from 'rxjs/operators';

import { BuyCardDialogComponent } from '../../_dialogs/buy-card/buy-card.component';
import { Observable } from 'rxjs';
import { RegisterServiceModel } from '@core/models/models';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-citizen-card-list',
  templateUrl: './card-list.component.html',
  styleUrls: ['./card-list.component.scss'],
  standalone: false,
})
export class CitizenCardListComponent extends AppBase implements OnInit {
  protected breakpointObserver = inject(BreakpointObserver);
  cards: RegisterServiceModel[] = [];
  availableCards: RegisterServiceModel[] = [];

  loading: boolean = false;
  constructor() {
    super();
  }

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map((result) => result.matches),
    shareReplay(),
  );

  ngOnInit(): void {
    this.getCitizenCards();
    this.listAvailableCards();
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

  listAvailableCards() {
    this.dataService
      .get(ServerApis.listAvailableCards)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        const cards: RegisterServiceModel[] = response.data ? response.data : [];
        this.availableCards = cards;
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
        if (response.isSuccess) {
          
          this.router.navigate(['/citizen/citizen-card/card-details'], {
            queryParams: { id: card.cardInfoId },
          });
        } else this.toastrService.error(response.messages);
      });
  }

  buyCard(card: any) {
    this.matDialog
      .open(BuyCardDialogComponent, {
        data: { card },
        panelClass: 'custom-dialog',
        width: '80%',
      })
      .afterClosed()
      .subscribe((result) => {});
  }
}
