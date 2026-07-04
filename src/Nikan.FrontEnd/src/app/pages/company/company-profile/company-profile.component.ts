import { Component, OnInit } from '@angular/core';
import { AuthUser } from '../../../core/authentication/user.model';
import { AuthService } from '../../../core/authentication/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-company-profile',
  templateUrl: './company-profile.component.html',
  styleUrls: ['./company-profile.component.scss'],
})
export class CompanyProfileComponent implements OnInit {
  user: AuthUser;
  constructor(
    private authService: AuthService,
    private router: Router,
  ) {
    this.user = this.authService.currentUserValue;
  }

  ngOnInit(): void {}

  back() {
    this.router.navigate(['/admin/companies']);
  }
}
