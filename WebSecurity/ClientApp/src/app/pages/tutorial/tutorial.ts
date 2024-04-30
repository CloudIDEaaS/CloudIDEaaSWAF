import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MenuController, IonSlides } from '@ionic/angular';
import { Storage } from '@capacitor/storage';

@Component({
  selector: 'page-tutorial',
  templateUrl: 'tutorial.html',
  styleUrls: ['./tutorial.scss'],
})
export class TutorialPage {
  showSkip = true;

  @ViewChild('slides', { static: true }) slides: IonSlides;

  constructor(
    public menu: MenuController,
    public router: Router
  ) {
  }

  startApp() {
    this.router
      .navigateByUrl('/app/tabs/project', { replaceUrl: true })
      .then(() => Storage.set({ key: 'ion_did_tutorial', value: "true" }));
  }

  onSlideChangeStart(event) {
    event.target.isEnd().then(isEnd => {
      this.showSkip = !isEnd;
    });
  }

  ionViewWillEnter() {
    Storage.get({ key: 'ion_did_tutorial' }).then(res => {
      if (res.value === "true") {
        this.router.navigateByUrl('/app/tabs/project', { replaceUrl: true });
      }
    });

    this.menu.enable(false);
  }

  ionViewDidLeave() {
    // enable the root left menu when leaving the tutorial page
    this.menu.enable(true);
  }
}
