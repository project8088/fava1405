import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { ActivatedRoute } from '@angular/router';
import { CitizenFamilyDialogComponent } from '../_dialogs/family-dialog/family-dialog.component';
import { CitizenProfileComponent } from '../profile.component';
import { DataService } from '../../../../core/services/data-service.service';
import { MatDialog } from '@angular/material/dialog';
import { ServerApis } from '../../../../core/server-apis';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { citizenFamilyModel } from '@core/models/citizen/family.model';
import { karjoEducationDto } from '@core/models/citizen/education';

@Component({
  selector: 'app-my-Identity-Info',
  templateUrl: './my-Identity-Info.component.html',
  styleUrls: ['./my-Identity-Info.component.scss'],
})
export class CitizenMyIdentityInfoComponent implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  userId: string;
  info: any;
  userStatus: number;

  constructor(
    private route: ActivatedRoute,
    private toastrService: ToastrService,
    private dataService: DataService,
  ) {}

  ngOnInit() {
    this.getPersonalInfo();
  }

  getPersonalInfo() {
    this.loading = true;
    this.dataService.get(ServerApis.getIdentityInformationByCitizen).subscribe(
      (response) => {
        this.loading = false;
        if (response && response.isSuccess) {
          this.info = response.data;
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loading = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }
}
