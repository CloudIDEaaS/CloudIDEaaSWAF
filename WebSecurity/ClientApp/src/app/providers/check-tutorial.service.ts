import { Injectable } from '@angular/core';
import { CanLoad, Router } from '@angular/router';
import { Storage } from '@capacitor/storage';
@Injectable({
  providedIn: 'root'
})
export class CheckTutorial implements CanLoad {
  constructor(private router: Router) {
  }

  canLoad() {
    return Storage.get({ key: 'ion_did_tutorial' }).then(res => {
      if (res) {
        this.router.navigate(['/app', 'tabs', 'project']);
        return false;
      } 
      else {
        return true;
      }
    });
  }
}
