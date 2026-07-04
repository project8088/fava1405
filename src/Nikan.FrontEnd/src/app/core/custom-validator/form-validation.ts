import { Injectable } from '@angular/core';
import { AbstractControl, ValidatorFn, FormGroup } from '@angular/forms';

@Injectable()
export class CustomFormValidators {
  /**
   * بررسی صحت کد ملی
   * use invalid
   * @param control  string
   */
  checkNationalCode(control) {
    if (!control.value) return null;
    var number = control.value;
    var returnModel = '',
      symbolMap = {
        '۱': '1',
        '۲': '2',
        '۳': '3',
        '۴': '4',
        '۵': '5',
        '۶': '6',
        '۷': '7',
        '۸': '8',
        '۹': '9',
        '۰': '0',
      };
    number = number.toString();
    for (var i = 0; i < number.length; i++)
      if (symbolMap[number[i]]) returnModel += symbolMap[number[i]];
      else returnModel += number[i];
    var nationalCode = returnModel;
    var isNumber = /^\d{10}$/.test(nationalCode);

    var isValid: boolean = false;

    if (isNumber && nationalCode.length == 10) {
      var allDigitEqual = [
        '0000000000',
        '1111111111',
        '2222222222',
        '3333333333',
        '4444444444',
        '5555555555',
        '6666666666',
        '7777777777',
        '8888888888',
        '9999999999',
      ];
      if (allDigitEqual.indexOf(nationalCode) > -1) isValid = false;
      else {
        var num0 = parseInt(nationalCode.charAt(0)) * 10;
        var num2 = parseInt(nationalCode.charAt(1)) * 9;
        var num3 = parseInt(nationalCode.charAt(2)) * 8;
        var num4 = parseInt(nationalCode.charAt(3)) * 7;
        var num5 = parseInt(nationalCode.charAt(4)) * 6;
        var num6 = parseInt(nationalCode.charAt(5)) * 5;
        var num7 = parseInt(nationalCode.charAt(6)) * 4;
        var num8 = parseInt(nationalCode.charAt(7)) * 3;
        var num9 = parseInt(nationalCode.charAt(8)) * 2;
        var a = parseInt(nationalCode.charAt(9));

        var b = num0 + num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9;
        var c = b % 11;

        if ((c < 2 && a == c) || (c >= 2 && 11 - c == a)) isValid = true;
        else isValid = false;
      }
    } else {
      isValid = false;
    }

    if (isValid == true) return null;
    else return { invalid: true };
  }

  /**
   *  بررسی صحت شماره موبایل
   * use invalid
   * @param control
   */
  checkMobileNumber(control: AbstractControl) {
    if (!control.value) return null;
    var number = control.value;
    var returnModel = '',
      symbolMap = {
        '۱': '1',
        '۲': '2',
        '۳': '3',
        '۴': '4',
        '۵': '5',
        '۶': '6',
        '۷': '7',
        '۸': '8',
        '۹': '9',
        '۰': '0',
      };
    number = number.toString();
    for (var i = 0; i < number.length; i++)
      if (symbolMap[number[i]]) returnModel += symbolMap[number[i]];
      else returnModel += number[i];
    var mobileNumber = returnModel;
    var isNumber = /^\d{11}$/.test(mobileNumber);
    if (!isNumber) return { invalid: true };

    // validation logic pre number
    var validNumber = [
      '901',
      '902',
      '903',
      '904',
      '905',
      '930',
      '933',
      '935',
      '936',
      '937',
      '938',
      '939', //irancel
      '910',
      '911',
      '912',
      '913',
      '914',
      '915',
      '916',
      '917',
      '918',
      '919',
      '990',
      '991',
      '992',
      '993',
      '994',
      '995',
      '996',
      '997',
      '998',
      '999', //MCI
      '920',
      '921',
      '922',
      '923',
      '924', //Rightel
      '931', //Espadan (MTCE)
      '921', //Taliya
      '934', //Kish-TCI
    ];
    var valid = false;
    // check validation mobile number
    for (var i = 0; i < validNumber.length; i++) {
      if (mobileNumber.toString().substring(1, 4) == validNumber[i]) valid = true;
    }

    if (valid) return null;
    else return { invalid: true };
  }

  /**
   * بررسی حروف فارسی
   * use pattern
   * @param control
   */
  checkPersianCharacters(control: AbstractControl) {
    if (!control.value) return null;
    var word = control.value;
    var isValid = /^([\u0600-\u06FF]+\s*)+$/.test(word);
    if (isValid) return null;
    else return { pattern: true };
  }

  /**
   * بررسی حروف انگلیسی
   * use pattern
   * @param control
   */
  checkEnglishCharacters(control: AbstractControl) {
    if (!control.value) return null;
    var word = control.value;
    var isValid = /^([a-zA-Z]+\s*)+$/.test(word);
    if (isValid) return null;
    else return { pattern: true };
  }

  /**
   * بررسی حروف انگلیسی و اعداد
   * use pattern
   * @param control
   */
  checkEnglishAndNumberCharacters(control: AbstractControl) {
    if (!control.value) return null;
    var word = control.value;
    var isValid = /^[a-zA-Z]+[a-zA-Z0-9-_@.]*$/.test(word);
    if (isValid) return null;
    else return { pattern: true };
  }
  /**
   * حروف لاتین بدون فاصله
   * @param control
   */
  checkEnglishWithoutSpace(control: AbstractControl) {
    if (!control.value) return null;
    var word = control.value;
    var isValid = /^[a-zA-Z]+[a-zA-Z0-9]$/.test(word);
    if (isValid) return null;
    else return { pattern: true };
  }

  /**
   * بررسی حروف انگلیسی
   * یا
   * اعداد
   */
  checkEnglishORNumberCharacters(control: AbstractControl) {
    if (!control.value) return null;
    var word = control.value;
    var isValid = /^[a-zA-Z]|[a-zA-Z0-9-_@.]*$/.test(word);
    if (isValid) return null;
    else return { pattern: true };
  }
  /**
   * بررسی تلفن ثابت به همراه کد
   * 0xxxxxxxxxx
   * use pattern
   * @param control
   */
  checkPhoneNumber(control: AbstractControl) {
    if (!control.value) return null;
    var number = control.value;
    var returnModel = '',
      symbolMap = {
        '۱': '1',
        '۲': '2',
        '۳': '3',
        '۴': '4',
        '۵': '5',
        '۶': '6',
        '۷': '7',
        '۸': '8',
        '۹': '9',
        '۰': '0',
      };
    number = number.toString();
    for (var i = 0; i < number.length; i++)
      if (symbolMap[number[i]]) returnModel += symbolMap[number[i]];
      else returnModel += number[i];

    var word = returnModel;
    var isValid = /^((?:0)[0-9]{10})$/.test(word);
    if (word.substr(0, 2) == '09') isValid = false;

    if (isValid) return null;
    else return { pattern: true };
  }

  /**
   * بررسی صحیت ایمیل
   * a@b.c
   * use email
   * @param control
   */
  checkEmail(control: AbstractControl) {
    if (!control.value) return null;
    var word = control.value;
    var isValid = /^[\w-.]+@([\w-]+\.)+[\w-]+$/.test(word); // /^.+@.+\..+$/.test(word);
    if (isValid) return null;
    else return { email: true };
  }

  /**
   * بررسی بازه سنی (شروع و پایان)
   * ageRangeFromوageRangeTo
   *
   * use range
   * */
  checkAgeRangeValidator: ValidatorFn = (fg: FormGroup) => {
    const start = fg.get('ageRangeFrom')?.value;
    const end = fg.get('ageRangeTo')?.value;
    if (!start && !end) return null;
    return start !== null && end !== null && start <= end ? null : { ageRange: true };
  };

  /**
   * بررسی کاراکترهای عددی
   * use pattern
   * @param control
   */
  checkNumberCharacters(control: AbstractControl) {
    if (!control.value) return null;
    var number = control.value;
    var returnModel = '',
      symbolMap = {
        '۱': '1',
        '۲': '2',
        '۳': '3',
        '۴': '4',
        '۵': '5',
        '۶': '6',
        '۷': '7',
        '۸': '8',
        '۹': '9',
        '۰': '0',
      };
    number = number.toString();
    for (var i = 0; i < number.length; i++)
      if (symbolMap[number[i]]) returnModel += symbolMap[number[i]];
      else returnModel += number[i];

    var word = returnModel;

    var isValid = /^-?(0|[0-9]\d*)?$/.test(word);
    if (isValid) return null;
    else return { pattern: true };
  }

  /**
   * حداقل 6 کاراکتر
   * حروف لاتین شامل اعداد و حروف بزرگ
   * @param control
   */
  strongPassword(control: AbstractControl) {
    if (!control.value) return null;
    var word = control.value;
    var isValid =
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%?&^#*()_\-=+])[A-Za-z\d$@$!%?&^#*()_\-=+]{6,}$/.test(
        word,
      );
    if (isValid) return null;
    else return { weakPassword: true };
  }

  /**
   * شامل این کاراکتر ها نباشد
   * - _
   * return invalidChar
   */
  notContainCharachter(control: AbstractControl) {
    if (!control.value) return null;
    var word = control.value;
    var inValid = /[!^\-_]/g.test(word);
    if (!inValid) return null;
    else return { invalidChar: true };
  }

  website(control: AbstractControl) {
    if (!control.value) return null;
    var word = control.value;
    var Valid =
      /(https?:\/\/)?(www\.)[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,4}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)|(https?:\/\/)?(www\.)?(?!ww)[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,4}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/g.test(
        word,
      );
    if (Valid) return null;
    else return { invalid: true };
  }
}
