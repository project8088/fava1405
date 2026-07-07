import { OnInit, Component } from '@angular/core';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-ticket-details',
  templateUrl: './ticket-details.component.html',
  styleUrls: ['./ticket-details.component.scss'],
  standalone: false,
})
export class TicketDetailsComponent extends AppBase implements OnInit {
  isAdmin: boolean;
  constructor() {
    super();
    this.isAdmin = this.authService.getAuthUser()?.isAdmin == true;
  }
  ngOnInit() {}
}
