import { Component, Inject, Input, OnInit } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogRef,
} from '@angular/material/dialog';

import { ActivatedRoute } from '@angular/router';
import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-view-citizen-profile',
  templateUrl: './view-citizen-profile.component.html',
  styleUrls: ['./view-citizen-profile.component.scss'],
})
export class ViewCitizenProfileComponent implements OnInit {
  @Input('userCode') userCode: string = '';
 
  feedbackfrm: FormGroup;
  citizen: any = {};
  manzalatdata: [];
  loading: boolean;
  loadingData: boolean;
  maritalStatus: boolean;

  baseUrl: string = ServerApis.baseUrl;

  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private fb: FormBuilder,
  ) {


  



  }

  ngOnInit(): void {
    this.getCitizenInfo();
    

    

   
  }
  getCitizenInfo() {
    this.loadingData = true;
    this.dataService.get(ServerApis.getCitizenFullInfo, { userCode: this.userCode }).subscribe(
      (response) => {
        this.loadingData = false;
        if (response.isSuccess) {
          this.citizen = response.data ? response.data : {};
        } else {
          let msg = response.messages
            ? response.messages
            : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.loadingData = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      }
    );
     

  }
  




}
