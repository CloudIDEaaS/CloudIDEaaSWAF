import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { UserProvider } from './user.provider';

@Injectable()
export class RouteGuardProvider implements CanActivate {

  constructor(public userProvider: UserProvider, public router: Router) {
  }

  canActivate(): boolean {

    if (!this.userProvider.isLoggedIn()) {
      this.router.navigate(['login']);
      return false;
    }

    return true;
  }
}
