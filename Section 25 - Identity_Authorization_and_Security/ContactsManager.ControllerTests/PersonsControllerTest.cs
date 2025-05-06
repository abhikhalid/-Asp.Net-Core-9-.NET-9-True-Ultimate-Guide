using AutoFixture;
using ContactsManager.Core.Enums;
using CRUDExample.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
//using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;

namespace CRUDTests
{
    public class PersonsControllerTest
    {
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;

        private readonly ICountriesGetterService _countriesGetterService;

        private readonly Mock<ICountriesGetterService> _countriesGetterServiceMock;
        private readonly Mock<IPersonsGetterService> _personGetterServiceMock;
        private readonly Mock<IPersonsAdderService> _personAdderServiceMock;
        private readonly Mock<IPersonsDeleterService> _personDeleterServiceMock;
        private readonly Mock<IPersonsSorterService> _personSorterServiceMock;
        private readonly Mock<IPersonsUpdaterService> _personUpdaterServiceMock;

        private readonly ILogger<PersonsController> _logger;
        private readonly Mock<ILogger<PersonsController>> _loggerMock;


        private readonly Fixture _fixture;

        public PersonsControllerTest()
        {
            _fixture = new Fixture();
            _countriesGetterServiceMock = new Mock<ICountriesGetterService>();

            _personGetterServiceMock = new Mock<IPersonsGetterService>();
            _personAdderServiceMock = new Mock<IPersonsAdderService>();
            _personDeleterServiceMock = new Mock<IPersonsDeleterService>();
            _personSorterServiceMock = new Mock<IPersonsSorterService>();
            _personUpdaterServiceMock = new Mock<IPersonsUpdaterService>();

            _countriesGetterService = _countriesGetterServiceMock.Object;
            _personsGetterService = _personGetterServiceMock.Object;
            _personsAdderService = _personAdderServiceMock.Object;
            _personsDeleterService = _personDeleterServiceMock.Object;
            _personsSorterService = _personSorterServiceMock.Object;
            _personsUpdaterService = _personUpdaterServiceMock.Object;

            _loggerMock = new Mock<ILogger<PersonsController>>();
            _logger = _loggerMock.Object;
        }

        #region Index
        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonsList()
        {
            //Arrange
            List<PersonResponse> persons_response_list = _fixture.Create<List<PersonResponse>>();

            PersonsController personsController = new PersonsController(_personsGetterService,_personsAdderService,_personsSorterService, _personsUpdaterService, _personsDeleterService, _countriesGetterService, _logger);

            _personGetterServiceMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(persons_response_list);
            _personSorterServiceMock.Setup(temp => temp.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>())).ReturnsAsync(persons_response_list);

            //Act
            IActionResult result = await personsController.Index(_fixture.Create<string>(),_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderOptions>());

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<List<PersonResponse>>();
            viewResult.ViewData.Model.Should().Be(persons_response_list);
        }
        #endregion

        #region Create
        //[Fact]
        //public async void Create_IfModelErrors_ToReturnCreateView()
        //{
        //    //Arrange
        //    PersonAddRequest person_add_request = _fixture.Create<PersonAddRequest>();

        //    PersonResponse person_response = _fixture.Create<PersonResponse>();
        //    List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

        //    _countriesGetterServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);
        //    _personAdderServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(person_response);

        //    PersonsController personsController = new PersonsController(_personsGetterService, _personsAdderService, _personsSorterService, _personsUpdaterService, _personsDeleterService, _countriesGetterService, _logger);


        //    //Act
        //    personsController.ModelState.AddModelError("PersonName", "Person Name can't be blank");

        //    IActionResult result = await personsController.Create(person_add_request);

        //    //Assert
        //    ViewResult viewResult = Assert.IsType<ViewResult>(result);
        //    viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
        //    viewResult.ViewData.Model.Should().Be(person_add_request);
        //}

        [Fact]
        public async void Create_IfNoModelErrors_ToReturnRedirectToIndex()
        {
            //Arrange
            PersonAddRequest person_add_request = _fixture.Create<PersonAddRequest>();

            PersonResponse person_response = _fixture.Create<PersonResponse>();
            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            _countriesGetterServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);
            _personAdderServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(person_response);

            PersonsController personsController = new PersonsController(_personsGetterService, _personsAdderService, _personsSorterService, _personsUpdaterService, _personsDeleterService, _countriesGetterService, _logger);


            //Act
            IActionResult result = await personsController.Create(person_add_request);

            //Assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);
            redirectResult.ActionName.Should().Be("Index");
        }
        #endregion
    }
}
