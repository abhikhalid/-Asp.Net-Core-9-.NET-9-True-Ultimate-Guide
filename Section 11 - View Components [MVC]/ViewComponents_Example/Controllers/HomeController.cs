using Microsoft.AspNetCore.Mvc;
using ViewComponents_Example.Models;

namespace ViewComponents_Example.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
        
        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("friends-list")]
        public IActionResult LoadFriendsList()
        {
            PersonGridModel personGridModel = new PersonGridModel()
            {
                GridTitle = "Persons",
                Persons = new List<Person>(){
                   new Person(){ PersonName = "Khalid", JobTitle = "Software Engineer"},
                   new Person(){ PersonName = "Khalid", JobTitle = "Software Engineer"},
                   new Person(){ PersonName = "Khalid", JobTitle = "Software Engineer"},
                   new Person(){ PersonName = "Khalid", JobTitle = "Software Engineer"},
                }
            };

            return ViewComponent("Grid", new { personGridModel = personGridModel });
        }
    }
}
