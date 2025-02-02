namespace Entities
{
    /// <summary>
    /// Domain Model for Country
    /// We will not explose this domain model to the presentation layer (Controller/View)
    /// </summary>
    public class Country
    {
        public Guid CountryID { get; set; }

        public string? CountryName { get; set; }
    }
}
