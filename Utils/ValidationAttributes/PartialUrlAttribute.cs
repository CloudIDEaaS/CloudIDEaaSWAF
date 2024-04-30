using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.ValidationAttributes
{
    public class PartialUrlAttribute : ValidationAttribute
    {
        public bool CanBeAbsolute { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var url = (string)value;
            Uri uriResult;
            var uriKind = UriKind.Relative;
            ValidationResult validationResult = null;

            if (url.IsNullOrEmpty())
            {
                validationResult = new ValidationResult(string.Format("Empty or null url"));

                return validationResult;
            }

            if (this.CanBeAbsolute)
            {
                uriKind = UriKind.RelativeOrAbsolute;
            }

            if (!Uri.TryCreate(url, uriKind, out uriResult))
            {
                validationResult = new ValidationResult(string.Format("Invalid partial url"));

                return validationResult;
            }

            return ValidationResult.Success;
        }
    }
}