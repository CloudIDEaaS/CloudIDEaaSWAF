import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewerPage } from '../viewer/viewer.page';

const routes: Routes = [
  {
    path: '',
    component: ViewerPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ViewerPageRoutingModule { }
