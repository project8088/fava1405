import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '../core/core.module';
import { MainHeaderComponent } from './main-header/main-header.component';
import { RouterModule } from '@angular/router';
import { MainFooterComponent } from './main-footer/main-footer.component';
import { MainTreeMenuComponent } from './main-header/tree-menu/tree-menu.component';

@NgModule({
  declarations: [MainHeaderComponent, MainFooterComponent, MainTreeMenuComponent],
  imports: [RouterModule, CommonModule, CoreModule, FormsModule, ReactiveFormsModule],
  exports: [RouterModule, MainHeaderComponent, MainFooterComponent],
})
export class LayoutsModule {}
