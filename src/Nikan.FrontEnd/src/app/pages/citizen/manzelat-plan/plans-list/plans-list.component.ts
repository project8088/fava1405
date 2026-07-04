import { Component, OnInit } from '@angular/core';

import { DataService } from 'src/app/core/services/data-service.service';
import { ServerApis } from 'src/app/core/server-apis';

@Component({
  selector: 'app-plans-list',
  templateUrl: './plans-list.component.html',
  styleUrls: ['./plans-list.component.scss'],
})
export class PlansListComponent implements OnInit {
  loading: boolean = true;
  hasAddress: boolean = false;
  hasRegister: boolean = false;
  formStatuse: number = 0;

  manzelatForms: [];
  displayedColumns: string[] = ['title', 'registerForm', 'uploadFile', 'status', 'help'];

  routes = ['janbazan', 'maloulin', 'zanan-sarparast', 'bazneshaste', 'salmandan'];
  constructor(private dataService: DataService) {}

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
