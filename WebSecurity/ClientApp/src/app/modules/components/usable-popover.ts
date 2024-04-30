import * as $ from 'jquery';
//declare const require: any;
const Resizable = require("resizable");
const jQueryUtils = require("../utils/jQueryUtils.js");
const queryString = require('query-string');

export abstract class UsablePopover {

    resizable: any;

    ionViewDidLoad() {

        let popover = $(".popover-content");
        let content = popover.find(".scroll-content");
        let debugging = false;
        const queryParms = queryString.parse(location.search);

        popover.css("overflow", "hidden");

        if (queryParms.debugging == true) {

            // debugging

            popover.css({"border-color": "red",
            "border-width":"1px",
            "border-style":"solid"});

            content.css({"border-color": "black",
            "border-width":"1px",
            "border-style":"solid"});
        }

        this.resizable = new Resizable(popover[0], {
            within: 'document',
            handles: 's, se, e',
            threshold: 10,
            draggable: true
        });

        this.resizable.on('resize', function(){
          (<any> content).resizeRelativeTo(popover);
        });
      }
}
