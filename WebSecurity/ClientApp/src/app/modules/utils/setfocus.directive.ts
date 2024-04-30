import { Directive, ElementRef, NgZone } from '@angular/core';
import * as $ from "jquery";

@Directive({
  selector: "[appSetFocus]"
})
export class SetFocusDirective {

  constructor(element: ElementRef, zone: NgZone) {

    let setFocusTimeout = () => {

      let input = $(element.nativeElement).find(":input:first");
      let inputElement = input.get(0);

      if (inputElement) {

        let focus;
        let focusElement;

        input.trigger("focus");

        (async () => {

          focus = $(":focus");
          focusElement = focus.get(0);

          if (inputElement !== focusElement) {
            setTimeout(setFocusTimeout, 100);
          }
        })();
      }
      else {
        setTimeout(setFocusTimeout, 100);
      }
    };

    setTimeout(setFocusTimeout, 100);
   }
}
