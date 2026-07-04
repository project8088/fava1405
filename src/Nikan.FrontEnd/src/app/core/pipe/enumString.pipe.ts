import { Pipe, PipeTransform, Injectable } from '@angular/core';
import { DataService } from '../services/data-service.service';

@Pipe({
  name: 'enumString',
  //  pure: false
})
@Injectable()
export class EnumStringPipe implements PipeTransform {
  enums: any;
  constructor(private dataService: DataService) {
    this.dataService.getEnums().subscribe((response) => {
      this.enums = response;
    });
  }

  transform(val: string, object: string = '') {
    if (val != undefined && val != null) {
      for (let property in this.enums) {
        if (property == object) {
          var txt = this.enums[property].find((v) => v.key == val);
          if (txt) return txt.text.replace(/_/g, ' ');
          else return '';
        }
      }
    }
  }
}
