import { Component, OnInit, Inject } from '@angular/core'; 
import { ToastrService } from 'ngx-toastr';
 import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'admin-event-details-dialog',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.scss']
})
export class AdminViewEventDetailsDialogComponent implements OnInit {
  id: string = '';




  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<AdminViewEventDetailsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any, 
    private toastrService: ToastrService
  ) {
    if (_data) {
      this.id = _data.id;
    }
  }

  ngOnInit(): void {
  }






}
