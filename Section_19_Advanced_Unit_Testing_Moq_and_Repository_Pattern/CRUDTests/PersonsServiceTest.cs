using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using ServiceContracts.Enums;
using Xunit.Abstractions;
using Xunit.Sdk;
using Entities;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using AutoFixture;
using FluentAssertions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        // private fields
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        //constructor
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            //_countriesService = new CountriesService(new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options));
            //_personsService = new PersonsService(new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options),_countriesService);
            //_testOutputHelper = testOutputHelper;

            var countriesInitialData = new List<Country>() { }; // I want the countries table by default.
            var personsInitialData = new List<Person>() { }; // I want the countries table by default.

            DbContextMock<ApplicationDbContext> dbContextMock = new
                DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            ApplicationDbContext dbContext = dbContextMock.Object; //hey dbContextMock, give me an instance of ApplicationDbContext
            //now, we have to mock dbSets
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);


            _countriesService = new CountriesService(dbContext);
            _personsService = new PersonsService(dbContext, _countriesService);
            _testOutputHelper = testOutputHelper;
            _fixture = new Fixture();
        }

        #region AddPerson
        //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Act
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    await _personsService.AddPerson(personAddRequest);
            //});

            Func<Task> action = async () =>
            {
                await _personsService.AddPerson(personAddRequest);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply null value as PersonName, it should throw ArgumetException
        [Fact]
        public async Task AddPerson_PersonNameIsNull()
        {
            //Arrange
            //PersonAddRequest? personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = null,
            //};

            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                                                 .With(temp => temp.PersonName, null as string)
                                                 .Create();

            //Act
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _personsService.AddPerson(personAddRequest);
            //});
            Func<Task> action = async () =>
            {
                await _personsService.AddPerson(personAddRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }


        //When we supply proper person details, it should insert the person into the persons list and it should return an object of PersonResponse,
        //which includes with the newly genarated person id
        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
            //Arrange
            //PersonAddRequest? personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = "Person Name...",
            //    Email = "person@example.com",
            //    Address = "sample address",
            //    CountryID = Guid.NewGuid(),
            //    Gender = GenderOptions.Male,
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    ReceiveNewsLetters = true,
            //};

            //create method sets the object immediately.
            //PersonAddRequest? personAddRequest = _fixture.Create<PersonAddRequest>();

            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                                                         .With(temp => temp.Email, "someone@example.com")
                                                         .Create();

            //Act
            PersonResponse person_response_from_add = await _personsService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = await _personsService.GetAllPersons();

            //Assert
            //Assert.True(person_response_from_add.PersonID != Guid.Empty);
            person_response_from_add.PersonID.Should().NotBe(Guid.Empty);
            //Assert.Contains(person_response_from_add, persons_list);
            //actual should be first
            persons_list.Should().Contain(person_response_from_add);
        }
        #endregion

        #region GetPersonByPersonId
        //If we supply null as PersonID, it should return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Guid? personID = null;

            //Act
            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonId(personID);

            //Assert
            //Assert.Null(person_response_from_get);
            person_response_from_get.Should().BeNull();
        }

        //If we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public async Task GetPersonByPersonID_WithPersonID()
        {
            //Arrange
            //CountryAddRequest country_request = new CountryAddRequest()
            //{
            //    CountryName = "Canada"
            //};

            CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse = await _countriesService.AddCountry(country_request);

            //Act
            //PersonAddRequest personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = "Person Name..",
            //    Email = "email@sample.com",
            //    Address = "address",
            //    CountryID = countryResponse.CountryID,
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    Gender = GenderOptions.Male,
            //    ReceiveNewsLetters = false
            //};

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com")
                                                .Create();

            PersonResponse person_response_from_add = await _personsService.AddPerson(personAddRequest);

            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonId(person_response_from_add.PersonID);
            //Assert
            //Assert.Equal(person_response_from_add, person_response_from_get);
            person_response_from_get.Should().Be(person_response_from_add);
        }
        #endregion

        #region GetAllPersons
        //The GetAllPersons() should return an empty list by default
        public async Task GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> person_from_get = await _personsService.GetAllPersons();

            //Assert
            //Assert.Empty(person_from_get);
            person_from_get.Should().BeEmpty();
        }

        //First, we will add few persons, and then when we call GetPersons(), it should return the same persons that were added.
        [Fact]
        public async Task GetAllPersons_AddFewPersons()
        {
            //Arrange
            //CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            //CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "Canada" };
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();


            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            //PersonAddRequest person_request_1 = new PersonAddRequest()
            //{
            //    PersonName = "Smith",
            //    Email = "smith@example.com",
            //    Gender = GenderOptions.Male,
            //    Address = "address of smith",
            //    CountryID = country_response_1.CountryID,
            //    DateOfBirth = DateTime.Parse("2002-05-06"),
            //    ReceiveNewsLetters = true,
            //};

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").Create();

            //PersonAddRequest person_request_2 = new PersonAddRequest()
            //{
            //    PersonName = "Mary",
            //    Email = "mary@example.com",
            //    Gender = GenderOptions.Female,
            //    Address = "address of mary",
            //    CountryID = country_response_1.CountryID,
            //    DateOfBirth = DateTime.Parse("2002-05-06"),
            //    ReceiveNewsLetters = true,
            //};

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                                                .With(temp => temp.Email, "dfsdf@sample.com").Create();

            //PersonAddRequest person_request_3 = new PersonAddRequest()
            //{
            //    PersonName = "Smith 3",
            //    Email = "smith@example.com",
            //    Gender = GenderOptions.Male,
            //    Address = "address of smith",
            //    CountryID = country_response_1.CountryID,
            //    DateOfBirth = DateTime.Parse("2002-05-06"),
            //    ReceiveNewsLetters = true,
            //};

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                                                .With(temp => temp.Email, "sdfdf@sample.com")
                                                .Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");

            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> person_list_from_get = await _personsService.GetAllPersons();

            //print person_list_from_get
            _testOutputHelper.WriteLine("Actual:");

            foreach (PersonResponse person_from_get in person_list_from_get)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
            }

            //Assert
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    Assert.Contains(person_response_from_add, person_list_from_get);
            //}

            person_list_from_get.Should().BeEquivalentTo(person_response_list_from_add);
        }
        #endregion

        #region GetFilteredPersons
        //If the saerch text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            //CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            //CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "Canada" };

            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            //PersonAddRequest person_request_1 = new PersonAddRequest()
            //{
            //    PersonName = "Smith",
            //    Email = "smith@example.com",
            //    Gender = GenderOptions.Male,
            //    Address = "address of smith",
            //    CountryID = country_response_1.CountryID,
            //    DateOfBirth = DateTime.Parse("2002-05-06"),
            //    ReceiveNewsLetters = true,
            //};

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "abc@gmail.com").Create();

            //PersonAddRequest person_request_2 = new PersonAddRequest()
            //{
            //    PersonName = "Mary",
            //    Email = "mary@example.com",
            //    Gender = GenderOptions.Female,
            //    Address = "address of mary",
            //    CountryID = country_response_1.CountryID,
            //    DateOfBirth = DateTime.Parse("2002-05-06"),
            //    ReceiveNewsLetters = true,
            //};

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "khalid@gmail.com").Create();

            //PersonAddRequest person_request_3 = new PersonAddRequest()
            //{
            //    PersonName = "Smith 3",
            //    Email = "smith@example.com",
            //    Gender = GenderOptions.Male,
            //    Address = "address of smith",
            //    CountryID = country_response_1.CountryID,
            //    DateOfBirth = DateTime.Parse("2002-05-06"),
            //    ReceiveNewsLetters = true,
            //};

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "khalid@gmail.com").Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");

            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> person_list_from_search = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "");

            //print person_list_from_get
            _testOutputHelper.WriteLine("Actual:");

            foreach (PersonResponse person_from_get in person_list_from_search)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
            }

            //Assert
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    Assert.Contains(person_response_from_add, person_list_from_search);
            //}
            person_list_from_search.Should().BeEquivalentTo(person_response_list_from_add);
        }
        //at your practice time, you can avoid the code repetition here. 


        //First we will add few persons, and then will search based on person name with some search string. It should return the matching person.
        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            //CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            //CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "Canada" };
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            //PersonAddRequest person_request_1 = new PersonAddRequest()
            //{
            //    PersonName = "Smith",
            //    Email = "smith@example.com",
            //    Gender = GenderOptions.Male,
            //    Address = "address of smith",
            //    CountryID = country_response_1.CountryID,
            //    DateOfBirth = DateTime.Parse("2002-05-06"),
            //    ReceiveNewsLetters = true,
            //};
            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                                                .With(temp => temp.PersonName, "Rahman")
                                                .With(temp => temp.Email, "smith@example.com")
                                                .With(temp => temp.CountryID, country_response_1.CountryID)
                                                .Create();

            //PersonAddRequest person_request_2 = new PersonAddRequest()
            //{
            //    PersonName = "Mary",
            //    Email = "mary@example.com",
            //    Gender = GenderOptions.Female,
            //    Address = "address of mary",
            //    CountryID = country_response_1.CountryID,
            //    DateOfBirth = DateTime.Parse("2002-05-06"),
            //    ReceiveNewsLetters = true,
            //};
            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                                                .With(temp => temp.PersonName, "Mary")
                                                .With(temp => temp.Email, "mary@example.com")
                                                .With(temp => temp.CountryID, country_response_2.CountryID)
                                                .Create();

            //PersonAddRequest person_request_3 = new PersonAddRequest()
            //{
            //    PersonName = "Smith 3",
            //    Email = "smith@example.com",
            //    Gender = GenderOptions.Male,
            //    Address = "address of smith",
            //    CountryID = country_response_1.CountryID,
            //    DateOfBirth = DateTime.Parse("2002-05-06"),
            //    ReceiveNewsLetters = true,
            //};

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                                                 .With(temp => temp.CountryID, country_response_1.CountryID)
                                                .With(temp => temp.Email, "smith@example.com").Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");

            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_search = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            //Assert
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    if (person_response_from_add.PersonName != null)
            //    {
            //        if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
            //        {
            //            Assert.Contains(person_response_from_add, persons_list_from_search);
            //        }
            //    }
            //}

            persons_list_from_search.Should().OnlyContain(temp => temp.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        //When we sort based on the PersonName in DESC, it should return persons list in descending on PersonName
        #region GetSortedPersons
        [Fact]
        public async Task GetSortedPersons()
        {
            //Arrange
            //CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            //CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "Canada" };

            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            //PersonAddRequest person_request_1 = new PersonAddRequest()
            //{
            //    PersonName = "Smith",
            //    Email = "smith@example.com",
            //    Gender = GenderOptions.Male,
            //    Address = "address of smith",
            //    CountryID = country_response_1.CountryID,
            //    DateOfBirth = DateTime.Parse("2002-05-06"),
            //    ReceiveNewsLetters = true,
            //};
            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>().With(temp => temp.CountryID, country_response_1.CountryID).With(temp => temp.Email, "abc@gmail.com").Create();

            //PersonAddRequest person_request_2 = new PersonAddRequest()
            //{
            //    PersonName = "Mary",
            //    Email = "mary@example.com",
            //    Gender = GenderOptions.Female,
            //    Address = "address of mary",
            //    CountryID = country_response_1.CountryID,
            //    DateOfBirth = DateTime.Parse("2002-05-06"),
            //    ReceiveNewsLetters = true,
            //};
            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>().With(temp => temp.CountryID, country_response_1.CountryID).With(temp => temp.Email, "csdfsd@gmail.com").Create();


            //PersonAddRequest person_request_3 = new PersonAddRequest()
            //{
            //    PersonName = "Smith 3",
            //    Email = "smith@example.com",
            //    Gender = GenderOptions.Male,
            //    Address = "address of smith",
            //    CountryID = country_response_1.CountryID,
            //    DateOfBirth = DateTime.Parse("2002-05-06"),
            //    ReceiveNewsLetters = true,
            //};

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>().With(temp => temp.CountryID, country_response_2.CountryID).With(temp => temp.Email, "smith@example.com").Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");

            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> allPersons = await _personsService.GetAllPersons();

            List<PersonResponse> persons_list_from_sort = await _personsService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            //print persons_list_from_get
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_response_from_get in persons_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //person_response_list_from_add = person_response_list_from_add.OrderByDescending(temp => temp.PersonName).ToList();

            //Assert
            //for (int i = 0; i < person_response_list_from_add.Count; i++)
            //{
            //    Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
            //}

            persons_list_from_sort.Should().BeInDescendingOrder(temp =>  temp.PersonName);

        }
        #endregion

        #region UpdatePerson
        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = null;

            //Assert
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    //Act
            //    await _personsService.UpdatePerson(person_update_request);
            //});

            Func<Task> action = async () =>
            {
                
                await _personsService.UpdatePerson(person_update_request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = new PersonUpdateRequest()
            {
                PersonID = Guid.NewGuid()
            };

            ////Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    //Act
            //    await _personsService.UpdatePerson(person_update_request);
            //});

            //Assert
            Func<Task> action =  async () =>
            {
                //Act
                await _personsService.UpdatePerson(person_update_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //when PersonName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            //CountryAddRequest country_add_request = new CountryAddRequest()
            //{
            //    CountryName = "UK"
            //};

            CountryAddRequest country_add_request = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_from_add =
            await _countriesService.AddCountry(country_add_request);

            //PersonAddRequest person_add_request = new PersonAddRequest()
            //{
            //    PersonName = "John",
            //    CountryID = country_response_from_add.CountryID,
            //    Email = "abc@example.com",
            //    Address = "Dhaka",
            //    Gender = GenderOptions.Male
            //};
            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "khalid@gmail.com").Create();

            PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = null;

            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _personsService.UpdatePerson(person_update_request);
            //});
            Func<Task> action = async () =>
            {
                await _personsService.UpdatePerson(person_update_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        // First add a new person and try to update the person name and email
        [Fact]
        public async Task UpdatePerson_PersonFullDetailsUpdation()
        {
            //Arrange
            //CountryAddRequest country_add_request = new CountryAddRequest()
            //{
            //    CountryName = "UK"
            //};
            CountryAddRequest country_add_request = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_from_add =
            await _countriesService.AddCountry(country_add_request);

            //PersonAddRequest person_add_request = new PersonAddRequest()
            //{
            //    PersonName = "John",
            //    CountryID = country_response_from_add.CountryID,
            //    Address = "Dhaka",
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    Email = "abc@example.com",
            //    Gender = GenderOptions.Male,
            //    ReceiveNewsLetters = true,
            //};

            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "abc@gmail.com").Create();

            PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = "William";
            person_update_request.Email = "william@example.com";

            //Act
            PersonResponse person_response_from_update = await _personsService.UpdatePerson(person_update_request);

            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonId(
                person_response_from_update.PersonID);

            //Assert
            //Assert.Equal(person_response_from_get, person_response_from_update);
            person_response_from_update.Should().Be(person_response_from_get);
        }

        #endregion

        #region DeletePerson
        //When you supply an valid PersonID, it should return true
        [Fact]
        public async Task DeletePerson_ValidPersonID()
        {
            //Arrange
            //CountryAddRequest counry_add_request = new CountryAddRequest()
            //{
            //    CountryName = "USA"
            //};

            CountryAddRequest counry_add_request = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_from_add =
            await _countriesService.AddCountry(counry_add_request);

            //PersonAddRequest person_add_request = new PersonAddRequest()
            //{
            //    PersonName = "Jones",
            //    Address = "address",
            //    CountryID = country_response_from_add.CountryID,
            //    DateOfBirth = Convert.ToDateTime("2010-01-01"),
            //    Email = "jones@example.com",
            //    Gender = GenderOptions.Male,
            //    ReceiveNewsLetters = true,
            //};

            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "asdf@gmail.com").Create();

            PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            //Act
            bool isDeleted = await _personsService.DeletePerson(person_response_from_add.PersonID);

            //Assert
            //Assert.True(isDeleted);
            isDeleted.Should().BeTrue();
        }

        //When you supply an invalid PersonID, it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonID()
        {
            //Act
            bool isDeleted = await _personsService.DeletePerson(Guid.NewGuid());

            //Assert
            //Assert.False(isDeleted);
            isDeleted.Should().BeFalse();
        }
        #endregion
    }
}
