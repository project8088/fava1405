import { Pipe, PipeTransform, Injectable } from '@angular/core';

@Pipe({
  name: 'toman'
})
@Injectable()
export class TomanPipe implements PipeTransform {



 s1 = new Array("", "يک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه");
 s2 = new Array("ده", "يازده", "دوازده", "سيزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده");
 s3 = new Array("", "", "بيست", "سي", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود");
 s4 = new Array("", "صد", "دويست", "سيصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد");
 result = '';

  transform(val: number): string {
    if (val)
      return this.convert(val.toString());
  }








/*------------------------------------------------------------------------------------------------*/
convert(z) {
  z = z.replace(/[^a-zA-Z0-9 ]/g, "");

  z = parseInt(z)
  if (z == 0) { this.result = "صفر" } else {
    this.result = ""
    this.convert2(z)
  }
  //alert("."+this.result) 

  if (this.result == "Error") {
    return ("--خطا--");
  }
  else {
    return this.result + " تومان";
  }
}

 convert2(y) {
  if (y > 999999999 && y < 1000000000000) { var bghb = (y % 1000000000); var temp = y - bghb; var bil = temp / 1000000000; this.convert3r(bil); this.result = this.result + " ميليارد"; if (bghb != 0) { this.result = this.result + " و "; this.convert2(bghb); } }
  else if (y > 999999 && y <= 999999999) { var bghm = (y % 1000000); temp = y - bghm; var mil = temp / 1000000; this.convert3r(mil); this.result = this.result + " ميليون"; if (bghm != 0) { this.result = this.result + " و "; this.convert2(bghm); } }
  else if (y > 999 && y <= 999999) { var bghh = (y % 1000); temp = y - bghh; var hez = temp / 1000; this.convert3r(hez); this.result = this.result + " هزار"; if (bghh != 0) { this.result = this.result + " و "; this.convert2(bghh); } }
  else if (y <= 999) this.convert3r(y); else this.result = "Error";
}

 convert3r(x) {
  var bgh = (x % 100); var temp = x - bgh; var sad = temp / 100;
  if (bgh == 0) { this.result = this.result + this.s4[sad] }
  else {
    if (x > 100) this.result = this.result + this.s4[sad] + " و ";
    if (bgh < 10) { this.result = this.result + this.s1[bgh] }
    else if (bgh < 20) { var bgh2 = (bgh % 10); this.result = this.result + this.s2[bgh2] }
    else {
      bgh2 = (bgh % 10); temp = bgh - bgh2; var dah = temp / 10;
      if (bgh2 == 0) { this.result = this.result + this.s3[dah] }
      else { this.result = this.result + this.s3[dah] + " و " + this.s1[bgh2] }
    }
  }
}


 itpro(Number) {
  Number += '';
  Number = Number.replace(',', ''); Number = Number.replace(',', ''); Number = Number.replace(',', '');
  Number = Number.replace(',', ''); Number = Number.replace(',', ''); Number = Number.replace(',', '');
  var x = Number.split('.');
  var y = x[0];
  var z = x.length > 1 ? '.' + x[1] : '';
  var rgx = /(\d+)(\d{3})/;
  while (rgx.test(y))
    y = y.replace(rgx, '$1' + ',' + '$2');
  return y + z;
}









}
