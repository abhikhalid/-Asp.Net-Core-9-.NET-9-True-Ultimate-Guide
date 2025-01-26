using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using ServiceContracts.Enums;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CRUDTests
{
    public class PersonsServiceTest
    { 
        // private fields
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        //constructor
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personsService = new PersonsService();
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson
        //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Act
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personsService.AddPerson(personAddRequest);
            });
        }

        //When we supply null value as PersonName, it should throw ArgumetException
        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = null,
            };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.AddPerson(personAddRequest);
            });
        }


        //When we supply proper person details, it should insert the person into the persons list and it should return an object of PersonResponse,
        //which includes with the newly genarated person id
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Person Name...",
                Email = "person@example.com",
                Address = "sample address",
                CountryID = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true,
            };

            //Act
            PersonResponse person_response_from_add =  _personsService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = _personsService.GetAllPersons();

            //Assert
            Assert.True(person_response_from_add.PersonID != Guid.Empty);
            Assert.Contains(person_response_from_add, persons_list);
        }
        #endregion

        #region GetPersonByPersonId
        //If we supply null as PersonID, it should return null as PersonResponse
        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Guid? personID = null;

            //Act
            PersonResponse? person_response_from_get = _personsService.GetPersonByPersonId(personID);

            //Assert
            Assert.Null(person_response_from_get);
        }

        //If we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public void GetPersonByPersonID_WithPersonID()
        {
            //Arrange
            CountryAddRequest country_request = new CountryAddRequest()
            {
                CountryName = "Canada"
            };

            CountryResponse countryResponse = _countriesService.AddCountry(country_request);

            //Act
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Person Name..",
                Email = "email@sample.com",
                Address = "address",
                CountryID = countryResponse.CountryID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };

            PersonResponse person_response_from_add = _personsService.AddPerson(personAddRequest);

            PersonResponse? person_response_from_get =  _personsService.GetPersonByPersonId(person_response_from_add.PersonID);
            //Assert
            Assert.Equal(person_response_from_add,person_response_from_get);
        }
        #endregion

        #region GetAllPersons
        //The GetAllPersons() should return an empty list by default
        public void GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> person_from_get = _personsService.GetAllPersons();

            //Assert
            Assert.Empty(person_from_get);
        }

        //First, we will add few persons, and then when we call GetPersons(), it should return the same persons that were added.
        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "Canada" };
            
            CountryResponse country_response_1 =  _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "smith@example.com",
                Gender = GenderOptions.Male,
                Address = "address of smith",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters  = true,
            };
            
            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "mary@example.com",
                Gender = GenderOptions.Female,
                Address = "address of mary",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters  = true,
            };
            
            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Smith 3",
                Email = "smith@example.com",
                Gender = GenderOptions.Male,
                Address = "address of smith",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters  = true,
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach(PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");

            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> person_list_from_get =  _personsService.GetAllPersons();

            //print person_list_from_get
            _testOutputHelper.WriteLine("Actual:");

            foreach (PersonResponse person_from_get in person_list_from_get)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
            }

            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, person_list_from_get);
            }
        }
        #endregion
    }
}
