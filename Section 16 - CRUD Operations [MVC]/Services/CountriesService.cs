using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();
            
            if (initialize)
            {
                _countries.AddRange(new List<Country>()
                {
                    new Country() { CountryID  = Guid.Parse("554C00E8-1114-4F3E-99C8-8D895DACE4E1"), CountryName = "USA"},
                    new Country() { CountryID  = Guid.Parse("9C0CAB52-4CA3-4F38-A2B8-1255D8BFD232"), CountryName = "Canada"},
                    new Country() { CountryID  = Guid.Parse("D7E9512D-2845-4728-8E54-1AA478141FF4"), CountryName = "Australia"},
                    new Country() { CountryID  = Guid.Parse("526922C9-4B81-42D6-A417-8A2D2F5F6482"), CountryName = "UK"},
                    new Country() { CountryID  = Guid.Parse("9D7ECC1B-F7AD-4754-9951-F303D9DE83A7"), CountryName = "Bangladesh"},
                });
            }
        }

       
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            //Validation: countryAddRequest parameter can't be null
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //Valiadation: countryName can't be null
            if(countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //Validation: CountryName can't be duplicate
            if(_countries.Where(country => country.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            //generate CountryID
            country.CountryID = Guid.NewGuid();

            //Add country object into _countries
            _countries.Add(country);

            return country.ToCountryResponse();
        }


       
        public List<CountryResponse> GetAllCountries()
        {
           return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null) return null;

            Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryID == countryID);

            if (country_response_from_list == null) return null;

            return country_response_from_list.ToCountryResponse();
        }
    }
}