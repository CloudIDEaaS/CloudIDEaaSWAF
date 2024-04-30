// import { IonicPage, NavController, LoadingController } from "ionic-angular";
// import { TranslateService } from "@ngx-translate/core";
// import { Authorize } from "../modules/utils/AuthorizeDecorator";
// import { Component, NgZone, ViewChild } from "@angular/core";
import { List } from "linq-javascript";
import { User } from "../models/user.model";
import { Injectable, Injector, ComponentFactoryResolver, Type } from '@angular/core';
import { Api } from './api/api';
import { AccessTokenInfo } from '../models/accesstokeninfo.model';
import { map, filter, mergeMap, tap } from "rxjs/operators";
import { Observable } from 'rxjs';
import { IndexPageLoader } from "./index-page-loader";
import clientInfo from "../../environments/api.json";

// import { ViewController, Nav } from 'ionic-angular';
// import { App } from 'ionic-angular/components/app/app';
// import { NavControllerBase } from 'ionic-angular/navigation/nav-controller-base';

@Injectable()
export class UserProvider {
  user: User;
  issuer = "CloudIDEaaS";
  clientKey = "9A7a7699-fF88-41f7-bAB6-40D3878B0957";
  public accessTokenInfo: AccessTokenInfo;
  // private navController : NavControllerBase;
  private roles: List<string>;

  constructor(public api: Api, /* private app: App, */ private componentFactoryResolver: ComponentFactoryResolver, private injector: Injector) {
  }

  isLoggedIn() {
    return this.api.accessToken != null;
  }

  async login(accountInfo: { userName: string, password: string }) {

    let authorizationBasic: string;
    let observable: Observable<object>;
    let pageLoader = this.injector.get(IndexPageLoader);

    pageLoader.setAuthenticateStart();

    authorizationBasic = btoa(clientInfo.Issuer + ':' + clientInfo.Key);
    observable = await this.api.authenticate('user/login', accountInfo, authorizationBasic);

    observable.subscribe((res: any) => {
      if (res.access_token !== undefined || res.error !== undefined) {
        this.loggedIn(res);
      }
      else {
        throw new Error("Invalid user token");
      }
    }, err => {
      throw err;
    });

    window.dispatchEvent(new CustomEvent('user:login'));

    return observable;
  }

  verifyEmail(parmId: string) {
    let seq = this.api.get('verifyemail', parmId);

    seq.subscribe((res: any) => {

      if (res.status == 'success') {
        this.loggedIn(res);
      }
    }, err => {
      throw new Error(err);
    });

    return seq;
  }

  register(accountInfo: any) {
    let seq = this.api.post('register', accountInfo);

    seq.subscribe((res: any) => {

      if (res.status == 'success') {
        this.loggedIn(res);
      }
    }, err => {
      throw new Error(err);
    });

    window.dispatchEvent(new CustomEvent('user:register'));

    return seq;
  }

  logout(): Observable<boolean> {

    let observable = this.api.get<boolean>("logout");
    this.api.accessToken = null;

    window.dispatchEvent(new CustomEvent('user:logout'));

    return observable;
  }

  loggedIn(resp: any) {

    let rolesString: string = resp.roles;

    this.accessTokenInfo = new AccessTokenInfo(resp);
    this.api.accessToken = this.accessTokenInfo.accessToken;
    this.roles = new List<string>(rolesString.split(","));
  }

  getUser(id: string) {
    let observable = this.api.get<User>(`user/${id}`);

    return observable.pipe(map(u => new User(<any>u)));
  }

  createUser(user: User) {
    let observable = this.api.post('user', user);

    return observable;
  }

  updateUser(user: User) {
    let observable = this.api.put('user', user);

    return observable;
  }

  deleteUser(id: string): any {
    let observable = this.api.delete('user?id=' + id);

    return observable;
  }
}
