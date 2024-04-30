import { Injectable } from '@angular/core';
import {Platform} from '@ionic/angular';

type CurrentPlatform = 'browser' | 'native' | 'mobile';

@Injectable({
  providedIn: 'root'
})
export class CurrentPlatformService {

  private _currentPlatform: CurrentPlatform;

  constructor(private platform: Platform) {
    this.setCurrentPlatform();
  }

  get currentPlatform() {
    return this._currentPlatform;
  }

  get isNative() {
    return this._currentPlatform === 'native';
  }
  get isBrowser() {
    return this._currentPlatform === 'browser';
  }

  private setCurrentPlatform() {
    // Are we on mobile platform? Yes if platform is ios or android, but not desktop or mobileweb, no otherwise
    if (
        this.platform.is('ios')
        || this.platform.is('android')
        && !( this.platform.is('desktop') || this.platform.is('mobileweb') ) ) {
      this._currentPlatform = 'mobile';
    } else {
      this._currentPlatform = 'browser';
    }
  }
}
