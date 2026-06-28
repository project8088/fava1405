import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';

import { DataService } from '../../core/services/data-service.service';
import { ServerApis } from '../../core/server-apis';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent implements OnInit {
 
  form: FormGroup; 
  loading: boolean;
  provinceList: any[] = [];

  constructor(
    private _formBuilder: FormBuilder,
    private dataService: DataService,
    private toastrService: ToastrService
  ) {
    this.form = this._formBuilder.group({
      province: [null, []],
      state: [null, []],
        area: [null, []],
      plate: ['67 ب 771 67', []]
 });


  }

  ngOnInit(): void {
    this.getProvinces();
  }

  /**
   *دریافت لیست استان ها
   * */
  getProvinces() { 
    this.dataService.get(ServerApis.getProvinces).subscribe(response => {
      this.loading = false;
      this.provinceList = response.data ? response.data : [];
    
    }, error => {
      this.loading = false;
      this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
    });
  }



}
