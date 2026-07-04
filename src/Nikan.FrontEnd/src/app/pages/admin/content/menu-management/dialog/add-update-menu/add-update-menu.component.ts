import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, Validators } from '@angular/forms';
import { ServerApis } from '../../../../../../core/server-apis';
import { forkJoin } from 'rxjs';
import { AppBase } from "@app/app.base";

declare var $: any;

@Component({
  selector: 'app-add-update-menu-dialog',
  templateUrl: './add-update-menu.component.html',
  styleUrls: ['./add-update-menu.component.scss'],
    standalone: false
})
export class AdminAddOrUpdateMenuDialogComponent extends AppBase implements OnInit {
  isUpdate: boolean;
  isSaving: boolean;

  loadingData: boolean;

  menuForm: FormGroup;
  parentMenus: any[] = [];
  innerMenuItems: any[] = [
    { menuName: 'صفحه اصلی', menuPath: '/home' },
    { menuName: 'تماس با ما', menuPath: '/home/contact-us' },
    { menuName: 'اخبار', menuPath: '/home/news-list' },
    { menuName: 'شرکت ها', menuPath: '/home/company-list' },
    { menuName: 'پرسش و پاسخ های متداول', menuPath: '/home/faq' },
    { menuName: 'ارسال سوال', menuPath: '/home/ticket' },
    { menuName: 'پیگیری سوال', menuPath: '/home/ticket-answer' },
    { menuName: 'ثبت نام شرکت', menuPath: '/account/register-company' },
    { menuName: 'محصولات', menuPath: '/home/products' },
  ];
  constructor(
    private matDialogRef: MatDialogRef<AdminAddOrUpdateMenuDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any
  ) {
      super();
    this.menuForm = this.fb.group({
      id: [null],
      menuName: [null, [Validators.required, Validators.maxLength(50)]],
      menuPath: ['', []],
      tabOrder: [1, []],
      iconFile: ['', []],
      parentId: [0, []],
      isVisible: [true, []],
      isSystem: [true, [Validators.required]],
      openInNewPage: [false, []],
      disableLink: [false, []],
      innerMenu: [null],
    });

    if (_data) {
      this.isUpdate = true;
      _data.parentId = _data.parentId ? _data.parentId : 0;
      console.log(_data);
      this.menuForm.patchValue(_data);
    } else {
      this.isUpdate = false;
    }
  }

  ngOnInit(): void {
    this.loadingData = true;
    this.parentMenus = [];

    forkJoin(
      this.dataService.get(ServerApis.getAllMenuItems, {}),
      this.dataService.get(ServerApis.getAllPagePath),
    ).subscribe(
      ([menus, pages]) => {
        this.loadingData = false;
        if (menus.isSuccess) {
          this.parentMenus = menus.data ? menus.data : [];
        } else {
          let msg = menus.messages ? menus.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }

        //pages
        if (pages.isSuccess) {
          var webPages = pages.data ? pages.data : [];
          webPages.forEach((item) => {
            this.innerMenuItems.push({
              menuName: item.text,
              menuPath: '/home/page/' + item.description,
            });
          });
        } else {
          let msg = pages.messages ? pages.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.matDialogRef.close(false);
      },
    );
  }

  setInnerSite() {
    if (this.menuForm.get('innerMenu').value) {
      this.menuForm.get('menuName').setValue(this.menuForm.get('innerMenu').value.menuName);
      this.menuForm.get('menuPath').setValue(this.menuForm.get('innerMenu').value.menuPath);
    } else {
      this.menuForm.get('menuName').setValue(null);
      this.menuForm.get('menuPath').setValue(null);
    }
  }

  changeMenuType() {
    if (this.menuForm.get('isSystem').value == true) {
      this.menuForm.get('innerMenu').setValidators([Validators.required]);
      this.menuForm.get('innerMenu').updateValueAndValidity();
    } else {
      this.menuForm.get('innerMenu').setValue(null);
      this.menuForm.get('innerMenu').clearValidators();
      this.menuForm.get('innerMenu').updateValueAndValidity();
    }
  }

  saveInfo() {
    if (this.menuForm.invalid) {
      this.toastrService.error('اطلاعات فرم را تکمیل کنید.');
      this.menuForm.markAllAsTouched();
      return false;
    }
    this.isSaving = true;
    this.dataService.post(ServerApis.addOrUpdateMenuItem, this.menuForm.value).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('ذخیره اطلاعات با موفقیت انجام شد.');
          this.matDialogRef.close(this.menuForm.value);
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
