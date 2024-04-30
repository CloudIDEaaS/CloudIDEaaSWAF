import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastController, LoadingController } from '@ionic/angular';
import { Observable } from 'rxjs';

@Injectable()
export class LoadOrToastProvider {

  constructor(public router: Router,
    public toastCtrl: ToastController,
    public loadingController: LoadingController,
  ) {
  }

  public loadOrToast<T>(providerFunction: () => Observable<T>, loadingMessage: string, navUrlSuccess: string | null = null, navUrlError: string | null = null, successNavWait: number = 100, errorNavWait: number = 100): Observable<T> {

    let observable = <Observable<T>> Observable.create((o : any) => {

      let loadingController = this.loadingController.create({message : loadingMessage}).then(loading => {

        loading.present();

        providerFunction().subscribe((resp) => {

          if (navUrlSuccess) {

            setTimeout(() => {

              this.router.navigateByUrl(navUrlSuccess, { replaceUrl: true });
              loading.dismiss();

              o.next(resp);
              o.complete();

            }, successNavWait);
          }
          else {

            loading.dismiss();
  
            o.next(resp);
            o.complete();

          }
        }, (err) => {

          let message = err.message;

          if (err.error) {
            if (typeof err.error === "string") {
              message = err.error;
            }
          }

          let toastController = this.toastCtrl.create({message: message, duration: 3000,position: 'top'}).then(toast => {
            toast.present();
          });

          loading.dismiss();

          if (navUrlError) {

            setTimeout(() => {

              this.router.navigateByUrl(navUrlError, { replaceUrl: true });
              loading.dismiss();

              o.error(err);
              o.complete();

            }, errorNavWait);
          }
          else {

            loading.dismiss();

            o.error(err);
            o.complete();
          }
        });
      });
    });

    return observable;
  }
}
