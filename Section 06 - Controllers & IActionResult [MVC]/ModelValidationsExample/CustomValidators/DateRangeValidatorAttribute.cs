using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ModelValidationsExample.CustomValidators
{
    public class DateRangeValidatorAttribute : ValidationAttribute
    {
        public string OtherPropertyName { get; set; } // ToDate

        public DateRangeValidatorAttribute(string otherPropertyName)
        {
            OtherPropertyName = otherPropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value != null)
            {
                //to_date

                DateTime to_date = Convert.ToDateTime(value);

                // validationContext.ObjectType => Person Class (Model Class)
                PropertyInfo? otherProperty = validationContext.ObjectType.GetProperty(OtherPropertyName);

                //from_date

                //validationContext.ObjectInstance => Person object (Current Object)
                if (otherProperty != null)
                {
                    DateTime from_date = Convert.ToDateTime(otherProperty.GetValue(validationContext.ObjectInstance));
                    if (from_date > to_date)
                    {
                        return new ValidationResult(ErrorMessage ?? "From Date should be older than or equal to 'To date'");
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }
                }
                else
                {
                    return null; // null means no validation result
                }
            }
            return null;
        }
    }
}
