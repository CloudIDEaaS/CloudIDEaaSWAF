import { NgModule } from '@angular/core';
import { BreadcrumbNavigator } from './breadcrumb-navigator';
import { IonicModule } from "@ionic/angular";
import { BreadcrumbModule } from "@cloudideaas/ngx-breadcrumb"
import { BreadCrumbNavigatorRoutingModule } from './breadcrumb-navigator-routing.module';

@NgModule({
  declarations: [
    BreadcrumbNavigator,
  ],
  imports: [
    BreadCrumbNavigatorRoutingModule,
    BreadcrumbModule,
    IonicModule,
  ],
  exports: [
    BreadcrumbNavigator
  ]
})
export class BreadCrumbNavigatorModule {
  constructor() {
  }
}
