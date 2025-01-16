using Microsoft.AspNetCore.Mvc;

namespace PartialViewExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            ViewData["ListTitle"] = "Cities";
            ViewData["ListItems"] = new List<string>()
            {
                "New York",
                "London",
                "Paris",
                "Tokyo"
            };

            return View();
        }

        [Route("/about")]
        public IActionResult About()
        {
            return View();
        }
    }
}
