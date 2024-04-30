import { Component, OnInit } from '@angular/core';
import { UserProvider } from '../../providers/user.provider';
import { Router } from '@angular/router';
import { ToastController, LoadingController } from '@ionic/angular';
import { LoadOrToastProvider } from '../../providers/loadOrToast';
declare var require: any;
const queryString = require('query-string');

@Component({
  selector: 'app-email-verification',
  templateUrl: './email-verification.page.html',
  styleUrls: ['./email-verification.page.scss'],
})
export class EmailVerificationPage implements OnInit {

  public id: string;
  public verified: boolean;

  constructor(public userProvider: UserProvider,
    public loadOrToastProvider: LoadOrToastProvider,
    public router: Router,
    public toastCtrl: ToastController,
    public loadingController: LoadingController,
  ) {

    let parms = queryString.parse(window.location.search);

    this.id = Object.keys(parms)[0];

    if (this.id) {
      loadOrToastProvider.loadOrToast<any>(() => this.userProvider.verifyEmail(this.id), "Verifying, please wait...").subscribe((resp) => {

        this.verified = true;
        console.debug("success!");

        setTimeout(() => {
          router.navigateByUrl("login");
        }, 3000);

      }, (err) => {

        router.navigateByUrl("login");
        console.debug(err);
      });
    }
    else {
    }
  }

  ngOnInit() {
  }
}
