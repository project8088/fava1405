import { AfterViewInit, Component, EventEmitter, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { ToastrService } from 'ngx-toastr'; 
import Swal from 'sweetalert2';
import { DataService } from '../../../core/services/data-service.service';
import { ServerApis } from '../../../core/server-apis';
 


@Component({ 
  selector: 'adm-config',
  templateUrl: './config.component.html',
  styleUrls: ['./config.component.scss']
})
export class AdminConfigComponent implements OnInit {
  id: string;


  isSaving: boolean;
  loading: boolean;

  configList: any[] = [];




  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private toastrService: ToastrService,
    private router: Router,
    private dataService: DataService
  ) {


  }

  ngOnInit(): void {
    
  }

  config(): void {
    this.isSaving = true;
    this.dataService.get(ServerApis.configPortal).subscribe(response => {
      if (response.isSuccess) {
        this.isSaving = false;
        this.configList = response.data ? response.data : [];
      } else {
        this.isSaving = false;
        const msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
        this.toastrService.error(msg);
      }

    });
  }





 







}
