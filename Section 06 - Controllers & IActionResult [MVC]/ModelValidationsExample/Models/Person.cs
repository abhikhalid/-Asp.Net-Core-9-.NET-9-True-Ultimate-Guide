using System.ComponentModel.DataAnnotations;

namespace ModelValidationsExample.Models
{
    //we are going to create Person registration form. So we will add all kinds of form validation.
    public class Person
    {
        [Required(ErrorMessage = "{0} can't be empty or null")] // 0 represents the name of the property.
        [Display(Name = "Person Name")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "{0} should be between {2} to {1} characters long")]
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }

        [Range(0,999.99, ErrorMessage = "{0} should be between {1} to {2}")]
        public double? Price{ get; set; }

        public override string ToString()
        {
            return $"Person object - Person name: {PersonName}, Email: {Email}, Phone: {Phone}, Password: {Password}, Confirm Password: {ConfirmPassword}, Price: {Price}";
        }
    }
}
