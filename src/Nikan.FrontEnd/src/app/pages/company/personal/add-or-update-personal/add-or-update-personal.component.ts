import { Component, OnInit, Inject, ViewChild, AfterViewInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { DataService } from '../../../../core/services/data-service.service';
import { ServerApis } from '../../../../core/server-apis';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import * as CkEditor from '../../../../../assets/ckeditor';
import { AuthUser } from '../../../../core/authentication/user.model';
import { AuthService } from '../../../../core/authentication/auth.service';

@Component({
  selector: 'company-add-or-update-personal',
  templateUrl: './add-or-update-personal.component.html',
  styleUrls: ['./add-or-update-personal.component.scss'],
})
export class CompanyAddOrUpdatePersonalComponent implements OnInit, AfterViewInit {
  isUpdate: boolean;
  id: string;
  companyId: string;
  isSaving: boolean;
  userForm: FormGroup;
  loading: boolean = true;
  imageUrl: string = '';
  namePrefixList: any = ([] = []);
  provinceList: any = ([] = []);
  organizationalPositionList: any = ([] = []);
  loadingData: boolean = true;
  baseUrl: string = ServerApis.baseUrl;
  htmlEditor: any;
  user: AuthUser;
  constructor(
    private matDialog: MatDialog,
    private toastrService: ToastrService,
    private fb: FormBuilder,
    private customValidator: CustomFormValidators,
    private dataService: DataService,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
  ) {
    this.user = this.authService.currentUserValue;

    this.route.params.subscribe((p) => {
      this.companyId = p.companyId;
      if (p.id && p.id != '0') {
        this.isUpdate = true;
        this.id = p.id;
        this.getUserInfo();
      } else {
        this.id = '';
        this.isUpdate = false;
      }
    });

    this.userForm = this.fb.group({
      id: [null],
      personelCode: [null, [Validators.required]],
      organizationalPositionId: [null, [Validators.required]],
      namePrefix: [null, [Validators.required]],
      firstName: [
        null,
        [
          Validators.required,
          Validators.maxLength(50),
          this.customValidator.checkPersianCharacters,
        ],
      ],
      lastName: [
        null,
        [
          Validators.required,
          Validators.maxLength(50),
          this.customValidator.checkPersianCharacters,
        ],
      ],
      fatherName: [
        null,
        [
          Validators.required,
          Validators.maxLength(50),
          this.customValidator.checkPersianCharacters,
        ],
      ],
      nationCode: [null, [Validators.required, this.customValidator.checkNationalCode]],
      mobileNumber: [null, [Validators.required, this.customValidator.checkMobileNumber]],
      cellNumber: [null, [Validators.required, this.customValidator.checkPhoneNumber]],
      email: [null, [Validators.required, this.customValidator.checkEmail]],
      province: [null],
      city: [null],
      imageUrl: [null],
      zipCode: [null, [Validators.maxLength(10), Validators.minLength(10)]],
      street: [null, [Validators.maxLength(100)]],
      fullAddress: [null, [Validators.maxLength(1000)]],
      pelak: [null, [Validators.maxLength(100)]],
      office: [null],
      officePhoneNumber: [null, [this.customValidator.checkPhoneNumber]],
      isManagementMembers: [false],
      biography: [''],
      hasSpecificDisease: [false],
      descriptionDisease: [''],
    });
  }

  ngOnInit() {
    this.getBaseData();
  }

  getBaseData() {
    this.loadingData = true;
    forkJoin(
      this.dataService.get(ServerApis.getProvinces),
      this.dataService.getEnums(),
      this.dataService.get(ServerApis.getPositionList),
    ).subscribe(([provinces, enums, positions]) => {
      this.loadingData = false;
      this.provinceList = provinces.data ? provinces.data : [];
      this.namePrefixList = enums.namePrefix ? enums.namePrefix : [];
      this.organizationalPositionList = positions.data ? positions.data : [];
    });
  }

  ngAfterViewInit() {
    setTimeout(() => {
      if (!this.isUpdate) this.loadCkEditor('');
      else this.loadCkEditor(this.userForm.get('biography').value);
    }, 500);
  }

  getUserInfo() {
    this.loading = true;
    this.dataService.get(ServerApis.getPersonelInfo, { id: this.id }).subscribe(
      (response) => {
        this.loading = false;
        if (response && response.isSuccess) {
          this.userForm.patchValue(response.data);
          this.imageUrl = response.data.imageUrl;
          setTimeout(() => {
            if (response.data.biography) this.loadCkEditor(this.userForm.get('biography').value);
          }, 500);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
          if (this.user.isAdmin)
            this.router.navigate(['/admin/company-personal/' + this.companyId]);
          else this.router.navigate(['/company/personal/0']);
        }
      },
      (error) => {
        this.loading = false;
      },
    );
  }

  getAttachmentId(ev) {
    this.imageUrl = ev.uploadUrl;
  }

  saveInfo() {
    if (this.userForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.userForm.markAllAsTouched();
      return false;
    }
    var formValue = this.userForm.value;

    this.isSaving = true;

    formValue.cityId = formValue.city.key;
    if (this.htmlEditor.getData()) formValue.biography = this.htmlEditor.getData();
    var params = {
      Id: formValue.id ? formValue.id : '',
      PersonelCode: formValue.personelCode,
      UserCompanyId: this.companyId && this.companyId != '0' ? this.companyId : null,
      OrganizationalPositionId: +formValue.organizationalPositionId,
      NamePrefix: formValue.namePrefix,
      FirstName: formValue.firstName,
      LastName: formValue.lastName,
      FatherName: formValue.fatherName,
      NationCode: formValue.nationCode,
      MobileNumber: formValue.mobileNumber,
      CellNumber: formValue.cellNumber,
      Email: formValue.email,
      CityId: +formValue.city.key,
      ImageUrl: this.imageUrl,
      ZipCode: formValue.zipCode,
      Street: formValue.street,
      FullAddress: formValue.fullAddress,
      Pelak: formValue.pelak,
      Office: formValue.office,
      OfficePhoneNumber: formValue.officePhoneNumber,
      EmployeementOnDate: null,
      IsManagementMembers: formValue.isManagementMembers ? formValue.isManagementMembers : false,
      Biography: formValue.biography ? formValue.biography : '',
      HasSpecificDisease: formValue.hasSpecificDisease ? formValue.hasSpecificDisease : false,
      DescriptionDisease: formValue.descriptionDisease ? formValue.descriptionDisease : '',
    };

    this.dataService.post(ServerApis.addOrUpdatePersonel, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response && response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
          if (this.user.isAdmin)
            this.router.navigate(['/admin/company-personal/' + this.companyId]);
          else this.router.navigate(['/company/personal/0']);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.isSaving = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
      },
    );
  }

  changeHasSpecificDisease() {
    if (this.userForm.get('hasSpecificDisease').value == true) {
      this.userForm.get('descriptionDisease').setValidators([Validators.required]);
      this.userForm.get('descriptionDisease').updateValueAndValidity();
    } else {
      this.userForm.get('descriptionDisease').clearValidators();
      this.userForm.get('descriptionDisease').updateValueAndValidity();
      this.userForm.get('descriptionDisease').setValue('');
    }
  }

  /**
   * لود کردن html editor
   * */
  loadCkEditor(content) {
    if (!this.htmlEditor && document.querySelector('.html-editor')) {
      document.querySelector('.html-editor').innerHTML = '';
      CkEditor.create(document.querySelector('.html-editor'), {
        removePlugins: ['Title'],
        toolbar: {
          items: [
            'heading',
            '|',
            'bold',
            'italic',
            'underline',
            'link',
            'bulletedList',
            'numberedList',
            '|',
            'indent',
            'alignment',
            'outdent',
            'pageBreak',
            '|',
            'fontBackgroundColor',
            'fontColor',
            'fontFamily',
            'fontSize',
            'highlight',
            'removeFormat',
            '|',
            'imageUpload',
            'blockQuote',
            'insertTable',
            'mediaEmbed',
            'code',
            'codeBlock',
            'exportPdf',
            'horizontalLine',
            'specialCharacters',
            'todoList',
            '|',
            'undo',
            'redo',
          ],
        },
        language: 'fa',
        image: {
          // Configure the available styles.
          styles: ['alignLeft', 'alignCenter', 'alignRight', 'full', 'side'],
          // You need to configure the image toolbar, too, so it shows the new style
          // buttons as well as the resize buttons.
          toolbar: [
            'imageStyle:alignLeft',
            'imageStyle:alignCenter',
            'imageStyle:alignRight',
            '|',
            'imageTextAlternative',
            'imageStyle:full',
            'imageStyle:side',
          ],
        },
        table: {
          contentToolbar: [
            'tableColumn',
            'tableRow',
            'mergeTableCells',
            'tableCellProperties',
            'tableProperties',
          ],
        },
        licenseKey: '',
        title: {
          placeholder: 'عنوان را در این قسمت تایپ کنید',
        },
        placeholder: 'محتوای خود را در این قسمت بنویسید و یا Paste کنید.',
      })
        .then((editor) => {
          //window.editor = editor;
          this.htmlEditor = editor;
          if (content) {
            this.htmlEditor.setData(content);
          }
          //this.htmlEditor.model.document.on('change', () => {
          //});
          //on blure
          //editor.ui.focusTracker.on('change:isFocused', (evt, name, isFocused) => {
          // // if (!isFocused)

          //});
        })
        .catch((error) => {
          //console.warn('Build id: nwwk5h15tym5-uff91zgwvva9');
          console.error(error);
        });
    }
  }
}
