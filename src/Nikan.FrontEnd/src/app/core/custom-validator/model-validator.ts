import { Injectable } from '@angular/core';

@Injectable()
export class CustomModelValidators {
  /**
   * بررسی صحت کد ملی
   * @param nationalCode  string
   */
  checkNationalCode(nationalCode: string) {
    if (!nationalCode) return null;
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
    } else isValid = false;

    return isValid;
  }

  /**
   *  بررسی صحت شماره موبایل
   * @param mobileNumber
   */
  checkMobileNumber(mobileNumber: string) {
    if (!mobileNumber) return null;
    var isNumber = /^\d{11}$/.test(mobileNumber);
    if (!isNumber) return false;

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
      '991', //MCI
      '920',
      '921',
      '922', //Rightel
      '931', //Espadan (MTCE)
      '921', //Taliya
      '934', //Kish-TCI
    ];
    var valid = false;
    // check validation mobile number
    for (var i = 0; i < validNumber.length; i++) {
      if (mobileNumber.toString().substring(1, 4) == validNumber[i]) valid = true;
    }

    return valid;
  }

  /**
   * بررسی صحت کد ملی
   * @param nationalCode  string
   */
  checkPersianNumber(number: string) {
    if (!number) return null;
    var returnModel = '';
    var symbolMap: any = {
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

    return returnModel;
  }

  /**
   * بررسی حروف فارسی
   * @param word
   */
  checkPersianCharacters(word: string) {
    if (!word) return null;
    var isValid = /^([\u0600-\u06FF]+\s*)+$/.test(word);
    return isValid;
  }

  /**
   * بررسی تلفن ثابت به همراه کد
   * 0xxxxxxxxxx
   * @param phoneNumber
   */
  checkPhoneNumber(phoneNumber: string) {
    if (!phoneNumber) return null;
    var isValid = /^((?:0)[0-9]{10})$/.test(phoneNumber);
    return isValid;
  }

  /**
   * بررسی صحیت ایمیل
   * a@b.c
   * @param email
   */
  checkEmail(email: string) {
    if (!email) return null;
    var isValid = /^[\w-.]+@([\w-]+\.)+[\w-]+$/.test(email); // /^.+@.+\..+$/.test(word);
    return isValid;
  }
}
