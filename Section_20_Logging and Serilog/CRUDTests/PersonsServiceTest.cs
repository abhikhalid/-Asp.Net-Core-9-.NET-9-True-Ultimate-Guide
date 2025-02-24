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
using RepositoryContracts;
using Moq;
using System.Linq.Expressions;
using Serilog.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.Logging;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        // private fields
        private readonly IPersonsService _personsService;

        private readonly Mock<IPersonsRepository> _personRepositoryMock;
        private readonly IPersonsRepository _personsRepository;

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        //constructor
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _fixture = new Fixture();

            _personRepositoryMock = new Mock<IPersonsRepository>();
            _personsRepository = _personRepositoryMock.Object;

            var loggerMock = new Mock<ILogger<PersonsService>>();
            var diagonsticContextMock = new Mock<IDiagnosticContext>();
            _personsService = new PersonsService(_personsRepository, loggerMock.Object, diagonsticContextMock.Object); 
        }

        #region AddPerson
        //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
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
                await _personsService.AddPerson(personAddRequest); //it does not call the repository method, so we also do not need to mock the repository method. 'AddPerson'
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply null value as PersonName, it should throw ArgumetException
        [Fact]
        public async Task AddPerson_PersonNameIsNull_ToBeArgumentException()
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

            Person person = personAddRequest.ToPerson();
            //When PersonRepository.AddPerson is called, it has to return the same 'person' object.
            _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            Func<Task> action = async () =>
            {
                await _personsService.AddPerson(personAddRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }


        //When we supply proper person details, it should insert the person into the persons list and it should return an object of PersonResponse,
        //which includes with the newly genarated person id
        [Fact]
        public async Task AddPerson_FullPersonDetails_ToBeSuccessful()
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

            Person person = personAddRequest.ToPerson();
            PersonResponse person_respose_expected = person.ToPersonResponse();

            //If we supply any argument value to the AddPerson method, it should return the same return value
            //_personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(new Person());
            _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            //Act

            //we should not call or test more than one method in unit test case. so comment it
            //List<PersonResponse> persons_list = await _personsService.GetAllPersons();
            PersonResponse person_response_from_add = await _personsService.AddPerson(personAddRequest);
            person_respose_expected.PersonID = person_response_from_add.PersonID;
            //Assert
            person_response_from_add.PersonID.Should().NotBe(Guid.Empty);
            person_response_from_add.Should().Be(person_respose_expected);
        }
        #endregion

        #region GetPersonByPersonId
        //If we supply null as PersonID, it should return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonID_NullPersonID_ToBeNull()
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
        public async Task GetPersonByPersonID_WithPersonID_ToBeSucessful()
        {
            //Arrange
            Person person = _fixture.Build<Person>()
                                    .With(temp => temp.Email, "email@sample.com")
                                    .With(temp => temp.Country, null as Country)
                                    .Create();

            PersonResponse person_response_expected = person.ToPersonResponse();

            _personRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);

            //Act
            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonId(person.PersonID);
            
            //Assert
            person_response_from_get.Should().Be(person_response_expected);
        }
        #endregion

        #region GetAllPersons
        
        //The GetAllPersons() should return an empty list by default
        [Fact]
        public async Task GetAllPersons_ToBeEmptyList()
        {
            //Arrange
            var persons = new List<Person>();
            _personRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);


            //Act
            List<PersonResponse> person_from_get = await _personsService.GetAllPersons();

            //Assert
            //Assert.Empty(person_from_get);
            person_from_get.Should().BeEmpty();
        }

        //First, we will add few persons, and then when we call GetPersons(), it should return the same persons that were added.
        [Fact]
        public async Task GetAllPersons_WithFewPersons_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>() {
                _fixture.Build<Person>()
                .With(temp => temp.Email, "email@sample.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(temp => temp.Email, "dfsdf@sample.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(temp => temp.Email, "sdfdf@sample.com")
                .With(temp => temp.Country, null as Country)
                .Create(),
            };


            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");

            foreach (PersonResponse person_response_from_add in person_response_list_expected)
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

            _personRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

            //Assert
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    Assert.Contains(person_response_from_add, person_list_from_get);
            //}

            person_list_from_get.Should().BeEquivalentTo(person_response_list_expected);
        }
        #endregion

        #region GetFilteredPersons
        //If the saerch text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>() {
                _fixture.Build<Person>()
                .With(temp => temp.Email, "email@sample.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(temp => temp.Email, "dfsdf@sample.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(temp => temp.Email, "sdfdf@sample.com")
                .With(temp => temp.Country, null as Country)
                .Create(),
            };
            
            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");

            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            _personRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);

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
            person_list_from_search.Should().BeEquivalentTo(person_response_list_expected);
        }
        //at your practice time, you can avoid the code repetition here. 


        //First we will add few persons, and then will search based on person name with some search string. It should return the matching person.
        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>() {
                _fixture.Build<Person>()
                .With(temp => temp.Email, "email@sample.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(temp => temp.Email, "dfsdf@sample.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(temp => temp.Email, "sdfdf@sample.com")
                .With(temp => temp.Country, null as Country)
                .Create(),
            };

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");

            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            _personRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);

            //Act
            List<PersonResponse> person_list_from_search = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "sa");

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
            person_list_from_search.Should().BeEquivalentTo(person_response_list_expected);
        }

        #endregion

        //When we sort based on the PersonName in DESC, it should return persons list in descending on PersonName
        #region GetSortedPersons
        [Fact]
        public async Task GetSortedPersons_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>() {
                _fixture.Build<Person>()
                .With(temp => temp.Email, "email@sample.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(temp => temp.Email, "dfsdf@sample.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(temp => temp.Email, "sdfdf@sample.com")
                .With(temp => temp.Country, null as Country)
                .Create(),
            };

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();
            _personRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");

            foreach (PersonResponse person_response_from_add in person_response_list_expected)
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
        public async Task UpdatePerson_NullPerson_ToBeArgumentNullException()
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
        public async Task UpdatePerson_InvalidPersonID_ToBeArgumentException()
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
        public async Task UpdatePerson_PersonNameIsNull_ToBeArgumentException()
        {
            Person person = _fixture.Build<Person>()
                        .With(temp => temp.PersonName, null as string)
                        .With(temp => temp.Email, "khalid@gmail.com")
                        .With(temp => temp.Country, null as Country)
                        .With(temp => temp.Gender, "Male")
                        .Create();

            PersonResponse person_response_from_add = person.ToPersonResponse();

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();

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
        public async Task UpdatePerson_PersonFullDetails_ToBeSuccessful()
        {
            //Arrange
            Person person = _fixture.Build<Person>()
                .With(temp => temp.Email, "abc@gmail.com")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .Create();

            PersonResponse person_response_expected = person.ToPersonResponse();

            PersonUpdateRequest person_update_request = person_response_expected.ToPersonUpdateRequest();

            _personRepositoryMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person);
            _personRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);
            
            //Act
            PersonResponse person_response_from_update = await _personsService.UpdatePerson(person_update_request);

            //Assert
            //Assert.Equal(person_response_from_get, person_response_from_update);
            person_response_from_update.Should().Be(person_response_expected);
        }

        #endregion

        #region DeletePerson
        //When you supply an valid PersonID, it should return true
        [Fact]
        public async Task DeletePerson_ValidPersonID()
        {
            Person person = _fixture.Build<Person>()
                                   .With(temp => temp.Email, "someone@example.com")
                                   .With(temp => temp.Country, null as Country)
                                   .With(temp => temp.Gender, "Female")
                                   .Create();

            _personRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);
            _personRepositoryMock.Setup(temp => temp.DeletePersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(true);

            //Act
            bool isDeleted = await _personsService.DeletePerson(person.PersonID);

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
