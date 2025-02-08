using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    ///  Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest">Countrry object to add</param>
        /// <returns>Returns the country object after adding it (including newly generated country id)</returns>
      
        //the meaning of Task is that it is awatable. That means while calling this method, the caller can use the 'await' keyword. 
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest); 


        /// <summary>
        /// Returns all countries from the list
        /// </summary>
        /// <returns>All countries from the list as List of CountryResponse</returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Returns a country object based on the given country id
        /// </summary>
        /// <param name="countryID">countryID (guid) to search</param>
        /// <returns>Matching country as CountryResponse object</returns>
        Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);
    }
}
