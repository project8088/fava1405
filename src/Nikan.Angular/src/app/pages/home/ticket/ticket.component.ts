import { Component, OnInit, Inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CustomFormValidators } from 'src/app/core/custom-validator/form-validation';
import Swal from 'sweetalert2';
import { AuthUser } from '../../../core/authentication/user.model';
import { DataService } from '../../../core/services/data-service.service';
import { AuthService } from '../../../core/authentication/auth.service';
import { ServerApis } from '../../../core/server-apis';
 
@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.scss']
})
export class TicketComponent implements OnInit {
  ticketForm: FormGroup;
  isSaving: boolean;
  loadingData: boolean = true;
  periorityList: any[] = [];
  
  organizationList: any = [] = [];
  unitList: any = [] = [];
  loadingUnit: boolean;
  user: AuthUser;
  constructor(
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private dataService: DataService,
    private customValidator: CustomFormValidators,
    private authService: AuthService
  ) {


    this.ticketForm = this.fb.group({
      subject: [null, [Validators.required]],
      ticketMessage: [null, [Validators.required, Validators.maxLength(10000)]],
      priority: [null, [Validators.required]], 
      name: [null, [Validators.required]],
      mobileNumber: [null, [this.customValidator.checkMobileNumber]],
      nationCode: [''],
      organizationId: [null, [Validators.required]],
      organizationalUnitId: [null, [Validators.required]],
      
    });

  }


  ngOnInit(): void {
    
    this.user = this.authService.getAuthUser();
    if (this.user) {
      this.ticketForm.get('name').setValue(this.user.displayName);
    }

   
    this.getOrganizations();

    this.dataService.getEnums().subscribe(response => {
      if (response) {
        this.periorityList = response.ticketPriority ? response.ticketPriority : [];
      } else {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      }
    }, error => {
      this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
    });

  }


  

  getOrganizations() {
    this.loadingData = true;

    this.dataService.get(ServerApis.getAllSupportCenter, {}).subscribe(response => {
      this.loadingData = false;
      if (response.isSuccess) {
        this.organizationList = response.data ? response.data : [];
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
      this.loadingData = false;
    });
  }


  getUnitsOfOrganization() {
    this.loadingUnit = true;

    this.dataService.get(ServerApis.getAllOrganizationalUnitByOrganId, {
      organId: this.ticketForm.get('organizationId').value
    }).subscribe(response => {
      this.loadingUnit = false;
      if (response.isSuccess) {
        this.unitList = response.data ? response.data : [];
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
      this.loadingUnit = false;
    });
  }




  saveInfo() {
    if (this.ticketForm.invalid) {
      this.toastrService.warning("اطلاعات فرم را تکمیل کنید.");
      this.ticketForm.markAllAsTouched();
      return false;
    }
    this.isSaving = true;
    let formData = this.ticketForm.value;
    this.dataService.post(ServerApis.sendUserTicket, {
      Id: '',
      Subject: formData.subject,
      TicketMessage: formData.ticketMessage,
      organizationalUnitId: formData.organizationalUnitId,
      name: formData.name ? formData.name : '',
      mobileNumber: formData.mobileNumber ? formData.mobileNumber : '',
      nationCode: formData.nationCode ? formData.nationCode : '', 
      UserId: this.user ? this.user.userId : ''

    }).subscribe(response => {
      this.isSaving = false;
      if (response.isSuccess) {
        this.toastrService.success("پیام شما با موفقیت ارسال شد.");
        Swal.fire({
          showConfirmButton: true,
          showCancelButton: false,
          title: 'پیام شما با موفقیت ارسال شد',
          text: 'کد رهگیری: ' + response.data.code,
          confirmButtonText: 'تائید'
        });
        this.ticketForm.reset();
        

      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
      this.isSaving = false;
      this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
    });
  }




  
}
