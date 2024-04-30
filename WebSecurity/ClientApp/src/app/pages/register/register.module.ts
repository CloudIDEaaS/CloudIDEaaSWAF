import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { NgxMaskIonicModule } from '@cloudideaas/ngx-mask-ionic';
import { RegisterPage } from './register';
import { RegisterPageRoutingModule } from './register-routing.module';
import { DirectivesModule } from '../../directives.module';
import { PasswordStrengthBarModule } from 'ng2-password-strength-bar';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ReactiveFormsModule,
    RegisterPageRoutingModule,
    NgxMaskIonicModule,
    DirectivesModule,
    PasswordStrengthBarModule
  ],
  declarations: [
    RegisterPage
  ]
})
export class RegisterModule { }
