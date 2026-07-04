import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-manzelat-plan-item',
  templateUrl: './manzelat-plan-item.component.html',
  styleUrls: ['./manzelat-plan-item.component.scss'],
  standalone: false,
})
export class CitizenManzelatPlanItemComponent extends AppBase implements OnInit {
  loading: boolean = true;
  isSaving: boolean = false;
  userId?: string;
  theForm: FormGroup;
  loadingState: boolean;
  baseEnums: any = {};
  baseUrl: string = ServerApis.baseUrl;
  imageUrl: string = '';
  data: any;
  id: number = 0;

  constructor() {
    super();
    this.route.params.subscribe((p) => {
      this.id = p['id'];
    });

    this.theForm = this.fb.group({
      chk_Janbazan: [true, []],
      chk_Janbazan_JesmiHarekati_NoWheelChair: [null, []],
      typ_Janbazan_JesmiHarekati_NoWheelChair: [null, []],
      chk_Janbazan_JesmiHarekati_WheelChair: [null, []],
      typ_Janbazan_JesmiHarekati_WheelChair: [null, []],
      chk_Janbazan_Zehni: [null, []],
      typ_Janbazan_Zehni: [null, []],
      chk_Janbazan_AsabRavan: [null, []],
      typ_Janbazan_AsabRavan: [null, []],
      chk_Janbazan_Binaei: [null, []],
      typ_Janbazan_Binaei: [null, []],
      chk_Janbazan_Shenavaei: [null, []],
      typ_Janbazan_Shenavaei: [null, []],
      chk_Janbazan_Sayer: [null, []],
      fu_Janbazan: [null, []],
      //معلولین
      chkMmaloulin: [true, []],
      chk_Maloulin_JesmiHarekati_NoWheelChair: [null, []],
      typ_Maloulin_JesmiHarekati_NoWheelChair: [null, []],
      chk_Maloulin_JesmiHarekati_WheelChair: [null, []],
      typ_Maloulin_JesmiHarekati_WheelChair: [null, []],
      chk_Maloulin_Zehni: [null, []],
      typ_Maloulin_Zehni: [null, []],
      chk_Maloulin_AsabRavan: [null, []],
      typ_Maloulin_AsabRavan: [null, []],
      chk_Maloulin_Binaei: [null, []],
      typ_Maloulin_Binaei: [null, []],
      chk_Maloulin_Shenavaei: [null, []],
      typ_Maloulin_Shenavaei: [null, []],
      chk_Maloulin_Sayer: [null, []],
      fu_Maloulin: [null, []],

      typ_ZananSarparast: [null, []],
      //بیماران خاص
      typ_SpecialDiseases: [null, []],
    });

    this.getBaseEnums();
    this.getCardInfo();
  }

  ngOnInit(): void {}

  getCardInfo() {
    this.dataService
      .get(ServerApis.getCitizenManzalat, { formBaseId: this.id })
      .subscribe((data) => {
        this.loading = false;

        this.theForm.patchValue({
          chk_Janbazan: data.data.chk_Janbazan,
          chk_Janbazan_JesmiHarekati_NoWheelChair:
            data.data.chk_Janbazan_JesmiHarekati_NoWheelChair,
          typ_Janbazan_JesmiHarekati_NoWheelChair:
            data.data.typ_Janbazan_JesmiHarekati_NoWheelChair,
          chk_Janbazan_JesmiHarekati_WheelChair: data.data.chk_Janbazan_JesmiHarekati_WheelChair,
          typ_Janbazan_JesmiHarekati_WheelChair: data.data.typ_Janbazan_JesmiHarekati_WheelChair,
          chk_Janbazan_Zehni: data.data.chk_Janbazan_Zehni,
          typ_Janbazan_Zehni: data.data.typ_Janbazan_Zehni,
          chk_Janbazan_AsabRavan: data.data.chk_Janbazan_AsabRavan,
          typ_Janbazan_AsabRavan: data.data.typ_Janbazan_AsabRavan,
          chk_Janbazan_Binaei: data.data.chk_Janbazan_Binaei,
          typ_Janbazan_Binaei: data.data.typ_Janbazan_Binaei,
          chk_Janbazan_Shenavaei: data.data.chk_Janbazan_Shenavaei,
          typ_Janbazan_Shenavaei: data.data.typ_Janbazan_Shenavaei,
          chk_Janbazan_Sayer: data.data.chk_Janbazan_Sayer,
          fu_Janbazan: data.data.fu_Janbazan,
          //معلولین
          chkMmaloulin: data.data.chkMmaloulin,
          chk_Maloulin_JesmiHarekati_NoWheelChair:
            data.data.chk_Maloulin_JesmiHarekati_NoWheelChair,
          typ_Maloulin_JesmiHarekati_NoWheelChair:
            data.data.typ_Maloulin_JesmiHarekati_NoWheelChair,
          chk_Maloulin_JesmiHarekati_WheelChair: data.data.chk_Maloulin_JesmiHarekati_WheelChair,
          typ_Maloulin_JesmiHarekati_WheelChair: data.data.typ_Maloulin_JesmiHarekati_WheelChair,
          chk_Maloulin_Zehni: data.data.chk_Maloulin_Zehni,
          typ_Maloulin_Zehni: data.data.typ_Maloulin_Zehni,
          chk_Maloulin_AsabRavan: data.data.chk_Maloulin_AsabRavan,
          typ_Maloulin_AsabRavan: data.data.typ_Maloulin_AsabRavan,
          chk_Maloulin_Binaei: data.data.chk_Maloulin_Binaei,
          typ_Maloulin_Binaei: data.data.typ_Maloulin_Binaei,
          chk_Maloulin_Shenavaei: data.data.chk_Maloulin_Shenavaei,
          typ_Maloulin_Shenavaei: data.data.typ_Maloulin_Shenavaei,
          chk_Maloulin_Sayer: data.data.chk_Maloulin_Sayer,
          fu_Maloulin: data.data.fu_Maloulin,
          typ_ZananSarparast: data.data.typ_ZananSarparast,
          typ_SpecialDiseases: data.data.typ_SpecialDiseases,
        });
      });
  }

  saveForm() {
    const form = this.theForm.value;
    this.dataService
      .post(ServerApis.addOrUpdateManzalat, {
        ...form,
        manzalatBaseFormId: this.id,
      })
      .subscribe(
        (response) => {
          if (response && response.isSuccess) {
            this.toastrService.success('اطلاعات با موفقیت ذخیره شد.');
            this.router.navigateByUrl('/citizen/upload-manzalat-form/' + this.id);
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

  /**
   * دریافت اطلاعات  پایه
   *
   * */
  getBaseEnums() {
    this.dataService.getEnums().subscribe(
      (response) => {
        if (response) {
          this.baseEnums.typ_AsabRavan = response.typ_AsabRavan;
          this.baseEnums.typ_Binaei = response.typ_Binaei;
          this.baseEnums.typ_JesmiHarekati_NoWheelChair = response.typ_JesmiHarekati_NoWheelChair;
          this.baseEnums.typ_JesmiHarekati_WheelChair = response.typ_JesmiHarekati_WheelChair;
          this.baseEnums.typ_Shenavaei = response.typ_Shenavaei;
          this.baseEnums.typ_Zehni = response.typ_Zehni;
          this.baseEnums.typ_ZananSarparast = response.typ_ZananSarparast;
          this.baseEnums.typ_SpecialDiseases = response.typ_SpecialDiseases;
        }
      },
      (error) => {
        this.toastrService.error('خطا در ارتباط با سرور!');
      },
    );
  }

  getImage(ev) {
    this.imageUrl = ev.uploadUrl;
  }
}
