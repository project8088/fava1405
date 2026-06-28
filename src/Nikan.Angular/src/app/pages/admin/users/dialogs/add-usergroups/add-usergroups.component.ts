import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '../../../../../core/custom-validator/form-validation';
import { DataService } from '../../../../../core/services/data-service.service';
import { ServerApis } from '../../../../../core/server-apis';
import { userGroupsDto } from '../../../../../core/models/users/usergroups';


@Component({
  selector: 'app-adm-add-usergroups-dialog',
  templateUrl: './add-usergroups.component.html',
  styleUrls: ['./add-usergroups.component.scss']
})
export class AdminAddUserGrousDialogComponent implements OnInit {
  isSaving: boolean;
  isUpdate: boolean;
  userGroupsForm: FormGroup;
  id: string; 
  loading: boolean = true;  
  userGroups: userGroupsDto;



  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<AdminAddUserGrousDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private dataService: DataService,
    private customValidator: CustomFormValidators,
    ) {

    if (_data) { 
      this.userGroups = _data.userGroups ? _data.userGroups : '';
      this.isUpdate = true;
      
    }



    this.userGroupsForm = this.fb.group({
      id: [null],
      name: [null, [Validators.required]],
    });
  }



  ngOnInit() {
    if (this.userGroups) {
      console.log(this.userGroups.name);
      this.userGroupsForm.setValue({ 
        name: this.userGroups.name,
        id: this.userGroups.id,
      });
    }

  }

  

  saveInfo() {
    if (this.userGroupsForm.invalid) {
      this.toastrService.warning("اطلاعات فرم را تکمیل کنید.");
      this.userGroupsForm.markAllAsTouched();
      return false;
    }

    var formValue = this.userGroupsForm.value;

    this.isSaving = true;
    this.dataService.post(ServerApis.addOrUpdateUserGroups, formValue).subscribe(response => {
      this.isSaving = false;
      if (response && response.isSuccess) {
        this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
        this.matDialogRef.close(response.data);
      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
      }
    }, error => {
      this.isSaving = false;
      this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
    });
  }




  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.key === c2.key : c1 === c2;
  }








}
