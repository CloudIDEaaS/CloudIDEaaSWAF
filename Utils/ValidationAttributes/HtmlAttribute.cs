using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Utils.ValidationAttributes
{
    public class HtmlAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var document = new HtmlDocument();
            ValidationResult validationResult = null;
            List<HtmlParseError> parseErrors;
            var html = (string)value;

            if (html.IsNullOrEmpty())
            {
                validationResult = new ValidationResult(string.Format("Parse errors exist. Empty or null html"));

                return validationResult;
            }

            document.LoadHtml((string)value);

            parseErrors = document.ParseErrors.ToList();

            if (parseErrors.Count > 0)
            {
                var errorList = parseErrors.Select(p => string.Format("{0} [{1},{2}]", p.Reason, p.Line, p.LinePosition)).ToMultiLineList();

                validationResult = new ValidationResult(string.Format("Parse errors exist, count: {0}, errors: \r\n{1}", parseErrors.Count, errorList));

                return validationResult;
            }

            return ValidationResult.Success;
        }
    }
}
