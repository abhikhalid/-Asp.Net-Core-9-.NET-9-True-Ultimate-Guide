﻿using CRUDExample.Filters.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        //private fields
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly IPersonsDeleterService _personsDeleterervice;


        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(IPersonsGetterService personsGetterService, IPersonsAdderService personsAdderService, IPersonsSorterService personsSorterService, IPersonsUpdaterService personsUpdaterService, IPersonsDeleterService personsDeleterService , ICountriesGetterService countriesGetterService, ILogger<PersonsController> logger)
        {
            _personsGetterService = personsGetterService;
            _personsAdderService = personsAdderService;
            _personsSorterService = personsSorterService;
            _personsUpdaterService = personsUpdaterService;
            _personsDeleterervice = personsDeleterService;
            _countriesGetterService = countriesGetterService;
            _logger = logger;
        }


        //[Route("index")]
        [Route("[action]")]
        [Route("/")]
        [TypeFilter(typeof(PersonsListActionFilter))]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy= nameof(PersonResponse.PersonName),SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index action method of PersonsController");
            _logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, sortBy: {sortBy}, sortOrder: {sortOrder}");

            //Search
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName), "Person Name"},
                {nameof(PersonResponse.Email), "Email"},
                {nameof(PersonResponse.DateOfBirth), "Date of Birth"},
                {nameof(PersonResponse.Gender), "Gender"},
                {nameof(PersonResponse.CountryID), "Country"},
                {nameof(PersonResponse.Address), "Address"},
            };


            List<PersonResponse> persons = await _personsGetterService.GetFilteredPersons(searchBy,searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sort
            List<PersonResponse> sortedPersons = await _personsSorterService.GetSortedPersons(persons,sortBy,sortOrder);
            ViewBag.CurrentSortBy = sortBy; 
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPersons); //Views/Persons/Index.cshtml
        }

        /// <summary>
        /// Executes when the user clicks on "Create Person" hyperlink from ("/") route(while opening the create view)
        /// </summary>
        //[Route("create")]
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countries = await _countriesGetterService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp => new SelectListItem()
            {
                Text = temp.CountryName,
                Value = temp.CountryID.ToString(),
            });

            //new SelectListItem() { Text = "Harsha", Value = "1"}
            //<option value="1">Harsha</option>

            return View();
        }

        [HttpPost] //when user clicks on Submit button this method gets executed
        //[Route("create")]
        [Route("[action]")]
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid) //before executing this controller method, model validation gets executed
            {
                List<CountryResponse> countries = await _countriesGetterService.GetAllCountries();
                ViewBag.Countries = countries;

                ViewBag.Errros =  ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(personAddRequest);
            }

            PersonResponse personResponse = await _personsAdderService.AddPerson(personAddRequest);
            return RedirectToAction("Index","Persons");
        }

        //for loading edit view
        [HttpGet]
        [Route("[action]/{personID}")] //Eg: /persons/edit/1
        public async Task<IActionResult> Edit(Guid personID)
        {
            PersonResponse? personResponse = await _personsGetterService.GetPersonByPersonId(personID);

            if (personResponse == null)
            {
                return RedirectToAction("Index"); // redirect to 'Index' action method
            }
            
            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = await _countriesGetterService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp => new SelectListItem()
            {
                Text = temp.CountryName,
                Value = temp.CountryID.ToString(),
            });

            return View(personUpdateRequest);
        }

        //on clicking the submit button from edit page
        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = await _personsGetterService.GetPersonByPersonId(personUpdateRequest.PersonID);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                PersonResponse updatedPerson = await _personsUpdaterService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                List<CountryResponse> countries = await _countriesGetterService.GetAllCountries();
                ViewBag.Countries = countries.Select(temp =>
                new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(personResponse.ToPersonUpdateRequest());
            }
        }




        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(Guid? personID)
        {
            PersonResponse? personResponse = await _personsGetterService.GetPersonByPersonId(personID);
            if (personResponse == null)
                return RedirectToAction("Index");

            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateResult)
        {
            PersonResponse? personResponse = await _personsGetterService.GetPersonByPersonId(personUpdateResult.PersonID);
            if (personResponse == null)
                return RedirectToAction("Index");

            await _personsDeleterervice.DeletePerson(personUpdateResult.PersonID);
            return RedirectToAction("Index");
        }

        [Route("PersonsPDF")]
        public async Task<IActionResult> PersonsPDF()
        {
            //Get list of persons
            List<PersonResponse> persons =  await _personsGetterService.GetAllPersons();

            //Return view as pdf

            //by default, every controller action method contains some 'ViewData'. We have to supply the same.
            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Bottom = 20,
                    Left = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }


        [Route("PersonsCSV")]
        public async Task<IActionResult> PersonsCSV()
        {
           MemoryStream memoryStream = await _personsGetterService.GetPersonsCSV();
           
           return File(memoryStream,"application/octet-stream","persons.csv");
        } 
        
        
        [Route("PersonsExcel")]
        public async Task<IActionResult> PersonsExcel()
        {
           MemoryStream memoryStream = await _personsGetterService.GetPersonsExcel();
           
           return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}
