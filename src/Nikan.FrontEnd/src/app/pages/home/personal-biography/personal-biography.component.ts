import { Component, OnInit, ViewChild } from '@angular/core'; 
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog'; 
import { DataService } from '../../../core/services/data-service.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { AuthUser } from '../../../core/authentication/user.model';
import { AuthService } from '../../../core/authentication/auth.service';
import Swal from 'sweetalert2';
import { ServerApis } from '../../../core/server-apis';
import { Meta, Title } from '@angular/platform-browser';
   
@Component({
  selector: 'home-personal-biography',
  templateUrl: './personal-biography.component.html',
  styleUrls: ['./personal-biography.component.scss']
})
export class PersonalBiographyComponent implements OnInit {
  id: string;
  loading: boolean;
  userInfo: any;
  baseUrl: string = ServerApis.baseUrl;

  constructor(
    private dataService: DataService,
    private toastrService: ToastrService,
    private router: Router,
    private matDialog: MatDialog,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authService: AuthService,
    private titleService: Title,
    private metaService: Meta
  ) {
    this.route.params.subscribe(p => {
      this.id = p.id;
      this.getUserInfo();
    });  
  }



  ngOnInit() {}

  getUserInfo() {
    this.loading = true;
    this.dataService.get(
      ServerApis.getPersonelInfoForView, { id: this.id }
    ).subscribe(response => {
      this.loading = false;
      if (response && response.isSuccess) {
        this.userInfo = response.data;

        if (this.userInfo.seoTags) {
          this.titleService.setTitle(this.userInfo.seoTags.seoTitle);
          this.metaService.addTags([
            { name: 'keywords', content: this.userInfo.seoTags.seokeywords },
            { name: 'description', content: this.userInfo.seoTags.seoDescription },
            { name: 'robots', content: 'index, follow' }
          ]);
        }

      } else {
        let msg = response.messages ? response.messages : "متاسفانه خطایی در سرور رخ داده است!";
        this.toastrService.error(msg);
        this.router.navigate(['/home']);
      }
    }, error => {
      this.loading = false;
    });
  }


}


