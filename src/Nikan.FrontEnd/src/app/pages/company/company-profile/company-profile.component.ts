import { Component, OnInit } from '@angular/core';
import { AuthUser } from '../../../core/authentication/user.model';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'app-company-profile',
  templateUrl: './company-profile.component.html',
  styleUrls: ['./company-profile.component.scss'],
    standalone: false
})
export class CompanyProfileComponent extends AppBase implements OnInit {
  user: AuthUser;
  constructor(
) {
      super();
    this.user = this.authService.currentUserValue;
  }

  ngOnInit(): void {}

  back() {
    this.router.navigate(['/admin/companies']);
  }
}
