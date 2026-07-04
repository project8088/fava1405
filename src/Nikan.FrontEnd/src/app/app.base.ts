import { inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '@core/services/data-service.service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '@core/authentication/auth.service';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

export class AppBase {
  protected dataService = inject(DataService);
  protected route = inject(ActivatedRoute);
  protected router = inject(Router);
  protected toastrService = inject(ToastrService);

  protected fb = inject(FormBuilder);
  protected authService = inject(AuthService);
  protected matDialog = inject(MatDialog);
}
