import { Component, OnInit, ViewChild, AfterViewInit, Input, Output, EventEmitter } from '@angular/core';
import { DataService } from '../../../../../core/services/data-service.service';
import { ToastrService } from 'ngx-toastr';
import { ServerApis } from '../../../../../core/server-apis'; 
import { AdminAddOrUpdateMenuDialogComponent } from '../dialog/add-update-menu/add-update-menu.component';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';

@Component({
  selector: 'adm-tree-menu',
  templateUrl: './tree-menu.component.html',
  styleUrls: ['./tree-menu.component.scss']
})
export class AdminTreeMenuComponent implements OnInit {
  @Input('menuItems') menuItems: any[];

  @Output() onUpdate=new  EventEmitter<boolean>();

     

  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private matDialog: MatDialog
  ) {

  

  }

  ngOnInit() { 
  }

   


  delete(row, ev: Event) {
    ev.preventDefault();
    ev.stopPropagation();
    Swal.fire({
      title: 'حذف',
      text: 'آیا برای حذف "' + row.menuName + '" اطمینان دارید؟',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: 'بله',
      cancelButtonText: 'خیر',
    }).then(result => {
      if (result.value) {
        this.dataService.get(ServerApis.removeMenu, {
          id: row.id,
        }).subscribe(response => {
          if (response.isSuccess) {
            this.toastrService.success('با موفقیت حذف شد.');
            //this.menuItems.forEach((item, index) => {
            //  if (item.id == row.id)
            //    this.menuItems.splice(index, 1);
            //});
            this.onUpdate.emit(true);
          } else {
            let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
            this.toastrService.error(msg);
          }
        }, error => {
        });
      }

    });
  }



  openMenuDialog(menu) {
    this.matDialog.open(AdminAddOrUpdateMenuDialogComponent, {
      data: menu,
      panelClass: 'custom-dialog'
    }).afterClosed().subscribe(resp => {
      if (resp)
        this.onUpdate.emit(true);

    });
  }

}
