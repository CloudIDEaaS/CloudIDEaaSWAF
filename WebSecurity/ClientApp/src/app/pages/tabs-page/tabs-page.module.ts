import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { TabsPage } from './tabs-page';
import { TabsPageRoutingModule } from './tabs-page-routing.module';
import { AboutModule } from '../about/about.module';
import { ProjectModule } from '../project/project.module';
import { DevopsModule } from '../devops/devops.module';

@NgModule({
  imports: [
    AboutModule,
    CommonModule,
    IonicModule,
    ProjectModule,
    DevopsModule,
    TabsPageRoutingModule
  ],
  declarations: [
    TabsPage,
  ]
})
export class TabsModule { }
