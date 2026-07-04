import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { map, shareReplay } from 'rxjs/operators';

import { DataService } from 'src/app/core/services/data-service.service';
import { Observable } from 'rxjs';
import { RegisterServiceModel } from 'src/app/core/models/models';
import { Router } from '@angular/router';
import { ServerApis } from 'src/app/core/server-apis';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-citizen-card',
  templateUrl: './citizen-card.component.html',
  styleUrls: ['./citizen-card.component.scss'],
})
export class CitizenCardComponent implements OnInit {
  constructor(
    private breakpointObserver: BreakpointObserver,
    private toastrService: ToastrService,
    private dataService: DataService,
    private router: Router,
  ) {}

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
