using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    ///  Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesAdderService
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest">Countrry object to add</param>
        /// <returns>Returns the country object after adding it (including newly generated country id)</returns>
      
        //the meaning of Task is that it is awatable. That means while calling this method, the caller can use the 'await' keyword. 
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
    }
}
