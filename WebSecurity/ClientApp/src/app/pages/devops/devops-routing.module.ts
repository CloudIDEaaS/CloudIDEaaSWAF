import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DevopsPage } from './devops.page';

const routes: Routes = [
  {
    path: '',
    component: DevopsPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DevopsPageRoutingModule {}
