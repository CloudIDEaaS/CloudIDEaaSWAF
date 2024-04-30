import { Directive, ElementRef, NgZone, OnDestroy, OnInit } from '@angular/core';
import * as $ from "jquery";
import { Dictionary, List } from "linq-collections";

@Directive({
  selector: "[appDefaultCommand]"
})
export class SetDefaultCommandDirective implements OnDestroy, OnInit {

  timeout: any;
  boxShadowSet = false;
  countToHook = 0;
  countToUnhook = 0;

  constructor(private element: ElementRef, zone: NgZone) {
  }

  ngOnInit(): void {

    let command = $(this.element.nativeElement);
    let form = command.closest("form");
    let inputSet = new Dictionary<string, { element: JQuery<HTMLElement>, on: boolean }>();
    let currentId = 0;
    let keyup = (e) => {

      if (e.key === "Enter") {

        console.debug("Enter KeyUp, clicking default");
        command.trigger("click");
      }
      else {
        console.debug("Not Enter Key");
      }
    };

    let setDefaultTimeout = () => {

      let formInputs = form.find(":input").each(function (i) {

        let element = $(this);
        let id: string;

        if (element.data("uniqueid")) {
          id = element.data("uniqueid");
        }
        else {
          id = (++currentId).toString();
          element.data("uniqueid", id);
        }

        if (!inputSet.containsKey(id)) {
          inputSet.set(id, { element: element, on: false });
        }
      });

      if (command.is(":visible") && !command.prop('disabled')) {

        let hookInputs = inputSet.getValues().where(e => !e.on);
        let count = hookInputs.count();

        if (!this.boxShadowSet) {

          console.debug("Command visible. Setting box shadow.");

          command.css("box-shadow", "5px 5px 5px rgba(0,0,0,0.6)");
          command.css("border-radius", "5px");

          this.boxShadowSet = true;
        }

        if (count !== this.countToHook) {
          console.debug(`Setting keyup on ${this.countToHook} inputs`);
        }

        this.countToHook = count;

        hookInputs.forEach(e => {
          e.element.on("keyup", keyup);
          e.on = true;
        });
      }
      else {

        let unhookInputs = inputSet.getValues().where(e => e.on);
        let count = unhookInputs.count();

        if (count !== this.countToUnhook) {
          console.debug(`Removing keyup on ${this.countToUnhook} inputs`);
        }

        unhookInputs.forEach(e => {
          e.element.off("keyup", keyup);
          e.on = false;
        });
      }

      clearTimeout(this.timeout);
      this.timeout = setTimeout(setDefaultTimeout, 100);
    };

    this.timeout = setTimeout(setDefaultTimeout, 10);
  }

  ngOnDestroy(): void {
    clearTimeout(this.timeout);
  }
}
