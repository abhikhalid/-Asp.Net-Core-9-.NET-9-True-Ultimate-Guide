using Microsoft.AspNetCore.Mvc;
using Section_08_Razor_Views.Models;

namespace Section_08_Razor_Views.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        [Route("/")]
        public IActionResult Index()
        {
            ViewData["appTitle"] = "Asp.Next Core Demo App";

            List<Person> people = new List<Person>(){
                  new Person(){Name="John", DateOfBirth = DateTime.Parse("2000-05-06"), PersonGender = Gender.Male},
                    new Person(){Name="Jane", DateOfBirth = DateTime.Parse("2000-05-06"), PersonGender = Gender.Male},
                    new Person(){Name="Jack", DateOfBirth = DateTime.Parse("2000-05-06"), PersonGender = Gender.Male}
                };

            //ViewData["people"] = people;
            ViewBag.people = people;

            return View(); 
        }
    }
}
