import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { map, shareReplay } from 'rxjs/operators';

import { BuyCardDialogComponent } from '../../_dialogs/buy-card/buy-card.component';
import { DataService } from 'src/app/core/services/data-service.service';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { RegisterServiceModel } from 'src/app/core/models/models';
import { Router } from '@angular/router';
import { ServerApis } from 'src/app/core/server-apis';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-citizen-card-list',
  templateUrl: './card-list.component.html',
  styleUrls: ['./card-list.component.scss'],
})
export class CitizenCardListComponent implements OnInit {
  constructor(
    private breakpointObserver: BreakpointObserver,
    private toastrService: ToastrService,
    private dataService: DataService,
    private router: Router,
    private matDialog: MatDialog
  ) {}

  cards;
  availableCards;

  loading: boolean = false;
  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(
      map((result) => result.matches),
      shareReplay()
    );

  ngOnInit(): void {
    this.getCitizenCards();
    this.listAvailableCards();
  }

  getCitizenCards() {
    this.dataService.get(ServerApis.getCitizenCardInfo).subscribe(
      (response) => {
        this.loading = false;
        const cards: RegisterServiceModel[] = response.data
          ? response.data
          : [];
        this.cards = cards;
      },
      (error) => {
        this.loading = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      }
    );
  }

  listAvailableCards() {
    this.dataService.get(ServerApis.listAvailableCards).subscribe(
      (response) => {
        this.loading = false;

        const cards: RegisterServiceModel[] = response.data
          ? response.data
          : [];
        this.availableCards = cards;
      },
      (error) => {
        this.loading = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      }
    );
  }

  checkCanOrderCard(card) {
    this.dataService
      .get(ServerApis.checkCanOrderCard, { cardInfoId: card.cardInfoId })
      .subscribe(
        (response) => {
          this.loading = false;
          const cards: RegisterServiceModel[] = response.data
            ? response.data
            : [];
          if (response.isSuccess) {
            debugger;
            this.router.navigate(['/citizen/citizen-card/card-details'], {
              queryParams: { id: card.cardInfoId },
            });
          } 
          else this.toastrService.error(response.messages);
        },
        (error) => {
          this.loading = false;
          this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
        }
      );
  }

  buyCard(card) {
    this.matDialog  
      .open(BuyCardDialogComponent, {
        data: { card },
        panelClass: 'custom-dialog',
        width: '1000px'
      })
      .afterClosed()
      .subscribe((result) => {});
  }
}
