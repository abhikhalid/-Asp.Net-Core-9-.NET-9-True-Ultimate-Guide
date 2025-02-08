using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Domain Model for Country
    /// We will not explose this domain model to the presentation layer (Controller/View)
    /// </summary>
    public class Country
    {
        //mandatory to add a primary key
        [Key]
        public Guid CountryID { get; set; }

        public string? CountryName { get; set; }

        public virtual ICollection<Person>? Persons { get; set; }
    }
}
