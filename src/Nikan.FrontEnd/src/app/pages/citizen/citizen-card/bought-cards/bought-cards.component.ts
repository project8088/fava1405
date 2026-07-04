import { Component, OnInit } from '@angular/core';

import { DataService } from 'src/app/core/services/data-service.service';
import { RegisterServiceModel } from 'src/app/core/models/models';
import { ServerApis } from 'src/app/core/server-apis';
import { ToastrService } from 'ngx-toastr';
import { ChangeCardAddressDialogComponent } from '../../_dialogs/change-card-address/change-card-address.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-bought-cards',
  templateUrl: './bought-cards.component.html',
  styleUrls: ['./bought-cards.component.scss'],
})
export class BoughtCardsComponent implements OnInit {
  loading: boolean = true;
  cards;

  displayedColumns: string[] = [
    'cardTitle',
    'requestDate',
    'requestStatuse',
    'cardNumber',
    'deliveredOnDate',
    'deliverType',
  ];

  constructor(
    private toastrService: ToastrService,
    private matDialog: MatDialog,
    private dataService: DataService
  ) {}

  ngOnInit(): void {
    this.getList();
  }


  getList() {
    this.dataService.get(ServerApis.getCitizenCardInfo).subscribe(
      (response) => {
        this.loading = false;
        this.cards = response.data;
      },
      (error) => {
        this.loading = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      }
    );
  }





  openCardAddress(info) {
    this.matDialog.open(ChangeCardAddressDialogComponent, {
      panelClass: 'custom-dialog',
      minWidth: '600px',
      data: {
        info: info
      }
    }).afterClosed().subscribe(result => {
      if (result) {
        this.getList();
      }
    });
  }








}
