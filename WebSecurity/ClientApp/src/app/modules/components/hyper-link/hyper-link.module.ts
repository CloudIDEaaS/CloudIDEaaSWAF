import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from '../../../app-routing.module';
import { HyperLink } from './hyper-link';

@NgModule({
  declarations: [
    HyperLink,
  ],
  imports: [
    RouterModule,
    AppRoutingModule,
  ],
  exports: [
    HyperLink
  ]
})
export class HyperLinkModule
{

}
