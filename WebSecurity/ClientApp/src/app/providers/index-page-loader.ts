import { EventEmitter, HostListener, Injectable, NgZone, Output } from "@angular/core";
import { NgxLoggerLevel } from "ngx-logger";
import { isDevMode } from '@angular/core';
import { ILogEvent } from "../modules/utils/logEvent";
import { environment } from "../../environments/environment";
import clientInfo from "../../environments/api.json";
import * as CryptoJS from 'crypto-js';
import { TOTP } from "totp-generator";
import base32Encode from 'base32-encode'
import { Utils } from "../modules/utils/Utils";
import { PingResponse } from "../models/pingresponse";

export class LoadParms {
  private α: any;
  private β: any;
  private ξ: any;
  private τ: any;
  private ς: any;
  private _submitStart: any;
  private _authenticateStart: any;
  private _loginPageLoadStart: any;
  private _userNameEnteredEnd: any;
  private _passwordEnteredEnd: any;
  private _captchaResolvedEnd: any;
  private _domEvents: Array<{ category: string, target: string, active: string, isActive: boolean }>;

  public get clientCert(): string {
    return this.β;
  }

  public get publicKey(): string {
    return this.α;
  }

  public get navigateStart(): string {
    return this.τ;
  }

  public get submitStart(): string {
    return this._submitStart;
  }

  public get authenticateStart(): string {
    return this._authenticateStart;
  }

  public get loginPageLoadStart(): string {
    return this._loginPageLoadStart;
  }

  public get userNameEnteredEnd(): string {
    return this._userNameEnteredEnd;
  }

  public get passwordEnteredEnd(): string {
    return this._passwordEnteredEnd;
  }

  public get captchaResolvedEnd(): string {
    return this._captchaResolvedEnd;
  }

  public get domEvents(): Array<{ category: string, target: string, active: string, isActive: boolean }> {
    return this._domEvents;
  }

  public setLoginPageLoadStart() {
    this._loginPageLoadStart = Date.now();
  }

  public setSubmitStart() {
    this._submitStart = Date.now();
  }

  public setAuthenticateStart() {
    this._authenticateStart = Date.now();
  }

  public setUserNameEnteredEnd() {
    this._userNameEnteredEnd = Date.now();
  }

  public setPaswordEnteredEnd() {
    this._passwordEnteredEnd = Date.now();
  }

  public setCaptchaResolvedEnd() {
    this._captchaResolvedEnd = Date.now();
  }

  public get usageError(): string {
    return this.ξ;
  }

  public set usageError(error: string) {
    this.ξ = error;
    window["ξ"] = error;
  }

  public get userInfo(): Promise<any> {

    let promise = new Promise<any>(async (resolve) => {

      let position: GeolocationPosition
      let positionError: GeolocationPositionError
      let info;

      navigator.geolocation.getCurrentPosition((p) => {
        position = p;
        info = {
          timeOpened: new Date(),
          timezone: (new Date()).getTimezoneOffset() / 60,
          get pageon() { return window.location.pathname },
          get referrer() { return document.referrer },
          get previousSites() { return history.length },
          get browserName() { return navigator.appName },
          get browserEngine() { return navigator.product },
          get browserVersion1a() { return navigator.appVersion },
          get browserVersion1b() { return navigator.userAgent },
          get browserLanguage() { return navigator.language },
          get browserOnline() { return navigator.onLine },
          get browserPlatform() { return navigator.platform },
          get javaEnabled() { return navigator.javaEnabled() },
          get dataCookiesEnabled() { return navigator.cookieEnabled },
          get dataCookies() { return document.cookie },
          get dataStorage() { return localStorage },
          get sizeScreenW() { return screen.width },
          get sizeScreenH() { return screen.height },
          get sizeInW() { return innerWidth },
          get sizeInH() { return innerHeight },
          get sizeAvailW() { return screen.availWidth },
          get sizeAvailH() { return screen.availHeight },
          get scrColorDepth() { return screen.colorDepth },
          get scrPixelDepth() { return screen.pixelDepth },
          get latitude() { return position?.coords.latitude },
          get longitude() { return position?.coords.longitude },
          get accuracy() { return position?.coords.accuracy },
          get altitude() { return position?.coords.altitude },
          get altitudeAccuracy() { return position?.coords.altitudeAccuracy },
          get heading() { return position?.coords.heading },
          get speed() { return position?.coords.speed },
          get timestamp() { return position?.timestamp },
        };
        resolve(info);
      }, (e) => {
        positionError = e;
        info = {
          timeOpened: new Date(),
          timezone: (new Date()).getTimezoneOffset() / 60,
          get pageon() { return window.location.pathname },
          get referrer() { return document.referrer },
          get previousSites() { return history.length },
          get browserName() { return navigator.appName },
          get browserEngine() { return navigator.product },
          get browserVersion1a() { return navigator.appVersion },
          get browserVersion1b() { return navigator.userAgent },
          get browserLanguage() { return navigator.language },
          get browserOnline() { return navigator.onLine },
          get browserPlatform() { return navigator.platform },
          get javaEnabled() { return navigator.javaEnabled() },
          get dataCookiesEnabled() { return navigator.cookieEnabled },
          get dataCookies() { return document.cookie },
          get dataStorage() { return localStorage },
          get sizeScreenW() { return screen.width },
          get sizeScreenH() { return screen.height },
          get sizeInW() { return innerWidth },
          get sizeInH() { return innerHeight },
          get sizeAvailW() { return screen.availWidth },
          get sizeAvailH() { return screen.availHeight },
          get scrColorDepth() { return screen.colorDepth },
          get scrPixelDepth() { return screen.pixelDepth },
          get positionError() { return positionError },
        };
        resolve(info);
      });
    });

    return promise;
  }

  public get captchaResponse(): string {
    return this.ς;
  }

  public set captchaResponse(value: string) {
    this.ς = value;
  }

  public appendDomEvent(event: { category: string, target: string, active: string, isActive: boolean, stamp: number }) {
    this._domEvents.push(event);
  }

  public get isReady(): boolean {
    this.α = window["α"];
    this.β = window["β"];
    this.ξ = window["ξ"];
    this.τ = window["τ"];
    return this.α && this.β && this.τ || this.ξ;
  }

  constructor() {
    this.α = window["α"];
    this.β = window["β"];
    this.ξ = window["ξ"];
    this.τ = window["τ"];
    this._domEvents = new Array<{ category: string, target: string, active: string, isActive: boolean }>();
  }
}

export class IndexPageLoader {

  GetLogLevel(): NgxLoggerLevel {

    let url: string = document.location.toString();
    let params = (new URL(url)).searchParams;
    let log = params.get("log");

    if (!log) {
      if (isDevMode()) {
        return NgxLoggerLevel.TRACE;
      }
      else {
        return NgxLoggerLevel.OFF;
      }
    }

    switch (log.toUpperCase()) {
      case "TRACE":
        return NgxLoggerLevel.TRACE;
      case "DEBUG":
        return NgxLoggerLevel.DEBUG;
      case "INFO":
        return NgxLoggerLevel.INFO;
      case "LOG":
        return NgxLoggerLevel.LOG;
      case "WARN":
        return NgxLoggerLevel.WARN;
      case "ERROR":
        return NgxLoggerLevel.ERROR;
      case "FATAL":
        return NgxLoggerLevel.FATAL;
      case "OFF":
        return NgxLoggerLevel.OFF;
    }
  }

  @Output() onReady = new EventEmitter<LoadParms>();
  loadParms: LoadParms;

  public set captchaResponse(value: string) {
    this.loadParms.captchaResponse = value;
  }

  async checkReadiness() {

    if (this.loadParms.isReady && !window["loadParmsReady"]) {

      window["loadParmsReady"] = true;

      this.onReady.emit(this.loadParms);
      this.fetchServerData();
      this.captureDomEvents();

      return;
    }

    await setTimeout(() => {
      this.checkReadiness();
    }, 100);
  }

  // { #if DEBUG }
  async handleBundle(url: string, headers: Headers, body: string) {

    let bundle = window["π"];

    console.log();
    try {

      let α;
      let β;
      let τ;

      console.debug("Bundle being handled");

      [α, β, τ] = await bundle.handle([url, headers, body]);

      if (β === undefined) {
        console.error(α);
        window["ξ"] = α;
      }
      else {
        window["α"] = α;
        window["β"] = β;
        window["τ"] = τ;
      }
    }
    catch (e) {
      window["ξ"] = e;
      console.error(e);
    }
  }
  // { #endif }

  fetchServerData() {

    let baseUrl: string = environment.baseServiceUrl;
    let script = new Error().stack.match(/([^ \n\(])*([a-z]*:\/\/\/?)*?[a-z0-9\/\\]*\.js/ig)[0]
    let credentials = CryptoJS.MD5(script).toString();
    let password = CryptoJS.AES.encrypt(script, credentials).toString();
    let servicesUrl: string = baseUrl + "/cloudideaas/hydra/services/api";
    let currentUrl = window.location.href;
    let url;
    let buffer = Utils.str2ab(clientInfo.Key);
    let encoded = base32Encode(buffer, "RFC4648");
    let period = 15;
    let otp: string;
    let token: string;

    // { #if DEBUG }
    period = 120;
    // { #endif }

    ({ otp } = TOTP.generate(encoded, { algorithm: "SHA-512", period: period, timestamp: Date.now(), digits: 10 }));
    token = btoa(`${credentials}:${password}`);

    url = servicesUrl + "/user/ping?identifier=" + otp;

    console.debug("Bundle imported");

    fetch(url, {
      method: 'GET',
      headers: {
        "X-API-Key": clientInfo.Key,
        "X-API-Host": baseUrl.replace(/https?:\/\//, ""),
        "Authorization": `Basic ${token}`
      },
    }).then(async (response) => {

      let headers = response.headers;
      let status = response.status;
      let body = await response.json();
      let pingResponse = <PingResponse>body;
      let checksum: string;
      let server: string;

      if (status != 200) {
        this.loadParms.usageError = response.statusText;
        return;
      }

      if (headers.has("X-API-Checksum")) {
        checksum = headers.get("X-API-Checksum");
      }
      else {
        throw "No Checksum";
      }

      if (headers.has("Server")) {
        server = headers.get("Server");
      }
      else {
        throw "No server";
      }

      if (server != environment.serverName) {
        throw "Invalid server";
      }

      if (Utils.checksum(pingResponse.AnonymousIdentifier) != parseInt(checksum)) {
        throw "Invalid checksum";
      }

      // { #if DEBUG }

      if (currentUrl.match("https?://localhost[/:]")) {
        this.handleBundle(url, headers, body);
      }

      // { #endif }
    }).catch((e) => {
      this.loadParms.usageError = e;
    });
  }

  captureDomEvents() {

    let domEvents = {
      UIEvent: "abort DOMActivate error load resize scroll select unload",
      ProgressEvent: "abort error load loadend loadstart progress progress timeout",
      Event: "abort afterprint beforeprint cached canplay canplaythrough change chargingchange chargingtimechange checking close dischargingtimechange DOMContentLoaded downloading durationchange emptied ended ended error error error error fullscreenchange fullscreenerror input invalid languagechange levelchange loadeddata loadedmetadata noupdate obsolete offline online open open orientationchange pause pointerlockchange pointerlockerror play playing ratechange readystatechange reset seeked seeking stalled submit success suspend timeupdate updateready visibilitychange volumechange waiting",
      AnimationEvent: "animationend animationiteration animationstart",
      AudioProcessingEvent: "audioprocess",
      BeforeUnloadEvent: "beforeunload",
      TimeEvent: "beginEvent endEvent repeatEvent",
      OtherEvent: "blocked complete upgradeneeded versionchange",
      FocusEvent: "blur DOMFocusIn  Unimplemented DOMFocusOut  Unimplemented focus focusin focusout",
      MouseEvent: "click contextmenu dblclick mousedown mouseenter mouseleave mousemove mouseout mouseover mouseup show",
      SensorEvent: "compassneedscalibration Unimplemented userproximity",
      OfflineAudioCompletionEvent: "complete",
      CompositionEvent: "compositionend compositionstart compositionupdate",
      ClipboardEvent: "copy cut paste",
      DeviceLightEvent: "devicelight",
      DeviceMotionEvent: "devicemotion",
      DeviceOrientationEvent: "deviceorientation",
      DeviceProximityEvent: "deviceproximity",
      MutationNameEvent: "DOMAttributeNameChanged DOMElementNameChanged",
      MutationEvent: "DOMAttrModified DOMCharacterDataModified DOMNodeInserted DOMNodeInsertedIntoDocument DOMNodeRemoved DOMNodeRemovedFromDocument DOMSubtreeModified",
      DragEvent: "drag dragend dragenter dragleave dragover dragstart drop",
      GamepadEvent: "gamepadconnected gamepaddisconnected",
      HashChangeEvent: "hashchange",
      KeyboardEvent: "keydown keypress keyup",
      MessageEvent: "message message message message",
      PageTransitionEvent: "pagehide pageshow",
      PopStateEvent: "popstate",
      StorageEvent: "storage",
      SVGEvent: "SVGAbort SVGError SVGLoad SVGResize SVGScroll SVGUnload",
      SVGZoomEvent: "SVGZoom",
      TouchEvent: "touchcancel touchend touchenter touchleave touchmove touchstart",
      TransitionEvent: "transitionend",
      WheelEvent: "wheel"
    }

    let recentlyLoggedDOMEventTypes = {};

    for (var domEvent in domEvents) {

      let domEventTypes = domEvents[domEvent].split(' ');

      domEventTypes.filter((domEventType) => {

        let domEventCategory = domEvent + ' ' + domEventType;

        document.addEventListener(domEventType, (e: Event) => {

          if (recentlyLoggedDOMEventTypes[domEventCategory]) {
            return;
          }

          recentlyLoggedDOMEventTypes[domEventCategory] = true;

          setTimeout(() => {
            recentlyLoggedDOMEventTypes[domEventCategory] = false
          }, 5000);

          let isActive = e.target == document.activeElement;
          let activeElement = <HTMLElement>document.activeElement;
          let target = <HTMLElement>e.target;
          let eventInfo: {
            type: string,
            text: string,
          };
          let activeElementInfo = {
            element: activeElement.toString(),
            tagName: activeElement.tagName,
            id: activeElement.id,
            className: activeElement.className,
            name: activeElement.attributes["name"]
          };
          let targetInfo = {
            element: target.toString(),
            tagName: activeElement.tagName,
            id: activeElement.id,
            className: activeElement.className,
            name: activeElement.attributes["name"]
          };

          eventInfo = { type: "unknown", text: "unknown" };

          if (e instanceof MouseEvent) {

            let mouseEvent = <MouseEvent>e;

            eventInfo.type = MouseEvent.name;
            eventInfo.text = mouseEvent.toString();
            eventInfo["x"] = mouseEvent.x;
            eventInfo["y"] = mouseEvent.y;
            eventInfo["clientX"] = mouseEvent.clientX;
            eventInfo["clientX"] = mouseEvent.clientY;
            eventInfo["offsetX"] = mouseEvent.offsetX;
            eventInfo["offsetY"] = mouseEvent.offsetY;
            eventInfo["movementX"] = mouseEvent.movementX;
            eventInfo["movementY"] = mouseEvent.movementY;
          }
          else if (e instanceof KeyboardEvent) {

            let keyboardEvent = <KeyboardEvent>e;

            eventInfo.type = KeyboardEvent.name;
            eventInfo.text = keyboardEvent.toString();
            eventInfo["key"] = keyboardEvent.key;
            eventInfo["code"] = keyboardEvent.code;
            eventInfo["altKey"] = keyboardEvent.altKey;
            eventInfo["shiftKey"] = keyboardEvent.shiftKey;
            eventInfo["ctrlKey"] = keyboardEvent.ctrlKey;
          }
          else if (e instanceof FocusEvent) {

            let focusEvent = <FocusEvent>e;

            eventInfo.type = KeyboardEvent.name;
            eventInfo.text = focusEvent.toString();
          }
          else if (e instanceof InputEvent) {

            let inputEvent = <InputEvent>e;

            eventInfo.type = KeyboardEvent.name;
            eventInfo.text = inputEvent.toString();
            eventInfo["data"] = inputEvent.data;
            eventInfo["inputType"] = inputEvent.inputType;
          }
          else if (e instanceof CompositionEvent) {

            let compositionEvent = <CompositionEvent>e;

            eventInfo.type = KeyboardEvent.name;
            eventInfo.text = compositionEvent.toString();
            eventInfo["data"] = compositionEvent.data;
          }
          else if (e instanceof UIEvent) {

            let uiEvent = <UIEvent>e;

            eventInfo.text = uiEvent.toString();
            eventInfo.type = UIEvent.name;
            eventInfo["eventType"] = uiEvent.type;
          }
          else if (e instanceof MutationEvent) {

            let mutationEvent = <MutationEvent>e;

            eventInfo.text = mutationEvent.toString();
            eventInfo.type = MutationEvent.name;
            eventInfo["attrName"] = mutationEvent.attrName;
            eventInfo["newValue"] = mutationEvent.newValue;
            eventInfo["prevValue"] = mutationEvent.prevValue;
            eventInfo["relatedNodeName"] = mutationEvent.relatedNode?.nodeName;
            eventInfo["relatedNodeType"] = mutationEvent.relatedNode?.nodeType;
            eventInfo["mutationType"] = mutationEvent.type;
          }
          else {

            let event = <Event>e;

            eventInfo.type = event.constructor.toString();
            eventInfo.text = event.toString();
            eventInfo["eventType"] = event.type;
          }

          if (isActive) {

            this.loadParms.appendDomEvent({
              category: domEventCategory,
              target: JSON.stringify(targetInfo),
              active: JSON.stringify(activeElementInfo),
              isActive: true,
              stamp: Date.now()
            });
          }
          else {

            this.loadParms.appendDomEvent({
              category: domEventCategory,
              target: e.target.toString(),
              active: document.activeElement.toString(),
              isActive: false,
              stamp: Date.now()
            });
          }
        }, true);
      });
    }
  }

  public static getLogQueue(): Array<ILogEvent> {
    return <Array<ILogEvent>>window["logQueue"];
  }

  constructor() {

    if (!IndexPageLoader.getLogQueue()) {
      window.addEventListener("log", IndexPageLoader.onLog);
      window["logQueue"] = new Array<ILogEvent>();
    }

    this.loadParms = new LoadParms();
    this.checkReadiness();
  }

  public get<T>(property: keyof LoadParms) {
    return <T>this.loadParms[property];
  }

  public setLoginPageLoadStart() {
    this.loadParms.setLoginPageLoadStart();
  }

  public setSubmitStart() {
    this.loadParms.setSubmitStart()
  }

  public setAuthenticateStart() {
    this.loadParms.setAuthenticateStart();
  }

  public setUserNameEnteredEnd() {
    this.loadParms.setUserNameEnteredEnd
  }

  public setPaswordEnteredEnd() {
    this.loadParms.setPaswordEnteredEnd();
  }

  public setCaptchaResolvedEnd() {
    this.loadParms.setCaptchaResolvedEnd();
  }

  public static onLog(event): void {
    let detail = event.detail;
    let logQueue = IndexPageLoader.getLogQueue();
    logQueue.push(detail);
  }
}