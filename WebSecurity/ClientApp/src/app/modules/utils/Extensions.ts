import { isDevMode } from '@angular/core';

declare global {

  export interface Object {
    isOneOf(...values): boolean;
  }

  export interface Number {
    isOneOf(...values): boolean;
  }

  export interface Uint8Array {
    indexOfText(text: string);
    toText();
    toBase64();
  }

  export interface ArrayBuffer {
    toBase64();
  }
}

export interface Console {
  writeLog(message: string);
  writeDebug(message: string);
  writeError(message: string);
  writeWaring(message: string);
}

let writeDebug = function (message) {
  if (isDevMode()) {
    console.log(message);
  }
  else {
    console.debug(message);
  }
}

let writeError = function (message) {
  console.error(message);
}

let writeWarning = function (message) {
  console.warn(message);
}

let toBase64Array = function () {

  var binary = '';
  var bytes = this;
  var len = bytes.byteLength;

  for (var i = 0; i < len; i++) {
    binary += String.fromCharCode(bytes[i]);
  }

  return window.btoa(binary);
}


let toBase64ArrayBuffer = function () {

  var binary = '';
  var bytes = new Uint8Array(this);
  var len = bytes.byteLength;

  for (var i = 0; i < len; i++) {
    binary += String.fromCharCode(bytes[i]);
  }

  return window.btoa(binary);
}


let toText = function () {

  let index: number = 0;
  let accum: string = "";

  index = this.forEach((b) => {
    let ch = String.fromCharCode(b);
    accum += ch;
  });

  return accum;
}

let indexOfText = function (text: string) {

  let index: number = 0;
  let accum: string = "";

  index = this.findIndex((b, i, a) => {

    let ch = String.fromCharCode(b);

    if (ch == text[accum.length]) {
      accum += ch;

      if (accum == text) {
        return true;
      }
    }
    else {
      accum = "";
    }

    return false;
  });

  if (index == -1) {
    return index;
  }
  else {
    return index + 1;
  }
}

let isOneOf = function (...values): boolean {

  let equals: boolean;

  this.segments = [];

  values.forEach(v => {
    if (this == v) {
      equals = true;
    }
  });

  return equals;
}

// Object.prototype.isOneOf = isOneOf;
// Number.prototype.isOneOf = isOneOf;
Uint8Array.prototype.indexOfText = indexOfText;
Uint8Array.prototype.toText = toText;
Uint8Array.prototype.toBase64 = toBase64Array;
ArrayBuffer.prototype.toBase64 = toBase64ArrayBuffer;

export { };
