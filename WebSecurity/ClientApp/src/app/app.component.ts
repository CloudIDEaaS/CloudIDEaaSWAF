import { Component, OnInit, ViewEncapsulation, ElementRef, ViewChild, NgZone, ChangeDetectorRef, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { SwUpdate } from '@angular/service-worker';
import { MenuController, Platform, ToastController, IonSplitPane, IonIcon } from '@ionic/angular';
import { Storage } from '@capacitor/storage';
import { UserProvider } from './providers/user.provider';
import { createAnimation } from "@ionic/core";
import * as $ from "jquery";
import { LoggerProxy } from './modules/utils/loggerProxy';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {
  appPages = [
    {
      title: 'Geo',
      url: '/app/tabs/geo',
      icon: 'earth'
    },
  ];
  loggedIn = false;
  dark = false;
  expanderIconAnimation: any;
  menuCollapsible: boolean;
  collapsibleExpression = "min-width: 200px";
  defaultExpression = "min-width: 992px";
  @ViewChild("splitPane", { static: true }) splitPane: IonSplitPane;
  @ViewChild("ionIcon", { static: true }) ionIcon: IonIcon;
  constructor(
    private menu: MenuController,
    private platform: Platform,
    private router: Router,
    private userProvider: UserProvider,
    private swUpdate: SwUpdate,
    private toastCtrl: ToastController,
    private zone: NgZone,
    private changeDetector: ChangeDetectorRef
  ) {

    this.initializeApp();
  }

  async ngOnInit() {

    let ξ = window["ξ"];

    if (ξ) {
      const toast = await this.toastCtrl.create({
        message: ξ,
        position: 'top',
      });

      await toast.present();
    }

    this.initializeMenuAnimation();
    this.checkLoginStatus();
    this.listenForLoginEvents();

    this.swUpdate.available.subscribe(async res => {
      const toast = await this.toastCtrl.create({
        message: 'Update available!',
        position: 'bottom',
        buttons: [
          {
            role: 'cancel',
            text: 'Reload'
          }
        ]
      });

      await toast.present();

      toast.onDidDismiss()
        .then(() => this.swUpdate.activateUpdate())
        .then(() => window.location.reload());
    });
  }

  get menuAsCollapsibleExpression() {
    return this.collapsibleExpression;
  }

  @HostListener('window:unload', ['$event'])
  unloadHandler(event) {
    this.logout();
  }

  @HostListener('window:beforeunload', ['$event'])
  beforeUnloadHandler(event) {
  }

  onSplitPaneChange(e) {

    if (e.detail.visible) {
      this.menuCollapsible = false;
    }
    else {
      this.menuCollapsible = true;
    }
  }

  getSplitPaneWhen(): boolean {

    if (this.menuCollapsible) {
      return false;
    }
    else {
      return true;
    }
  }

  setMenuAsCollapsible(event: Event) {

    this.expanderIconAnimation.play();

    setTimeout(() => {

      if (this.menuCollapsible) {
        this.ionIcon.name = "caret-down-outline";
        this.menuCollapsible = false;
      }
      else {
        this.ionIcon.name = "caret-back-outline";
        this.menuCollapsible = true;
      }

      this.expanderIconAnimation.stop();
      this.initializeMenuAnimation();

      this.zone.run(() => {
        this.changeDetector.markForCheck();
      });

    }, 200);
  }

  initializeMenuAnimation() {

    let expanderIcon = $(".expander-icon");
    let degrees = this.menuCollapsible ? -90 : 90;

    this.expanderIconAnimation = createAnimation()
      .addElement(expanderIcon[0])
      .duration(100)
      .keyframes([
        { offset: 0, transform: 'rotate(0)' },
        { offset: 1, transform: `rotate(${degrees}deg)` }
      ]);
  }

  initializeApp() {
    this.platform.ready().then(() => {
    });
  }

  checkLoginStatus() {
    if (this.userProvider.isLoggedIn()) {
      return this.updateLoggedInStatus(true);
    }
    else {
      return this.updateLoggedInStatus(false);
    }
  }

  updateLoggedInStatus(loggedIn: boolean) {
    setTimeout(() => {
      this.loggedIn = loggedIn;
    }, 300);
  }

  listenForLoginEvents() {
    window.addEventListener('user:login', () => {
      this.updateLoggedInStatus(true);
    });

    window.addEventListener('user:register', () => {
      this.updateLoggedInStatus(true);
    });

    window.addEventListener('user:logout', () => {
      this.updateLoggedInStatus(false);
    });
  }

  logout() {
    if (this.userProvider.isLoggedIn()) {
      this.userProvider.logout().subscribe((b) => {
        return this.router.navigateByUrl("login");
      }, (e) => {
        return this.router.navigateByUrl("login");
      });
    }
  }

  openTutorial() {
    this.menu.enable(false);
    Storage.set({ key: 'ion_did_tutorial', value: "false" });
    this.router.navigateByUrl('/tutorial');
  }
}
