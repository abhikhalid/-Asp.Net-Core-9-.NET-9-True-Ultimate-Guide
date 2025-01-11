using Microsoft.AspNetCore.Mvc;
using ModelValidationsExample.CustomModelBinders;
using ModelValidationsExample.Models;

namespace ModelValidationsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("register")]
        //model binding executes before this action method executes
        //public IActionResult Index([Bind(nameof(Person.PersonName),nameof(Person.Email),nameof(Person.Password),nameof(Person.ConfirmPassword))]Person person)
        public IActionResult Index([FromBody][ModelBinder(BinderType = typeof(PersonModelBinder))] Person person)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            return Content($"{person}");
        }
    }
}
