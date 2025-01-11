using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ModelValidationsExample.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace ModelValidationsExample.Models
{
    //we are going to create Person registration form. So we will add all kinds of form validation.
    public class Person : IValidatableObject
    {
        //[Required(ErrorMessage = "{0} can't be empty or null")] // 0 represents the name of the property.
        [Display(Name = "Person Name")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "{0} should be between {2} to {1} characters long")]
        [RegularExpression("^[A-Za-z .]*$", ErrorMessage = "{0} should contain only alphabet, space and dot (.)")]
        public string? PersonName { get; set; }

        [EmailAddress(ErrorMessage = "{0} should be a proper email address")]
        [Required(ErrorMessage = "{0} can't be blank")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "{0} should contain 10 digits")]
        //[ValidateNever]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "{0} can't be blank")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "{0} can't be blank")]
        [Compare("Password", ErrorMessage = "{0} and {1} does not match")]
        [Display(Name = "Re-enter Password")]
        public string? ConfirmPassword { get; set; }

        [Range(0, 999.99, ErrorMessage = "{0} should be between {1} to {2}")]
        public double? Price { get; set; }


        //[MinimumYearValidator(2005,ErrorMessage="Date of Birth should not be newer than Jan 01, {0}")]
        [MinimumYearValidator(2005)] //in case if the user doesn't supply the error mesasge, then system should genarate a default error message.
        [BindNever] //this attribute is used to exclude the property from model binding.
        public DateTime? DateOfBirth { get; set; }

        public int? Age { get; set; }

        public DateTime? FromDate { get; set; }

        [DateRangeValidator("FromDate", ErrorMessage = "From Date should be older than or equal to 'To date'")]
        public DateTime? ToDate { get; set; }


        public override string ToString()
        {
            return $"Person object - Person name: {PersonName}, Email: {Email}, Phone: {Phone}, Password: {Password}, Confirm Password: {ConfirmPassword}, Price: {Price}";
        }

        //this method is used to validate the model object. This method is called when there are no other validation errors.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateOfBirth.HasValue == false && Age.HasValue == false)
            {
                //in c#, yeild is used to return mutiole values from a function.
                yield return new ValidationResult("Either Date of Birth or Age is required", new[] { nameof(Age) });
            }
        }
    }
}
