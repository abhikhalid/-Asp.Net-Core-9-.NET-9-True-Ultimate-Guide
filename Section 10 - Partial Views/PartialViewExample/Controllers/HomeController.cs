using Microsoft.AspNetCore.Mvc;
using PartialViewExample.Models;

namespace PartialViewExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("/programming-languages")]
        public IActionResult ProgrammingLanguages()
        {
            ListModel listModel = new ListModel()
            {
                ListTitle = "Programming Langauges List",
                ListItems = new List<string>()
                {
                    "C#",
                    "Java",
                    "Python",
                    "JavaScript",
                    "TypeScript",
                    "C++",
                    "PHP",
                    "Ruby",
                    "Swift",
                    "Kotlin"
                }
            };

            return PartialView("_ListPartialView",listModel);
        }
    }
}
