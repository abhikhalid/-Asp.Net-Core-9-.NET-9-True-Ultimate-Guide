namespace Section_08_Razor_Views.Models
{
    public class Person
    {
        public string? Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender PersonGender { get; set; }
    }

    public enum Gender
    {
        Male, Female, Other
    }
}
