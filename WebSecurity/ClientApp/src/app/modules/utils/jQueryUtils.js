import * as jQuery from 'jquery';

(function ($) {
 
    $.fn["resizeRelativeTo"] = function(parentElement) {

        let self = this;
        let parentWidth = parentElement.width();
        let parentHeight = parentElement.height();
        let width = self.width();
        let height = self.height();
        let paddingLeftCss = self.css("padding-left");
        let paddingTopCss = self.css("padding-top");
        let paddingRightCss = self.css("padding-right");
        let paddingBottomCss = self.css("padding-bottom");
        let paddingLeft = 0;
        let paddingTop = 0;
        let paddingRight = 0;
        let paddingBottom = 0;
        let marginLeftCss = self.css("margin-left");
        let marginTopCss = self.css("margin-top");
        let marginRightCss = self.css("margin-right");
        let marginBottomCss = self.css("margin-bottom");
        let marginLeft = 0;
        let marginTop = 0;
        let marginRight = 0;
        let marginBottom = 0;
        let pattern = /(\d+?)px/;

        if (pattern.test(paddingLeftCss))
        {
            paddingLeft = parseInt(pattern.exec(paddingLeftCss));
        }

        if (pattern.test(paddingTopCss))
        {
            paddingTop = parseInt(pattern.exec(paddingTopCss));
        }

        if (pattern.test(paddingRightCss))
        {
            paddingRight = parseInt(pattern.exec(paddingRightCss));
        }

        if (pattern.test(paddingBottomCss))
        {
            paddingBottom = parseInt(pattern.exec(paddingBottomCss));
        }

        if (pattern.test(marginLeftCss))
        {
            marginLeft = parseInt(pattern.exec(marginLeftCss));
        }

        if (pattern.test(marginTopCss))
        {
            marginTop = parseInt(pattern.exec(marginTopCss));
        }

        if (pattern.test(marginRightCss))
        {
            marginRight = parseInt(pattern.exec(marginRightCss));
        }

        if (pattern.test(marginBottomCss))
        {
            marginBottom = parseInt(pattern.exec(marginBottomCss));
        }
  
        self.css("width", parentWidth - (paddingRight + paddingLeft + marginRight + marginLeft));
        self.css("height", parentHeight - (paddingTop + paddingBottom + marginTop + marginBottom));
      }; 
 
}(jQuery));