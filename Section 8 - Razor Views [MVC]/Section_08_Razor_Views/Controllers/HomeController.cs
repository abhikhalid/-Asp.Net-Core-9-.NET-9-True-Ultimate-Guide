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
            //ViewBag.people = people;

            return View("Index", people);
        }

        [Route("person-details/{name}")]
        public IActionResult Details(string? name)
        {
            if(name == null)
            {
                return Content("Person name can't be null");
            }

            List<Person> people = new List<Person>(){
                  new Person(){Name="John", DateOfBirth = DateTime.Parse("2000-05-06"), PersonGender = Gender.Male},
                    new Person(){Name="Jane", DateOfBirth = DateTime.Parse("2000-05-06"), PersonGender = Gender.Male},
                    new Person(){Name="Jack", DateOfBirth = DateTime.Parse("2000-05-06"), PersonGender = Gender.Male}
            };

            Person? matchingPerson = people.Where(temp => temp.Name == name).FirstOrDefault();

            //return View("Details", matchingPerson);
            //or
            return View(matchingPerson); //Views/Home/Details.cshtml
        }

        [Route("person-with-product")]
        public IActionResult PersonWithProduct()
        {
            Person person = new Person()
            {
                Name = "John",
                DateOfBirth = DateTime.Parse("2000-05-06"),
                PersonGender = Gender.Male
            };

            Product product = new Product()
            {
                ProductId = 101,
                ProductName = "Laptop"
            };

            PersonAndProductWrapperModel personAndProductWrapperModel = new PersonAndProductWrapperModel()
            {
                PersonData = person,
                ProductData = product
            };

            return View(personAndProductWrapperModel);
        }


        [Route("home/all-products")]
        public IActionResult All()
        {
            return View();
            //Views/Home/All.cshtml
            //Views/Shared/All.cshtml
        }
    }

}