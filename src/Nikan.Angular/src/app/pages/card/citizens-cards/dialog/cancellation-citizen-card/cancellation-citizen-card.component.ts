import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '../../../../../core/custom-validator/form-validation';
import { DataService } from '../../../../../core/services/data-service.service';
import { ServerApis } from '../../../../../core/server-apis';

@Component({
  selector: 'ard-cancellation-citizen-card-dialog',
  templateUrl: './cancellation-citizen-card.component.html',
  styleUrls: ['./cancellation-citizen-card.component.scss']
})
export class CardCancellationCitizenCardDialogComponent implements OnInit {
  isSaving: boolean;
  frm: FormGroup;
  centerList: any[] = [];
  baseInfo: any = {};
  loading: boolean = true;
  info: any; 
  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<CardCancellationCitizenCardDialogComponent>,
    @Inject(MAT_DIALOG_DATA) _data: any,
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private customValidator: CustomFormValidators,
    private dataService: DataService) {

    this.info = _data.info;

    this.frm = this.fb.group({
      cardCancellationItemId: ['', [Validators.required]],
      description: [null, [Validators.required]],
      cardCancellationOnDate: [null, [Validators.required]],
      sendSms: [false],

    });
    this.frm.patchValue(_data.info);
  }

 

  ngOnInit() {
    this.dataService.getEnums().subscribe(response => {
      this.baseInfo = {
        cardCancellationItem: response.cardCancellationItem,
      };
    });
  }
  


  saveInfo() { 
    if (this.frm.invalid) {
      this.toastrService.warning("اطلاعات فرم را تکمیل کنید.");
      this.frm.markAllAsTouched();
      return false;
    } 
    
    this.isSaving = true;
    var url = ServerApis.cardCancellation;
    var params = this.frm.value;
    params.id = this.info.id;
    params.cardCancellationItemId = params.cardCancellationItemId;
    params.description = params.description;
    params.cardCancellationOnDate = params.cardCancellationOnDate;
    params.sendSms = params.sendSms;
   

    this.dataService.post(url, params).subscribe(response => {
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
      this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
    });
  }



  



}
