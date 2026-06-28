import { ActivatedRoute, Router } from '@angular/router';
import { Component, ElementRef, Input, OnDestroy, OnInit } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

import { AuthService } from 'src/app/core/authentication/auth.service';
import { AuthUser } from '../../core/authentication/user.model';
import { DataService } from '../../core/services/data-service.service';
import { MatDialog } from '@angular/material/dialog';
import { ServerApis } from '../../core/server-apis';
import { SideNavMenuItem } from '../../core/models/menuItems';
import { ToastrService } from 'ngx-toastr';
import { UploadUserAvatarDialogComponent } from '../_dialog/upload-avatar/upload-avatar.component';

@Component({
  selector: 'side-nav-menu',
  templateUrl: './side-nav-menu.component.html',
  styleUrls: ['./side-nav-menu.component.scss'],
})
export class SideNavMenuComponent implements OnInit {
  @Input('menu') menuItems: SideNavMenuItem[];
  @Input('showLogout') showLogout: boolean = true;

  isLoading: boolean = true;
  //user: AuthUser;

  menuIsOpen: string;
  countofElements: number = 0;
  user: AuthUser;
  baseUrl: string = ServerApis.baseUrl;
  userImage: string;

  constructor(
    private toastrService: ToastrService,
    private elementRef: ElementRef,
    private sanitizer: DomSanitizer,
    private authService: AuthService,
    private router: Router,
    private matDialog: MatDialog,
    private dataService: DataService
  ) {
    this.authService.currentUser.subscribe((u) => {
      this.user = u;
    });
  }

  ngOnInit(): void {
    this.setId(this.menuItems);
    let parent = this.getParent(this.menuItems, this.menuIsOpen, '');
    while (parent) {
      parent.isOpen = true;
      parent = this.getParent(this.menuItems, parent.id, '');
    }

    if (this.user.isJobseeker) this.getJobseekerImage();
    if (this.user.isCompany) this.getCompanyLogo();
    if (this.user.isCitizen) this.getCitizenImage();
  }

  setId(menu: SideNavMenuItem[]) {
    for (var item of menu) {
      this.countofElements++;
      item.id = 'M-' + this.countofElements;
      if (item.url == this.router.url) this.menuIsOpen = item.id;
      if (item.children) this.setId(item.children);
    }
  }

  getParent(root, id, parent) {
    var finded = '';
    for (var n of root) {
      if (n.id === id) {
        return (finded = parent);
      }
      if (n.children) {
        parent = n;
        return this.getParent(n.children, id, parent);
      }
    }
    return finded;
  }

  logout() {
    this.authService.logout(true);
  }

  openUploadDialog() {
    if (!this.user.isCompany && !this.user.isJobseeker && !this.user.isCitizen)
      return false;

    this.matDialog
      .open(UploadUserAvatarDialogComponent, {
        data: { imageUrl: this.userImage },
        panelClass: 'custom-dialog',
        width: '85%',
        height: '90%',
      })
      .afterClosed()
      .subscribe((res) => {
        if (res) {
          this.userImage = res;
        }
      });
  }

  getJobseekerImage() {
    this.dataService.get(ServerApis.getKarjoImage).subscribe((response) => {
      if (response.isSuccess) this.userImage = response.data.imageUrl;
    });
  }
  getCompanyLogo() {
    this.dataService.get(ServerApis.getCompanyLogo).subscribe((response) => {
      if (response.isSuccess) this.userImage = response.data.imageUrl;
    });
  }
  getCitizenImage() {
    this.dataService
      .get(ServerApis.getShortCitizenInfoByCitizen)
      .subscribe((response) => {
        if (response.isSuccess)
          this.userImage =
            response.data.personalPictureUrl + '?v=' + Math.random() * 1000;
      });
  }
}
