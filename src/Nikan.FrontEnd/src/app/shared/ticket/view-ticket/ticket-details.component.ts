import { OnInit, Component } from '@angular/core';
import { AuthService } from '../../../core/authentication/auth.service';

@Component({
  selector: 'app-ticket-details',
  templateUrl: './ticket-details.component.html',
  styleUrls: ['./ticket-details.component.scss'],
})
export class TicketDetailsComponent implements OnInit {
  isAdmin: boolean;
  constructor(private authService: AuthService) {
    this.isAdmin = this.authService.getAuthUser().isAdmin;
  }
  ngOnInit() {}
}
