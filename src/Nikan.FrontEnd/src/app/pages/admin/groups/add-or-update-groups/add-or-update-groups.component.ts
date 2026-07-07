import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-add-or-update-groups',
  templateUrl: './add-or-update-groups.component.html',
  styleUrls: ['./add-or-update-groups.component.scss'],
  standalone: false,
})
export class AdminAddOrUpdateGroupsComponent extends AppBase implements OnInit, AfterViewInit {
  isUpdate=false;
  id: string ='';
  groupForm: FormGroup;
  baseUrl = ServerApis.baseUrl;

  isSaving=false;
  imageUrl: string = '';
    loading?: boolean;
  parentGroups: any[] = [];

  constructor() {
    super();
    this.route.params.subscribe((p) => {
      if (p['id'] && p['id'] != '0') {
        this.isUpdate = true;
        this.id = p['id'];
        this.getStoreInfo();
      } else {
        this.id = '';
        this.isUpdate = false;
      }
    });
    this.groupForm = this.fb.group({
      id: [null],
      parentId: [null],
      groupName: [null, [Validators.required, Validators.maxLength(500)]],
      expireDate: [null],
      code: [null],
      maxMembers: [null],
      law_AgeFrom: [null],
      law_AgeTo: [null],
      isActive: [true, []],
      showToMembers: [false, []],
      autoAddMembers: [false, []],
      specialRules: [false, []],
      law_Gender: [false, []],
      showToAddCitizen: [false, []],
      canBuyFreeCard: [false, []],
      municipalPersonnelGroup: [false, []],
    });
  }

  ngOnInit(): void {
    this.getGroups();
  }

  ngAfterViewInit() {}

  getStoreInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.groupInfo, {
        id: this.id,
        forEdit: true,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.groupForm.patchValue(response.data);
          } else {
            var msg = response.messages ? response.messages : 'خطایی در سرور رخ داده است.';
            this.toastrService.error(msg);
          }
        },
        (error) => {
          this.loading = false;
        },
      );
  }

  getGroups() {
    this.dataService.get(ServerApis.getGroups).subscribe(
      (response) => {
        this.parentGroups = response.data;
      },
      (error) => {
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  /**
   * for bind object in select
   * @param item
   */
  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? +c1.key === c2.key : c1 === c2;
  }

  getAttachmentId(ev:{uploadUrl:string}) {
    this.imageUrl = ev.uploadUrl;
  }

  save() {
    if (this.groupForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.groupForm.markAllAsTouched();
        return ;
    }

    let form = this.groupForm.value;
    let params = form;

    this.isSaving = true;
    this.dataService.post(ServerApis.addOrUpdateGroup, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.router.navigate(['/admin/group-list']);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.isSaving = false;
      },
    );
  }
}
