using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace CRUDExample.Filters.ActionFilters
{
    //we want to apply this action filter to the PersonsList action method in the HomeController
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger;

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger;
        }

        //To do: add before logic here
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("PersonsListActionFilter.OnActionExecuted method");
        }

        //To do: add before logic here
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("PersonsListActionFilter.OnActionExecuting method");

            if(context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);

                //validate the searchBy parameter value
                if (!string.IsNullOrEmpty(searchBy))
                {
                    var searchByOptions = new List<string>() {
                        nameof(PersonResponse.PersonName),
                        nameof(PersonResponse.Email),
                        nameof(PersonResponse.DateOfBirth),
                        nameof(PersonResponse.Gender),
                        nameof(PersonResponse.CountryID),
                        nameof(PersonResponse.Address),
                    };

                    //reset the searchBy parameter value.
                    if(searchByOptions.Any(temp => temp == searchBy) == false)
                    {
                        _logger.LogInformation("SearchBy actual value {searchBy}",searchBy);
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                        _logger.LogInformation("SearchBy updated value {searchBy}", context.ActionArguments["searchBy"]);
                    }
                }
            }
        }
    }
}
