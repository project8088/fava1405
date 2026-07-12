import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { ChangeCardAddressDialogComponent } from '../../_dialogs/change-card-address/change-card-address.component';
import { AppBase } from '@app/app.base';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-bought-cards',
  templateUrl: './bought-cards.component.html',
  styleUrls: ['./bought-cards.component.scss'],
  standalone: false,
})
export class BoughtCardsComponent extends AppBase implements OnInit {
  loading: boolean = true;
  cards: any[] = [];

  displayedColumns: string[] = [
    'cardTitle',
    'requestDate',
    'requestStatuse',
    'cardNumber',
    'deliveredOnDate',
    'deliverType',
  ];

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.getList();
  }

  getList() {
    this.dataService
      .get(ServerApis.getCitizenCardInfo)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.chdr.detectChanges();
        }),
      )
      .subscribe((response) => {
        this.cards = response.data;
      });
  }

  openCardAddress(info: any) {
    this.matDialog
      .open(ChangeCardAddressDialogComponent, {
        panelClass: 'custom-dialog',
        minWidth: '600px',
        data: {
          info: info,
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          this.getList();
        }
      });
  }
}
