using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;
using ServiceContracts.DTO;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            //we don't want to create mock data for test.
            _countriesService = new CountriesService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
        }

        #region AddCountry
        //There are 4 requirement.

        // When CountryAddRequest is null, it should throw ArgumentNullException.
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
        }

        // When the CountryName is null, it should throw ArgumentException

        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
        }

        // When the CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            
            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            });
        }

        // When you supply proper country name, it should insert (add) the country to the existing list of countries. 
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Japan"
            };

            //Act
            CountryResponse response = await _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountries = await _countriesService.GetAllCountries();


            //Assert
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response,countries_from_GetAllCountries);
        }
        #endregion

        #region GetAllCountries
        [Fact]
        //The list of countries should be empty by default (before adding any countries)
        public async Task GetAllCountries_EmptyList() 
        {
            //Act
            List<CountryResponse> actual_country_response_list = await _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actual_country_response_list); // if the collection is empty, this test case is first
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
            {
                new CountryAddRequest(){CountryName = "USA"},
                new CountryAddRequest(){CountryName = "UK"}
            };

            //Act
            List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();

            foreach(CountryAddRequest country_request in country_request_list)
            {
                countries_list_from_add_country.Add(await _countriesService.AddCountry(country_request));
            }

            List<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();

            //read each element from countries_list_from_add_country
            foreach(CountryResponse expected_country in countries_list_from_add_country)
            {
                Assert.Contains(expected_country,actualCountryResponseList);
            }
        }

        #endregion

        #region GetCountryByCountryID
        [Fact]
        //If we supply null as CountryID, it should return null as CountryResponse
        public async void GetCountryByCountryID_NullCountryID()
        {
            //Arrange
            Guid? countryID = null;
            //Act
            CountryResponse? country_response_from_get_method = await _countriesService.GetCountryByCountryID(countryID);
            //Assert
            Assert.Null(country_response_from_get_method);
        }

        [Fact]
        //If we supply a valid country id, it should return the matching country details as CountryResponse object.

        //Remember, every test cases run independently. I mean for every test case, Countries List is empty by default.
        public async void GetCountryByCountryID_ValidCountryID()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "China" };
            CountryResponse country_response_from_add = await _countriesService.AddCountry(countryAddRequest);

            //Act
            CountryResponse? country_response_from_get = await _countriesService.GetCountryByCountryID(country_response_from_add.CountryID);

            //Assert
            Assert.Equal(country_response_from_add, country_response_from_get);
        }

        #endregion
    }
}
