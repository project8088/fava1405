import { Component, OnInit, Inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'; 
 import { CustomFormValidators } from '../../../../../core/custom-validator/form-validation';
import { DataService } from '../../../../../core/services/data-service.service';
import { ServerApis } from '../../../../../core/server-apis';

@Component({
  selector: 'company-update-user-dialog',
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.scss']
})
export class CompanyUpdateUserDialogComponent implements OnInit {
  isSaving: boolean;
  userForm: FormGroup;
  userId: string;
  loading: boolean = true;  
  companyId: string;

  userAccountStateList: any[] = []; 
  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<CompanyUpdateUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
    private fb: FormBuilder, 
    private customValidator: CustomFormValidators,
    private dataService: DataService) {
    this.companyId = _data.companyId;
    if (_data.userId) {
      this.userId = _data.userId;
      this.getUserInfo();
    }


    this.userForm = this.fb.group({
      id: [null],
      displayName: [null, [Validators.required, this.customValidator.checkPersianCharacters]],
      userName: [null, [Validators.required, this.customValidator.checkEnglishORNumberCharacters]],
      mobileNumber: [null, [Validators.required, this.customValidator.checkMobileNumber]],
      emailAddress: [null, [Validators.required, this.customValidator.checkEmail]],
      userAccountState: ['', [Validators.required]],
    
    });
  }



  ngOnInit() { 
    this.dataService.getEnums().subscribe(response => {
      this.userAccountStateList = response.userAccountState ? response.userAccountState : [];
    });
  }

  

  getUserInfo() {
    this.loading = true
    //todo
    this.dataService.get(ServerApis.getUserAccountInfo,{ userId: this.userId }).subscribe(response => {
      this.loading = false;
      if (response.isSuccess) {
        this.userForm.patchValue(response.data);
         
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است.";
        this.toastrService.error(msg);
        this.matDialogRef.close(false);
      }

    }, error => {
      this.loading = false; 
      this.matDialogRef.close(false);
    });
  }




  displayFn(item): string {
    return item && item.text ? item.text : '';
  }





  saveInfo() {
    if (this.userForm.invalid) {
      this.toastrService.warning("اطلاعات فرم را تکمیل کنید.");
      this.userForm.markAllAsTouched();
      return false;
    }


    var formValue = this.userForm.value;

    this.isSaving = true;
    this.dataService.post(ServerApis.updateCompanyUserAccount,
      {
        CompanyId: this.companyId && this.companyId != '0' ? +this.companyId : null,
        UserId: this.userId,
        DisplayName: formValue.displayName,
       // UserName: formValue.username,
        MobileNumber: formValue.mobileNumber,
        Email: formValue.emailAddress,
        UserAccountState: formValue.userAccountState,
       
      }
    ).subscribe(response => {
      this.isSaving = false;
      if (response && response.isSuccess) {
        this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
        this.matDialogRef.close(true);
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
      this.isSaving = false; 
    });
  }

   


  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.key === c2.key : c1 === c2;
  }



    



}
