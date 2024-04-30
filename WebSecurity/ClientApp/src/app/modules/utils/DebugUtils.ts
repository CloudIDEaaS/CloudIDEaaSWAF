import { isDevMode } from '@angular/core';

export class DebugUtils {

  public static noop() {
    console.warn("Please remove noops in production build");
  }

  public static break() {
    if (isDevMode()) {
      debugger;
    }
    else {
      throw "Invalid operation";
    }
  }
}
