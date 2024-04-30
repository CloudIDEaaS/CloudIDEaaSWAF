using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.ValidationAttributes
{
    public enum ListType
    {
        Multiline,
        CommaDelimited
    }

    public class TextListAttribute : ValidationAttribute
    {
        public bool CanBeAbsolute { get; set; }
        public ListType ListType { get; }

        public TextListAttribute(ListType listType)
        {
            this.ListType = listType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = (string)value;
            ValidationResult validationResult = null;

            if (!list.IsNullOrEmpty())
            {
                if (this.ListType == ListType.CommaDelimited)
                {
                    if (list.GetLineCount() > 1)
                    {
                        validationResult = new ValidationResult(string.Format("List should be comma delimited.  Detected multiple lines"));

                        return validationResult;
                    }
                }
                else if (this.ListType == ListType.Multiline)
                {
                    if (list.Where(c => c == ',').Count() >= 3 && list.GetLineCount() > 0)
                    {
                        validationResult = new ValidationResult(string.Format("List should be multi-line.  Detected multiple commas"));

                        return validationResult;
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}