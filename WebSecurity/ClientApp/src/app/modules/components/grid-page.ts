import { Component, NgZone, OnInit, OnDestroy } from '@angular/core';
//declare const require: any;
const queryString = require('query-string');

export abstract class GridPage {

  static renderLinkElement(pageName: string, methodName: string, linkText: string, params: any) {
    // tslint:disable-next-line:max-line-length
    return '<a target="_blank" style="cursor: pointer;" onclick="my.GridPageBase.' + pageName + '.' + methodName + '(\'' + queryString.stringify(params.value) + '\')">' + linkText + '</a>';
  }

  bindClickMethod(pageName: string, methodName: string, method: Function) {

    const myWindow = window as any;

    myWindow.GridPageBase = myWindow.GridPageBase || {};
    myWindow.GridPageBase[pageName] = myWindow.GridPageBase[pageName] || {};
    myWindow.GridPageBase[pageName][methodName] = method.bind(this);
  }

  parseLinkParams(params: string): any {
    return queryString.parse(params);
  }
}
