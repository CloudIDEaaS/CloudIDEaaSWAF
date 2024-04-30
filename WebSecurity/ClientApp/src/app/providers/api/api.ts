import { HttpClient, HttpParams, HttpHeaders, HttpEvent, HttpRequest, HttpHandler } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from '../../../environments/environment';
import { share } from 'rxjs/operators';
import { detect } from "detect-browser"
import { get as getTrace } from 'stack-trace';
import { List } from "linq-javascript/build/src/Collections";
import { TraceInfo } from "./traceInfo";
import { JSEncrypt } from "jsencrypt"
import { IndexPageLoader } from "../index-page-loader";
import { compress } from "@zalari/string-compression-utils";
import * as CryptoJS from 'crypto-js';
import clientInfo from "../../../environments/api.json";

/**
 * Api is a generic REST Api handler. Set your API url first.
 */
@Injectable()
export class Api {
  baseUrl: string = environment.baseServiceUrl;
  apiVersion: string = environment.apiVersion;
  servicesUrl: string = this.baseUrl + "/cloudideaas/hydra/services/api";
  public accessToken: string | null;
  device: any;

  constructor(public http: HttpClient, private pageLoader: IndexPageLoader) {
    this.device = detect();
    console.debug();
  }

  private addToken(headers: HttpHeaders): HttpHeaders {

    if (this.accessToken) {
      headers = headers.append("Authorization", "Bearer " + this.accessToken);
    }

    return headers;
  }

  private async addClientCert(headers: HttpHeaders): Promise<HttpHeaders> {

    let publicKey = this.pageLoader.get<string>("publicKey");
    let clientCert = this.pageLoader.get<string>("clientCert");
    let captchaResponse = this.pageLoader.get<string>("captchaResponse");
    let navigationStart = this.pageLoader.get<string>("navigateStart");
    let userNameEnteredEnd = this.pageLoader.get<string>("userNameEnteredEnd");
    let passwordEnteredEnd = this.pageLoader.get<string>("passwordEnteredEnd");
    let captchaResolvedEnd = this.pageLoader.get<string>("captchaResolvedEnd");
    let submitStart = this.pageLoader.get<string>("submitStart");
    let authenticateStart = this.pageLoader.get<string>("authenticateStart");
    let userInfo = await this.pageLoader.get<string>("userInfo");
    let domEvents = this.pageLoader.get<Array<{ category: string, target: string, active: string, isActive: boolean }>>("domEvents");
    let privateKey = clientCert.substring(0, 242);
    let encrypt = new JSEncrypt({ default_key_size: (4096).toString(), log: true });
    let traceInfo = new List<Object>(getTrace()).take(3).select(t => this.getTraceInfo(t));
    let traceHeader = await compress(JSON.stringify(traceInfo.toArray()), "deflate-raw");
    let deviceHeader = await compress(JSON.stringify(this.device), "deflate-raw");
    let userActivity = {
      navigationStart: navigationStart,
      userNameEnteredEnd: userNameEnteredEnd,
      passwordEnteredEnd: passwordEnteredEnd,
      captchaResolvedEnd: captchaResolvedEnd,
      submitStart: submitStart,
      authenticateStart: authenticateStart,
      domEvents: domEvents
    }
    let userActivityBase64 = btoa(JSON.stringify(userActivity));
    let userInfoBase64 = btoa(JSON.stringify(userInfo));
    let environmentBase64 = btoa(JSON.stringify(environment));

    encrypt.setPublicKey(publicKey);
    privateKey = <string>encrypt.encrypt(privateKey);

    traceHeader = CryptoJS.AES.encrypt(traceHeader, publicKey).toString();
    deviceHeader = CryptoJS.AES.encrypt(deviceHeader, publicKey).toString();

    headers = headers.append("ClientCertificate", clientCert);

    if (captchaResponse) {
      headers = headers.append("CaptchaResponse", captchaResponse);
    }
    
    headers = headers.append("ClientKey", privateKey);
    headers = headers.append("Trace", traceHeader);
    headers = headers.append("Device", deviceHeader);
    headers = headers.append("UserInfo", userInfoBase64);
    headers = headers.append("UserActivity", userActivityBase64);
    headers = headers.append("Environment", environmentBase64);
    headers = headers.append("X-API-Key", clientInfo.Key);
    headers = headers.append("X-API-Host", this.baseUrl.replace(/https?:\/\//, ""));

    return headers;
  }

  getTraceInfo(trace: any): TraceInfo {
    return {
      typeName: trace.getTypeName(),
      functionName: trace.getFunctionName(),
      methodName: trace.getMethodName(),
      fileName: trace.getFileName(),
      lineNumber: trace.getLineNumber(),
      columnNumber: trace.getColumnNumber(),
    };
  }

  handle(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req);
  }

  getJsonHeaders(): HttpHeaders {
    let headers = new HttpHeaders()
      .append("Content-Type", "application/json")
      .append("Accept", "application/json; x-api-version=" + this.apiVersion)
      .append("x-api-version", this.apiVersion);

    headers = this.addToken(headers);

    return headers;
  }

  initializeHeaders(headers: any) {
    if (!!headers.lazyInit) {
      if (headers.lazyInit instanceof HttpHeaders) {
        headers.copyFrom(headers.lazyInit);
      }
      else {
        headers.lazyInit();
      }

      headers.lazyInit = null;

      if (!!headers.lazyUpdate) {
        headers.lazyUpdate.forEach((update) => headers.applyUpdate(update));
        headers.lazyUpdate = null;
      }
    }
  }

  getTextOrHtmlHeaders(): HttpHeaders {
    let headers = new HttpHeaders()
      .append("Content-Type", "application/json")
      .append("Accept", "text/html");

    headers = this.addToken(headers);

    return headers;
  }

  getUrl(endpoint: string): string {

    let url = this.servicesUrl + '/' + endpoint;

    console.debug(url);

    return url;
  }

  async authenticate(endpoint: string, accountInfo: { userName: string, password: string }, authorizationBasic: string) {

    let headers = await this.addClientCert(this.getJsonHeaders());

    headers = headers.append("Authorization", "Basic " + authorizationBasic);
    headers = headers.set("Content-Type", "application/json");

    return this.http.post(this.getUrl(endpoint), accountInfo, { headers: headers }).pipe(share());
  }

  getRaw(endpoint: string, params?: any, reqOpts?: any) {
    if (!reqOpts) {
      reqOpts = this.getJsonHeaders();
    }

    // Support easy query params for GET requests
    if (reqOpts.params) {
      reqOpts.params = new HttpParams();
      for (let k in params) {
        reqOpts.params = reqOpts.params.set(k, params[k]);
      }
    }

    return this.http.get(this.getUrl(endpoint), reqOpts).pipe(share());
  }

  get<T>(endpoint: string, params?: any, options?: {
    headers?: HttpHeaders | {
      [header: string]: string | string[];
    };
    observe?: 'body';
    params?: HttpParams | {
      [param: string]: string | string[];
    };
    reportProgress?: boolean;
    withCredentials?: boolean;
  }): Observable<T> {

    let requestHeaders = this.getJsonHeaders();

    options = options || { headers: requestHeaders };

    if (params) {
      options.params = new HttpParams();

      if (typeof params === "string") {
        endpoint = endpoint + "?" + params;
      }
      else {
        for (let k in params) {
          options.params = options.params.set(k, params[k]);
        }
      }
    }

    return this.http.get<T>(this.getUrl(endpoint), options).pipe(share());
  }

  post(endpoint: string, body: any) {

    let headers = this.getJsonHeaders();

    return this.http.post(this.getUrl(endpoint), body, { headers: headers }).pipe(share());
  }

  put(endpoint: string, body: any) {

    let headers = this.getJsonHeaders();

    return this.http.put(this.getUrl(endpoint), body, { headers: headers }).pipe(share());
  }

  delete(endpoint: string) {

    let headers = this.getJsonHeaders();

    return this.http.delete(this.getUrl(endpoint), { headers: headers }).pipe(share());
  }

  patch(endpoint: string, body: any) {

    let headers = this.getJsonHeaders();

    return this.http.patch(this.getUrl(endpoint), body, { headers: headers }).pipe(share());
  }
}
