import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserProvider } from '../../providers/user.provider';
import { ToastController, LoadingController } from '@ionic/angular';
import { LoginUser } from '../../models/loginuser.model';
import { NGXLogger } from 'ngx-logger';
import { IndexPageLoader } from 'src/app/providers/index-page-loader';
import { environment } from 'src/environments/environment';
import * as $ from "jquery";
@Component({
  selector: 'page-login',
  templateUrl: 'login.html',
  styleUrls: ['./login.scss'],
})
export class LoginPage implements OnInit {
  login: LoginUser = new LoginUser({ userName: "" });
  submitted = false;
  public captchaResolved: boolean = false;
  public environment = environment;
  constructor(
    public userProvider: UserProvider,
    public toastCtrl: ToastController,
    public loadingController: LoadingController,
    public router: Router,
    private indexPageLoader: IndexPageLoader,
    private logger: NGXLogger
  ) {
    this.logger.debug("Hit constructor of LoginPage");
  }
  ngOnInit(): void {
    this.indexPageLoader.setLoginPageLoadStart();
  }
  // { #if !DEBUG }
    /*** (auto added, do not remove)   checkCaptcha(captchaResponse: string) { ***/
    /*** (auto added, do not remove)     this.indexPageLoader.captchaResponse = captchaResponse; ***/
    /*** (auto added, do not remove)     this.captchaResolved = (captchaResponse && captchaResponse.length > 0) ? true : false; ***/
    /*** (auto added, do not remove)     if (this.captchaResolved) { ***/
    /*** (auto added, do not remove)       this.indexPageLoader.setCaptchaResolvedEnd(); ***/
    /*** (auto added, do not remove)     } ***/
    /*** (auto added, do not remove)   } ***/
  // { #endif }
  onLogin(form: NgForm) {
    if (form.valid) {
      this.submitted = true;
      this.indexPageLoader.setSubmitStart();
      this.loadingController.create({ message: "Logging in, please wait..." }).then(async loading => {
        loading.present();
        (await this.userProvider.login(this.login.userData)).subscribe((resp) => {
          setTimeout(() => {
            this.router.navigateByUrl('/app/tabs/geo', { replaceUrl: true });
            loading.dismiss();
          }, 100);
        }, (err) => {
          let message = err.message;
          if (err.error) {
            if (typeof err.error === "string") {
              message = err.error;
            }
          }
          this.toastCtrl.create({ message: message, duration: 3000, position: 'top' }).then(toast => {
            toast.present();
          });
          loading.dismiss();
        });
      });
    }
  }
  onRegister() {
    this.router.navigateByUrl('/register');
  }
  onUserNameInput(value: string) {
    this.indexPageLoader.setUserNameEnteredEnd();
  }
  onPasswordInput(value: string) {
    this.indexPageLoader.setPaswordEnteredEnd();
    // { #if DEBUG }
    setTimeout(() => {
      this.captchaResolved = true;
      this.indexPageLoader.setCaptchaResolvedEnd();
    }, 1000);
    // { #endif }
  }
}