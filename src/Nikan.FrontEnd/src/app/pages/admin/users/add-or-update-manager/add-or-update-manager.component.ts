import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CustomFormValidators } from '../../../../core/custom-validator/form-validation';
import { ServerApis } from '../../../../core/server-apis';
import { forkJoin } from 'rxjs';
import * as CkEditor from '../../../../../assets/ckeditor';
import { AppBase } from "@app/app.base";

@Component({
  selector: 'adm-add-or-update-manager',
  templateUrl: './add-or-update-manager.component.html',
  styleUrls: ['./add-or-update-manager.component.scss'],
    standalone: false
})
export class AdminAddOrUpdateManagerComponent extends AppBase implements OnInit, AfterViewInit {
  isUpdate: boolean;
  id: string;
  isSaving: boolean;
  userForm: FormGroup;
  loading: boolean = true;
  imageUrl: string = '';
  baseUrl: string = ServerApis.baseUrl;
  namePrefixList: any = ([] = []);
  provinceList: any = ([] = []);
  organizationalPositionList: any = ([] = []);
  loadingData: boolean = true;
  htmlEditor: any;
  constructor(
    private customValidator: CustomFormValidators
  ) {
      super();
    this.route.params.subscribe((p) => {
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
      userCompanyId: [null],
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

  ngAfterViewInit() {
    setTimeout(() => {
      if (!this.isUpdate) this.loadCkEditor('');
      else this.loadCkEditor(this.userForm.get('biography').value);
    }, 500);
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
          this.router.navigate(['/admin/manager-users']);
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
      Id: formValue.id,
      PersonelCode: formValue.personelCode,
      UserCompanyId: formValue.userCompanyId,
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
      ThumbnailUrl: '',
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

    this.dataService.post(ServerApis.addOrUpdatePersonelByAdmin, params).subscribe(
      (response) => {
        this.isSaving = false;
        if (response && response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
          this.router.navigate(['/admin/manager-users']);
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
