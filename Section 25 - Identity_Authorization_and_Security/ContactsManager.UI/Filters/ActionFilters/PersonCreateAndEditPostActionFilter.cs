using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;

namespace CRUDExample.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ILogger<PersonCreateAndEditPostActionFilter> _logger;

        public PersonCreateAndEditPostActionFilter(ICountriesGetterService countriesGetterService, ILogger<PersonCreateAndEditPostActionFilter> logger)
        {
            _countriesGetterService = countriesGetterService;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //TO DO: before logic
            if(context.Controller is PersonsController personsController)
            {
                if (!personsController.ModelState.IsValid) //before executing this controller method, model validation gets executed
                { 
                    List<CountryResponse> countries = await _countriesGetterService.GetAllCountries();
                    personsController.ViewBag.Countries = countries.Select(temp => new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString()});

                    personsController.ViewBag.Errros = personsController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                        
                    var personAddRequest = context.ActionArguments["personRequest"];

                    context.Result = personsController.View(personAddRequest); // short-circuits or skips the subsequent action filters & action method
                }
                else
                {
                    await next(); //invokes the subsequent filters or action method
                }
            }
            else
            {
                await next(); //invokes the subsequent filters or action method
            }    
            //await next();
            //TO DO: after logic

            _logger.LogInformation("In after logic of PersonsCreateAndEdit Action filter");           
        }
    }
}
