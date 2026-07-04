import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { ActivatedRoute } from '@angular/router';

import { DataService } from '../../../../core/services/data-service.service';
import { MatDialog } from '@angular/material/dialog';
import { ServerApis } from '../../../../core/server-apis';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'adm-citizen-Identity-Info',
  templateUrl: './citizen-Identity-Info.component.html',
  styleUrls: ['./citizen-Identity-Info.component.scss'],
})
export class AdminCitizenIdentityInfoComponent implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  userId: string;
  info: any;
  userStatus: number;
  userCode: string = '';
  constructor(
    private route: ActivatedRoute,
    private toastrService: ToastrService,
    private dataService: DataService,
  ) {
    this.route.params.subscribe((p) => {
      if (p.id != '0' && p.id) this.userCode = p.id;
      this.getPersonalInfo();
    });
  }

  ngOnInit() {
    this.getPersonalInfo();
  }

  getPersonalInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getIdentityInformationByAdmin, { userCode: this.userCode })
      .subscribe(
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
