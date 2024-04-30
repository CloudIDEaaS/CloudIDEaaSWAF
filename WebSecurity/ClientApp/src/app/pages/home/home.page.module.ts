import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HomePage } from './home.page';
import { BreadcrumbModule } from "@cloudideaas/ngx-breadcrumb";
import { HomePageRoutingModule } from './home.page.routing.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    BreadcrumbModule,
    IonicModule,
    HomePageRoutingModule
  ],
  declarations: [ HomePage ]
})
export class HomePageModule {}
