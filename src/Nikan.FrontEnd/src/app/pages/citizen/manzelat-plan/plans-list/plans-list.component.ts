import { Component, OnInit } from '@angular/core';
import { ServerApis } from '@core/server-apis';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'app-plans-list',
  templateUrl: './plans-list.component.html',
  styleUrls: ['./plans-list.component.scss'],
  standalone: false,
})
export class PlansListComponent extends AppBase implements OnInit {
  loading: boolean = true;
  hasAddress: boolean = false;
  hasRegister: boolean = false;
  formStatuse: number = 0;

  manzelatForms: any[] = [];
  displayedColumns: string[] = ['title', 'registerForm', 'uploadFile', 'status', 'help'];

  routes = ['janbazan', 'maloulin', 'zanan-sarparast', 'bazneshaste', 'salmandan'];
  constructor() {
    super();
  }

  ngOnInit(): void {
    this.getManzelatSetrices();
  }

  getManzelatSetrices() {
    this.loading = true;
    this.dataService.get(ServerApis.getAllAvailableManzaltForm).subscribe((data) => {
      this.loading = false;
      debugger;
      this.manzelatForms = data.data.forms;
      this.hasAddress = data.data.hasAddress;
      this.hasRegister = data.data.hasRegister;
      this.formStatuse = data.data.formStatuse;
    });
  }
}
