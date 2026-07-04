import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { map, shareReplay } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { RegisterServiceModel } from '@core/models/models';
import { ServerApis } from '@core/server-apis';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-citizen-card',
  templateUrl: './citizen-card.component.html',
  styleUrls: ['./citizen-card.component.scss'],
})
export class CitizenCardComponent extends AppBase implements OnInit {
  constructor(
    private breakpointObserver: BreakpointObserver
  ) {
      super();}

  cards;
  availableCards;

  loading: boolean = false;
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map((result) => result.matches),
    shareReplay(),
  );

  ngOnInit(): void {
    this.getCitizenCards();
  }

  getCitizenCards() {
    this.dataService.get(ServerApis.getCitizenCardInfo).subscribe(
      (response) => {
        this.loading = false;
        const cards: RegisterServiceModel[] = response.data ? response.data : [];
        this.cards = cards;
      },
      (error) => {
        this.loading = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  checkCanOrderCard(card) {
    this.dataService.get(ServerApis.checkCanOrderCard, { cardInfoId: card.cardInfoId }).subscribe(
      (response) => {
        this.loading = false;
        const cards: RegisterServiceModel[] = response.data ? response.data : [];
        // if (response.isSuccess)
        //   this.router.navigateByUrl('./card-details');
        // else this.toastrService.error(response.message);
      },
      (error) => {
        this.loading = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }
}
