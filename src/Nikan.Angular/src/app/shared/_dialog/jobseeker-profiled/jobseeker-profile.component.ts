import { Component, OnInit, Inject } from '@angular/core'; 
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-jobseeker-profile-dialog',
  templateUrl: './jobseeker-profile.component.html',
  styleUrls: ['./jobseeker-profile.component.scss']
})
export class JobseekerProfileDialogComponent implements OnInit {
  id: string; 
  
  


  constructor(
    private matDialog: MatDialog,
    private matDialogRef: MatDialogRef<JobseekerProfileDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any,
  ) {
    debugger;
    if (_data) {
      this.id = _data.id;  

    }
  }

  ngOnInit(): void {
  }




  closeDialog(result) {
    this.matDialogRef.close(result);
  }




}
