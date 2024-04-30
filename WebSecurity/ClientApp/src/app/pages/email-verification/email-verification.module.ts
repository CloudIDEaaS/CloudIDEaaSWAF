import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { EmailVerificationPageRoutingModule } from './email-verification-routing.module';
import { EmailVerificationPage } from './email-verification.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    EmailVerificationPageRoutingModule
  ],
  declarations: [EmailVerificationPage]
})
export class EmailVerificationPageModule {}
