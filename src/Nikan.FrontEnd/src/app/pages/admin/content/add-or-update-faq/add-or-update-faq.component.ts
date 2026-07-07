import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-add-or-update-faq',
  templateUrl: './add-or-update-faq.component.html',
  styleUrls: ['./add-or-update-faq.component.scss'],
  standalone: false,
})
export class AdminAddOrUpdateFaqComponent extends AppBase implements OnInit, AfterViewInit {
  isUpdate=false;
  faqId: string;
  faqForm: FormGroup;

  isSaving=false;
  attachmentId: string = '';
    loading?: boolean;

  groupList: any[] = [];

  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

  tagNames: any[] = [];

  constructor() {
    super();
    this.faqForm = this.fb.group({
      id: [null],
      title: [null, [Validators.required, Validators.maxLength(500)]],
      description: [null],
      questionGroupTypeId: [null, [Validators.required]],
      tagNames: [null, []],
      isActive: [true, []],
    });
  }

  ngAfterViewInit() {
  }
  ngOnInit(): void {
    this.getGroups();
    this.route.params.subscribe((p) => {
      if (p['id'] && p['id'] != '0') {
        this.isUpdate = true;
        this.faqId = p['id'];
        this.getFaqInfo();
      } else {
        this.faqId = '';
        this.isUpdate = false;
      }
    });
  }

  getGroups() {
    this.dataService.get(ServerApis.getFaqGroups).subscribe((response) => {
      if (response.isSuccess) this.groupList = response.data ? response.data : [];
      else {
        let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
        this.toastrService.error(msg);
      }
    });
  }

  getFaqInfo() {
    this.loading = true;
    this.dataService
      .get(ServerApis.getFaq, {
        id: this.faqId,
      })
      .subscribe(
        (response) => {
          this.loading = false;
          if (response.isSuccess && response.data) {
            this.faqForm.setValue({
              id: response.data.id,
              title: response.data.title,
              description: response.data.description,
              tagNames: response.data.tagNames,
              questionGroupTypeId: +response.data.questionGroupTypeId,
              isActive: response.data.isActive,
            });
            this.tagNames = response.data.tagNames ? response.data.tagNames.split(',') : [];
         
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

  add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;
    if ((value || '').trim()) {
      this.tagNames.push(value.trim());
    }
    if (input) {
      input.value = '';
    }
  }

  remove(item): void {
    const index = this.tagNames.indexOf(item);

    if (index >= 0) {
      this.tagNames.splice(index, 1);
    }
  }

  /**
   * for bind object in select
   * @param item
   */
  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.id == c2.id : c1 == c2;
  }


  getAttachmentId(ev:{uploadUrl:string}) {
    this.attachmentId = ev;
  }

  save() {
    if (this.faqForm.invalid) {
      this.toastrService.warning('اطلاعات فرم را تکمیل کنید.');
      this.faqForm.markAllAsTouched();
      return ;
    }

  
    let form = this.faqForm.value;
    let data = {
      id: this.faqId ? this.faqId : '',
      title: form.title,
      description: form.description,
      tagNames: this.tagNames.join(','),
      questionGroupTypeId: form.questionGroupTypeId,
      isActive: form.isActive,
    };
    this.isSaving = true;
    this.dataService.post(ServerApis.addOrUpdateFaq, data).subscribe(
      (response) => {
        this.isSaving = false;
        if (response.isSuccess) {
          this.toastrService.success('اطلاعات با موفقیت ثبت شد.');
          this.router.navigate(['/admin/faq-list']);
        } else {
          let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
          this.toastrService.error(msg);
        }
      },
      (error) => {
        this.isSaving = false;
        this.toastrService.error('متاسفانه خطایی در سرور رخ داده است!');
      },
    );
  }
}
