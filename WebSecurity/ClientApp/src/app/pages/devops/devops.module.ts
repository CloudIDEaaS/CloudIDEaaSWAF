import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { DevopsPageRoutingModule } from './devops-routing.module';
import { DevopsPage } from './devops.page';
import { NgTerminalModule } from 'ng-terminal';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    DevopsPageRoutingModule,
    NgTerminalModule
  ],
  declarations: [DevopsPage]
})
export class DevopsModule {}
