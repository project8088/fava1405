import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServerApis } from '@core/server-apis';
import { FormGroup, Validators } from '@angular/forms';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-group-transfer-dialog',
  templateUrl: './group-transfer.component.html',
  styleUrls: ['./group-transfer.component.scss'],
  standalone: false,
})
export class AdminGroupTransferDialogComponent extends AppBase implements OnInit {
  sourceGroupId: number;
  isSaving=false;
  groupList: any[] = [];
  form: FormGroup;

  constructor(
    private matDialogRef: MatDialogRef<AdminGroupTransferDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) {
    super();
    this.form = this.fb.group({
      destinationGroupId: [null, [Validators.required]],
      sourceGroupId: [null],
      isTransfer: [false],
      isHasQueue: [false],
      isTransferQueue: [false],
    });
    this.sourceGroupId = _data.groupId;
  }

  ngOnInit() {
    this.getGroups();
  }

  getGroups() {
    this.dataService.get(ServerApis.getGroups).subscribe(
      (response) => {
        this.groupList = response.data;
      },
      (error:any) => {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  saveInfo() {
    if (this.form.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.form.markAllAsTouched();
        return ;
    }

    var formValue = this.form.value;
    this.isSaving = true;
    this.dataService
      .post(ServerApis.groupTransfer, {
        sourceGroupId: +this.sourceGroupId,
        destinationGroupId: formValue.destinationGroupId,
        isTransfer: formValue.isTransfer,
        isHasQueue: formValue.isHasQueue,
        isTransferQueue: formValue.isTransferQueue,
      })
      .subscribe(
        (response) => {
          this.isSaving = false;
          if (response.isSuccess) {
            this.toastrService.success(response.messages);
            this.matDialogRef.close(true);
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error:any) => {
          this.isSaving = false;
        },
      );
  }

  /**
   * for bind object in select
   * @param item
   */
  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.id == c2.id : c1 == c2;
  }
}
