import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { LoginPage } from './login';
import { LoginPageRoutingModule } from './login-routing.module';
import { DirectivesModule } from '../../directives.module';
import { RecaptchaFormsModule, RecaptchaModule } from "ng-recaptcha";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    LoginPageRoutingModule,
    RecaptchaModule,
    RecaptchaFormsModule,
    DirectivesModule,
  ],
  declarations: [
    LoginPage
  ]
})
export class LoginModule { }
